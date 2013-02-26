using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.ObjectModel;
using ECU.Golf.Shared.Models;

namespace ECU.Golf.ViewModels
{
    public enum ScreenTypes
    {
        Home,
        Pairings,
        Tracking,
        HoleStats,
        Research,
        History,
        ShotLog,
        Twitter
    }

    public enum HoleStatTypes
    {
        Front,
        Back,
        AllByHole,
        AllByRank
    }

    public abstract class ViewModelBase : INotifyPropertyChanged, IDisposable
    {

        #region Private Members

        //private string _statusMessageText;
        //private string _statusMessageColor;
        //private string _promptMessage;

        //public delegate void SetStatusBarMsgEventHandler(string msgText, string msgColor);
        public delegate void SelectPlayerEventHandler(Player player);

        //public event SetStatusBarMsgEventHandler SetStatusBarMsg;
        public event SelectPlayerEventHandler SelectPlayerEvent;
        
        #endregion

        #region Properties

        //public string StatusMessageText
        //{
        //    get { return _statusMessageText; }
        //    set { _statusMessageText = value; OnPropertyChanged("StatusMessageText"); }
        //}

        //public string StatusMessageColor
        //{
        //    get { return _statusMessageColor; }
        //    set { _statusMessageColor = value; OnPropertyChanged("StatusMessageColor"); }
        //}

        //public string PromptMessage
        //{
        //    get { return _promptMessage; }
        //    set { _promptMessage = value; OnPropertyChanged("PromptMessage"); }
        //}

        #endregion

        #region Private Methods

        

        #endregion

        #region Protected Methods

        //protected void OnSetStatusBarMsg(string msgText, string msgColor)
        //{
        //    SetStatusBarMsgEventHandler handler = SetStatusBarMsg;

        //    if (handler != null)
        //    {
        //        handler(msgText, msgColor);
        //    }
        //}

        protected void OnSelectPlayer(Player player)
        {
            SelectPlayerEventHandler handler = SelectPlayerEvent;

            if (handler != null)
            {
                handler(player);
            }
        }
                      
        #endregion

        #region Public Methods

        public virtual void Update()
        {

        }

        public virtual void Search(string search)
        {

        }

        #endregion

        #region INotifyPropertyChanges

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        #region IDisposable Members

        //Implement IDisposable.
        public virtual void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
