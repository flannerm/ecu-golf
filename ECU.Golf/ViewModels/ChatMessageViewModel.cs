using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ECU.Golf.ViewModels
{
    public class ChatMessageViewModel : ViewModelBase
    {
        #region Private Members

        private int _messageId;
        private string _timestamp;
        private string _userName;
        private string _messageText;

        #endregion

        #region Properties

        public int MessageId
        {
            get { return _messageId; }
            set { _messageId = value; OnPropertyChanged("MessageId"); }
        }

        public string Timestamp
        {
            get { return _timestamp; }
            set { _timestamp = value; OnPropertyChanged("Timestamp"); }
        }

        public string UserName
        {
            get { return _userName; }
            set { _userName = value; OnPropertyChanged("UserName"); }
        }

        public string MessageText
        {
            get { return _messageText; }
            set { _messageText = value; OnPropertyChanged("MessageText"); }
        }

        #endregion

    }
}
