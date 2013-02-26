using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ECU.Golf.Shared.Models
{
    public class Tweet : ModelBase
    {

        #region Private Variables

        private string _tweetID;
        private string _createdAt;
        private string _text;
        private string _user;
        private string _userImage;
        private string _tweetType;
        private string _screenName;

        #endregion

        #region Properties

        public string TweetID
        {
            get { return _tweetID; }
            set { _tweetID = value; OnPropertyChanged("TweetID"); }
        }

        public string CreatedAt
        {
            get { return _createdAt; }
            set { _createdAt = value; OnPropertyChanged("CreatedAt"); }
        }

        public string Text
        {
            get { return _text; }
            set { _text = value; OnPropertyChanged("Text"); }
        }

        public string User
        {
            get { return _user; }
            set { _user = value; OnPropertyChanged("User"); }
        }

        public string UserImage
        {
            get { return _userImage; }
            set { _userImage = value; OnPropertyChanged("UserImage"); }
        }

        public string TweetType
        {
            get { return _tweetType; }
            set { _tweetType = value; OnPropertyChanged("TweetType"); }
        }

        public string ScreenName
        {
            get { return _screenName; }
            set { _screenName = value; OnPropertyChanged("ScreenName"); }
        }

        #endregion

    }
}
