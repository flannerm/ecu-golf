using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Threading;
using ECU.Golf.Shared.Models;
using ECU.Golf.DataAccess;
using System.Configuration;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Linq.Expressions;

namespace ECU.Golf.ViewModels
{
    public class LeaderboardScreenViewModel : ViewModelBase
    {
        #region Private Members

        private ObservableCollection<PlayerViewModel> _leaderboardPlayerVMs;
        private ObservableCollection<PlayerViewModel> _notablePlayerVMs;
        private ObservableCollection<PlayerViewModel> _bestRoundsPlayerVMs;

        private PlayerViewModelBase _selectedPlayer;

        private string _playerSearch;
        private PlayerViewModel _playerSearchVM;

        #endregion

        #region Properties

        public ObservableCollection<PlayerViewModel> LeaderboardPlayerVMs
        {
            get { return _leaderboardPlayerVMs; }
            set { _leaderboardPlayerVMs = value; OnPropertyChanged("LeaderboardPlayerVMs"); }
        }

        public ObservableCollection<PlayerViewModel> NotablePlayerVMs
        {
            get { return _notablePlayerVMs; }
            set { _notablePlayerVMs = value; OnPropertyChanged("NotablePlayerVMs"); }
        }

        public ObservableCollection<PlayerViewModel> BestRoundsPlayerVMs
        {
            get { return _bestRoundsPlayerVMs; }
            set { _bestRoundsPlayerVMs = value; OnPropertyChanged("BestRoundsPlayerVMs"); }
        }

        public PlayerViewModelBase SelectedPlayer
        {
            get { return _selectedPlayer; }
            set 
            { 
                if (value != null && value != _selectedPlayer)
                {
                    _selectedPlayer = value; 
                    OnPropertyChanged("SelectedPlayer");
                    
                    OnSelectPlayer(_selectedPlayer.Player);

                    //_selectedPlayer = null;
                }
            }
        }

        public string PlayerSearch
        {
            get { return _playerSearch; }
            set { _playerSearch = value; OnPropertyChanged("PlayerSearch"); }
        }

        public PlayerViewModel PlayerSearchVM
        {
            get { return _playerSearchVM; }
            set { _playerSearchVM = value; OnPropertyChanged("PlayerSearchVM"); }
        }

        #endregion

        #region Constructor

        public LeaderboardScreenViewModel()
        {
            loadPlayerVMs();
        }

        #endregion

        #region Public Methods

        public override void Update()
        {
            loadPlayerVMs();

            Debug.WriteLine("Updated players " + DateTime.Now);
        }

        public override void Search(string search)
        {
            List<PlayerViewModel> playerVMs = (List<PlayerViewModel>)_leaderboardPlayerVMs.Where(p => p.Player.LastName.ToLower().Contains(search.ToLower())).ToList();

            if (playerVMs.Count > 0)
            {
                PlayerSearchVM = playerVMs[0];
            }
            
            PlayerSearch = search;
        }

        #endregion

        #region Private Methods

        private void loadPlayerVMs()
        {
            ObservableCollection<PlayerViewModel> playerVMs;

            string lastPos = "";

            if (AppData.Players != null && AppData.Players.Count > 0)
            {
                playerVMs = new ObservableCollection<PlayerViewModel>();

                foreach (Player player in AppData.Players)
                {
                    if (player.Status == 1)
                    {
                        if (lastPos == player.DisplayPosition)
                        {
                            player.DisplayPosition = "";
                        }
                        else
                        {
                            lastPos = player.DisplayPosition;
                        }

                        PlayerViewModel playerVM = new PlayerViewModel(player);

                        playerVMs.Add(playerVM);
                    }
                }

                LeaderboardPlayerVMs = playerVMs;

                var results = _leaderboardPlayerVMs.Where(p => p.Player.Notable == true);
                NotablePlayerVMs = new ObservableCollection<PlayerViewModel>(results);

                results = _leaderboardPlayerVMs.OrderBy(p => p.Player.TodayScore).Where(p => p.Player.Thru < 18 && p.Player.Thru > 0);
                BestRoundsPlayerVMs = new ObservableCollection<PlayerViewModel>(results);

            }
        }

        #endregion

    }
}
