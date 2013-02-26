using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Windows.Threading;
using System.ComponentModel;
using System.Windows.Input;
using ECU.Golf.Commands;
using ECU.Golf.DataAccess;
using ECU.Golf.Shared.Models;
using System.Configuration;

namespace ECU.Golf.ViewModels
{
    public class ResearchScreenViewModel : ViewModelBase
    {
        #region Private Members

        private List<ChatUser> _chatUsers;
        private List<ChatUser> _chatRecipients;

        private ChatUser _selectedUser;
        private ChatUser _selectedRecipient;

        private string _researchNote;

        private bool _researchNoteHasFocus = false;

        private string _autoRefreshNote;

        private ObservableCollection<ChatMessageViewModel> _chatMessageVMs = new ObservableCollection<ChatMessageViewModel>();

        private string _messageToSend;

        private DelegateCommand _sendMessageCommand;
        private DelegateCommand _saveNoteCommand;

        private DispatcherTimer _refreshTimer;

        #endregion

        #region Properties

        public List<ChatUser> ChatUsers
        {
            get { return _chatUsers; }
            set { _chatUsers = value; OnPropertyChanged("ChatUsers"); }
        }

        public List<ChatUser> ChatRecipients
        {
            get { return _chatRecipients; }
            set { _chatRecipients = value; OnPropertyChanged("ChatRecipients"); }
        }

        public ChatUser SelectedUser
        {
            get { return _selectedUser; }
            set { _selectedUser = value; OnPropertyChanged("SelectedUser"); }
        }

        public ChatUser SelectedRecipient
        {
            get { return _selectedRecipient; }
            set { _selectedRecipient = value; OnPropertyChanged("SelectedRecipient"); }
        }

        public ObservableCollection<ChatMessageViewModel> ChatMessageVMs
        {
            get { return _chatMessageVMs; }
            set { _chatMessageVMs = value; OnPropertyChanged("ChatMessageVMs"); }
        }
        
        public string MessageToSend
        {
            get { return _messageToSend; }
            set { _messageToSend = value; OnPropertyChanged("MessageToSend"); }
        }

        public string ResearchNote
        {
            get { return _researchNote; }
            set { _researchNote = value; OnPropertyChanged("ResearchNote"); }
        }

        public bool ResearchNoteHasFocus
        {
            get { return _researchNoteHasFocus; }
            set { _researchNoteHasFocus = value; OnPropertyChanged("ResearchNoteHasFocus"); }
        }

        #endregion

        #region Constructor

        public ResearchScreenViewModel()
        {
            _refreshTimer = new DispatcherTimer();
            _refreshTimer.Interval = new TimeSpan(0, 0, 1);
            _refreshTimer.Tick += new EventHandler(refreshTimer_Tick);

            loadChatUsers();

            loadChatRecipients();

            loadResearchNote();

            _autoRefreshNote = ConfigurationManager.AppSettings["AutoRefreshResearchNote"].ToString().ToUpper();

            _refreshTimer.Start();
        }

        #endregion

        #region Private Methods

        private void loadChatUsers()
        {
            ChatUsers = DbConnection.GetChatUsers(false);
        }

        private void loadChatRecipients()
        {
            ChatRecipients = DbConnection.GetChatUsers(true);
        }

        private void loadChatMessages()
        {
            var uiDispatcher = Dispatcher.CurrentDispatcher;

            if (_selectedUser != null)
            {
                List<ChatMessage> chatMessages = DbConnection.GetChatMessages(_selectedUser.UserId);
                ObservableCollection<ChatMessageViewModel> chatMessageVMs = new ObservableCollection<ChatMessageViewModel>();

                if (chatMessages != null)
                {
                    foreach (ChatMessage message in chatMessages)
                    {
                        ChatMessageViewModel chatMessageVM = new ChatMessageViewModel();

                        chatMessageVM.MessageId = message.MessageId;
                        chatMessageVM.MessageText = message.MessageText;

                        if (message.Sender.UserId == _selectedUser.UserId)
                        {
                            chatMessageVM.UserName = message.Sender.UserName + " to " + message.Recipient.UserName;
                        }
                        else
                        {
                            chatMessageVM.UserName = message.Sender.UserName;
                        }
                        
                        chatMessageVM.Timestamp = message.Timestamp.ToString();

                        chatMessageVMs.Add(chatMessageVM);
                    }
                    
                    if (chatMessageVMs.Count != _chatMessageVMs.Count)
                    {
                        ChatMessageVMs = chatMessageVMs;
                    }

                }
            }
            
        }

        private void refreshTimer_Tick(object sender, EventArgs e)
        {
            _refreshTimer.Stop();

            BackgroundWorker worker = new BackgroundWorker();
            //worker.WorkerReportsProgress = true;

            worker.DoWork += delegate(object s, DoWorkEventArgs args)
            {
                loadChatMessages();

                if (_autoRefreshNote == "TRUE" || _autoRefreshNote == "YES" || _researchNoteHasFocus == false) { loadResearchNote(); }
            };

            worker.RunWorkerCompleted += delegate(object s, RunWorkerCompletedEventArgs args)
            {
                OnPropertyChanged("ChatMessageVMs");              
                
                _refreshTimer.Start();
            };

            worker.RunWorkerAsync();
        }

        private void sendMessage()
        {
            if (_selectedUser != null && _selectedRecipient != null)
            {
                bool sent = DbConnection.SendChatMessage(_selectedUser.UserId, _selectedRecipient.UserId, _selectedRecipient.UserType, _messageToSend);

                if (sent)
                {
                    MessageToSend = "";
                }
            }
        }

        private void loadResearchNote()
        {
            ResearchNote = DbConnection.GetResearchNote();
        }

        private void saveNote()
        {
            DbConnection.SaveResearchNote(_researchNote);

            _researchNoteHasFocus = false;
        }

        #endregion

        #region Commands

        public ICommand SendMessageCommand
        {
            get
            {
                if (_sendMessageCommand == null)
                {
                    _sendMessageCommand = new DelegateCommand(sendMessage);
                }
                return _sendMessageCommand;
            }
        }

        public ICommand SaveNoteCommand
        {
            get
            {
                if (_saveNoteCommand == null)
                {
                    _saveNoteCommand = new DelegateCommand(saveNote);
                }
                return _saveNoteCommand;
            }
        }

        #endregion

    }
}
