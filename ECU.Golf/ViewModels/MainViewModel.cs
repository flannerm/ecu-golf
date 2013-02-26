using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ECU.Golf.DataAccess;
using System.Collections.ObjectModel;
using ECU.Golf.Shared.Models;
using System.Windows;
using System.Windows.Input;
using ECU.Golf.Commands;
using System.Windows.Threading;
using System.Configuration;
using System.ComponentModel;
using System.Diagnostics;

namespace ECU.Golf.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        #region Private Members

        //private int _selectedRound;

        private ViewModelBase _currentScreen;

        private string _playerSearch;

        private PlayerPopupViewModel _playerPopup;
        private Visibility _playerPopupVisibility = Visibility.Hidden;

        private LeaderboardScreenViewModel _leaderboardScreen;
        private PairingsScreenViewModel _pairingsScreen;
        private TrackingScreenViewModel _trackingScreen;
        private HoleStatsScreenViewModel _holeStatsScreen;
        private HistoryScreenViewModel _historyScreen;
        private ResearchScreenViewModel _researchScreen;
        private ShotLogScreenViewModel _shotLogScreen;
        private TwitterScreenViewModel _twitterScreen;

        private ScreenTypes _screenType = ScreenTypes.Home;

        private DelegateCommand _loadHomeScreenCommand;
        private DelegateCommand _loadPairingsScreenCommand;
        private DelegateCommand _loadTrackingScreenCommand;
        private DelegateCommand _loadHoleStatsScreenCommand;
        private DelegateCommand _loadHistoryScreenCommand;
        private DelegateCommand _loadResearchScreenCommand;
        private DelegateCommand _loadShotLogScreenCommand;
        private DelegateCommand _loadTwitterScreenCommand;
        private DelegateCommand _clearSearchTextCommand;

        private DispatcherTimer _refreshTimer;

        #endregion

        #region Properties

        public string TournamentName
        {
            get { return AppData.Tournament.TournamentName; }
            set { AppData.Tournament.TournamentName = value; OnPropertyChanged("TournamentName"); }
        }

        public PlayerPopupViewModel PlayerPopup
        {
            get { return _playerPopup; }
            set { _playerPopup = value; OnPropertyChanged("PlayerPopup"); }
        }

        public Visibility PlayerPopupVisibility
        {
            get { return _playerPopupVisibility; }
            set { _playerPopupVisibility = value; OnPropertyChanged("PlayerPopupVisibility"); }
        }

        public ViewModelBase CurrentScreen
        {
            get { return _currentScreen; }
            set 
            { 
                _currentScreen = value; 
                OnPropertyChanged("CurrentScreen");

                PlayerSearch = "";

                if (_playerPopup != null)
                {
                    hidePlayerPopup();
                }
            }
        }

        public ScreenTypes ScreenType
        {
            get { return _screenType; }
            set { _screenType = value; OnPropertyChanged("ScreenType"); }
        }

        public string PlayerSearch
        {
            get { return _playerSearch; }
            set
            {
                _playerSearch = value;
                OnPropertyChanged("PlayerSearch");

                _currentScreen.Search(_playerSearch);
            }
        }

        #endregion

        #region Constructor

        public MainViewModel()
        {
            _refreshTimer = new DispatcherTimer();
            _refreshTimer.Interval = new TimeSpan(0, 0, Convert.ToInt16(ConfigurationManager.AppSettings["RefreshInterval"]));
            _refreshTimer.Tick += new EventHandler(refreshTimer_Tick);

            AppData.Tournament = DbConnection.GetTournament();

            AppData.Players = DbConnection.GetPlayers();

            AppData.Pairings = DbConnection.GetPairings(AppData.Players);

            //AppData.Holes = DbConnection.GetHoleStats(AppData.Tournament.CurrentRound);

            //load the screen VMs

            _leaderboardScreen = new LeaderboardScreenViewModel();
            _leaderboardScreen.SelectPlayerEvent += new SelectPlayerEventHandler(selectPlayer);

            _pairingsScreen = new PairingsScreenViewModel();
            _pairingsScreen.SelectPlayerEvent += new SelectPlayerEventHandler(selectPlayer);

            _trackingScreen = new TrackingScreenViewModel();
            _trackingScreen.SelectPlayerEvent += new SelectPlayerEventHandler(selectPlayer);

            _holeStatsScreen = new HoleStatsScreenViewModel();
            //_holeStatsScreen.SelectRoundEvent += new HoleStatsScreenViewModel.SelectRoundEventHandler(selectRound);
            
            _historyScreen = new HistoryScreenViewModel();

            _researchScreen = new ResearchScreenViewModel();

            _twitterScreen = new TwitterScreenViewModel();

            _shotLogScreen = new ShotLogScreenViewModel();
            
            _currentScreen = _leaderboardScreen;

            _refreshTimer.Start();
        }

        #endregion

        #region Private Methods

        private void refreshTimer_Tick(object sender, EventArgs e)
        {
            _refreshTimer.Stop();

            BackgroundWorker worker = new BackgroundWorker();

            worker.DoWork += delegate(object s, DoWorkEventArgs args)
            {
                Debug.WriteLine("Starting update " + DateTime.Now);

                AppData.Tournament = DbConnection.GetTournament();

                AppData.Players = DbConnection.GetPlayers();

                AppData.Pairings = DbConnection.GetPairings(AppData.Players);

                //AppData.Holes = DbConnection.GetHoles();

                //if (_currentScreen.GetType() == typeof(PairingsScreenViewModel) || _currentScreen.GetType() == typeof(TrackingScreenViewModel))
                //{
                    //loadPairings();
                    
                //}

                //if (_currentScreen.GetType() == typeof(HoleStatsScreenViewModel))
                //{
                
                //}

                //always update the leaderboard and tracking screen
                _trackingScreen.Update();

                _leaderboardScreen.Update();

                _pairingsScreen.Update();

                //if (_currentScreen.GetType() != typeof(LeaderboardScreenViewModel))
                //{
                //    _currentScreen.Update(_holes);
                //}                

                if (_playerPopup != null)
                {
                    PlayerPopup.Update(AppData.Players.SingleOrDefault(p => p.PlayerID == _playerPopup.Player.PlayerID));
                }
            };

            worker.RunWorkerCompleted += delegate(object s, RunWorkerCompletedEventArgs args)
            {
                _refreshTimer.Start();
            };

            worker.RunWorkerAsync();
        }               

        //private void loadPlayers()
        //{
            

        //    _leaderboardScreen.Update();
        //}

        //private void loadPairings()
        //{
            

        //    _pairingsScreen.Update();
                      
        //} 

        //private void loadHoles()
        //{
           
        //}

        //private void getTourneyInfo()
        //{
        //    DbConnection.GetTourneyInfo(ref _currentRound, ref _tournamentName);

        //    OnPropertyChanged("TournamentName");
        //}

        private void selectPlayer(Player player)
        {
            PlayerPopup = new PlayerPopupViewModel(player, AppData.Tournament.CurrentRound);
            PlayerPopup.HidePopupEvent += new PlayerPopupViewModel.HidePopupEventHandler(hidePlayerPopup);

            PlayerPopupVisibility = Visibility.Visible;
        }

        private void hidePlayerPopup()
        {
            string clothing = PlayerPopup.PlayerClothing;
            Int32 playerID = PlayerPopup.Player.PlayerID;

            PlayerPopup.HidePopupEvent -= hidePlayerPopup;
            PlayerPopup = null;

            PlayerPopupVisibility = Visibility.Hidden;  

            //update the player in the _players collection with his new clothes
            Player player = AppData.Players.SingleOrDefault(p => p.PlayerID == playerID);

            if (player != null)
            {
                if (player.Clothing != clothing)
                {
                    player.Clothing = clothing;
                                        
                    //loadPairings();

                    _currentScreen.Update();
                }          
            }      
        }

        //private void selectRound(int round)
        //{
        //    //_selectedRound = round;

        //    loadHoles();

        //    _currentScreen.Update();
        //}

        private void loadHomeScreen()
        {
            try
            {          
                CurrentScreen = _leaderboardScreen;
            }
            finally
            {
                
            }
        }

        private void loadPairingsScreen()
        { 
            try
            {
                _refreshTimer.Stop();
                //loadPairings();
                CurrentScreen = _pairingsScreen;
            }
            finally
            {
                _refreshTimer.Start();
            }
        }

        private void loadTrackingScreen()
        {            
            try
            {
                //_refreshTimer.Stop();
                //loadPairings();
                CurrentScreen = _trackingScreen;
            }
            finally
            {
                //_refreshTimer.Start();
            }
        }

        private void loadHoleStatsScreen()
        {
            try
            {
                _refreshTimer.Stop();
                CurrentScreen = _holeStatsScreen;
                //loadHoles();
            }
            finally
            {
                _refreshTimer.Start();
            }
        }

        private void loadHistoryScreen()
        {
            CurrentScreen = _historyScreen;
        }

        private void loadResearchScreen()
        {
            CurrentScreen = _researchScreen;
        }

        private void loadShotLogScreen()
        {
            CurrentScreen = _shotLogScreen;
        }

        private void loadTwitterScreen()
        {
            CurrentScreen = _twitterScreen;
        }

        private void clearSearchText()
        {
            PlayerSearch = "";
        }

        #endregion

        #region Commands

        public ICommand LoadHomeScreenCommand
        {
            get
            {
                if (_loadHomeScreenCommand == null)
                {
                    _loadHomeScreenCommand = new DelegateCommand(loadHomeScreen);
                }
                return _loadHomeScreenCommand;
            }
        }

        public ICommand LoadPairingsScreenCommand
        {
            get
            {
                if (_loadPairingsScreenCommand == null)
                {
                    _loadPairingsScreenCommand = new DelegateCommand(loadPairingsScreen);
                }
                return _loadPairingsScreenCommand;
            }
        }

        public ICommand LoadTrackingScreenCommand
        {
            get
            {
                if (_loadTrackingScreenCommand == null)
                {
                    _loadTrackingScreenCommand = new DelegateCommand(loadTrackingScreen);
                }
                return _loadTrackingScreenCommand;
            }
        }

        public ICommand LoadHoleStatsScreenCommand
        {
            get
            {
                if (_loadHoleStatsScreenCommand == null)
                {
                    _loadHoleStatsScreenCommand = new DelegateCommand(loadHoleStatsScreen);
                }
                return _loadHoleStatsScreenCommand;
            }
        }

        public ICommand LoadHistoryScreenCommand
        {
            get
            {
                if (_loadHistoryScreenCommand == null)
                {
                    _loadHistoryScreenCommand = new DelegateCommand(loadHistoryScreen);
                }
                return _loadHistoryScreenCommand;
            }
        }

        public ICommand LoadResearchScreenCommand
        {
            get
            {
                if (_loadResearchScreenCommand == null)
                {
                    _loadResearchScreenCommand = new DelegateCommand(loadResearchScreen);
                }
                return _loadResearchScreenCommand;
            }
        }

        public ICommand LoadShotLogScreenCommand
        {
            get
            {
                if (_loadShotLogScreenCommand == null)
                {
                    _loadShotLogScreenCommand = new DelegateCommand(loadShotLogScreen);
                }
                return _loadShotLogScreenCommand;
            }
        }

        public ICommand LoadTwitterScreenCommand
        {
            get
            {
                if (_loadTwitterScreenCommand == null)
                {
                    _loadTwitterScreenCommand = new DelegateCommand(loadTwitterScreen);
                }
                return _loadTwitterScreenCommand;
            }
        }

        public ICommand ClearSearchTextCommand
        {
            get
            {
                if (_clearSearchTextCommand == null)
                {
                    _clearSearchTextCommand = new DelegateCommand(clearSearchText);
                }
                return _clearSearchTextCommand;
            }
        }

        #endregion
    }
}
