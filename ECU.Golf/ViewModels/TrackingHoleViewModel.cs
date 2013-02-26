using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace ECU.Golf.ViewModels
{
    public class TrackingHoleViewModel : ViewModelBase
    {
        #region Private Members

        private string _hole;

        private ObservableCollection<PlayerTrackingViewModel> _teePlayers;
        private ObservableCollection<PlayerTrackingViewModel> _fairwayPlayers;
        private ObservableCollection<PlayerTrackingViewModel> _greenPlayers;

        private PlayerTrackingViewModel _selectedPlayer;

        #endregion

        #region Properties

        public PlayerTrackingViewModel SelectedPlayer
        {
            get { return _selectedPlayer; }
            set
            {
                _selectedPlayer = value;
                OnPropertyChanged("SelectedPlayer");

                OnSelectPlayer(_selectedPlayer.Player);
            }
        }

        public string Hole
        {
            get { return _hole; }
            set { _hole = value; OnPropertyChanged("Hole"); }
        }

        public ObservableCollection<PlayerTrackingViewModel> TeePlayers
        {
            get { return _teePlayers; }
            set { _teePlayers = value; OnPropertyChanged("TeePlayers"); }
        }

        public ObservableCollection<PlayerTrackingViewModel> FairwayPlayers
        {
            get { return _fairwayPlayers; }
            set { _fairwayPlayers = value; OnPropertyChanged("FairwayPlayers"); }
        }

        public ObservableCollection<PlayerTrackingViewModel> GreenPlayers
        {
            get { return _greenPlayers; }
            set { _greenPlayers = value; OnPropertyChanged("GreenPlayers"); }
        }

        #endregion



    }
}
