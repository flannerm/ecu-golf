using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using ECU.Golf.Shared.Models;
using System.Windows.Threading;
using System.Configuration;
using System.Diagnostics;
using System.ComponentModel;
using ECU.Golf.DataAccess;

namespace ECU.Golf.ViewModels
{
    public class TwitterScreenViewModel : ViewModelBase
    {
        #region Private Members

        private ObservableCollection<TweetViewModel> _tweetVMs;

        private DispatcherTimer _refreshTimer;

        #endregion

        #region Properties

        public ObservableCollection<TweetViewModel> TweetVMs
        {
            get { return _tweetVMs; }
            set { _tweetVMs = value; OnPropertyChanged("TweetVMs"); }
        }
        
        #endregion

        #region Constructor

        public TwitterScreenViewModel()
        {
            _refreshTimer = new DispatcherTimer();
            _refreshTimer.Interval = new TimeSpan(0, 0, Convert.ToInt16(ConfigurationManager.AppSettings["RefreshInterval"]));
            _refreshTimer.Tick += new EventHandler(refreshTimer_Tick);

            loadTweets();

            _refreshTimer.Start();
        }

        #endregion

        #region Private Methods

        private void loadTweets()
        {
            List<Tweet> tweets = DbConnection.GetTweets();

            ObservableCollection<TweetViewModel> tweetVMs = new ObservableCollection<TweetViewModel>();

            foreach (Tweet tweet in tweets)
            {
                TweetViewModel tweetVM = new TweetViewModel(tweet);

                tweetVMs.Add(tweetVM);
            }

            TweetVMs = tweetVMs;
        }

        private void refreshTimer_Tick(object sender, EventArgs e)
        {
            _refreshTimer.Stop();

            BackgroundWorker worker = new BackgroundWorker();

            worker.DoWork += delegate(object s, DoWorkEventArgs args)
            {
                //Debug.WriteLine("Starting tweets update " + DateTime.Now);

                loadTweets();
            };

            worker.RunWorkerCompleted += delegate(object s, RunWorkerCompletedEventArgs args)
            {
                _refreshTimer.Start();
            };

            worker.RunWorkerAsync();
        }            

        #endregion
    }
}
