using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ECU.Golf.Shared.Models;
using System.Collections.ObjectModel;

namespace ECU.Golf.ViewModels
{
    public class PairingViewModel : ViewModelBase
    {
        #region Private Members

        private Pairing _pairing;

        private ObservableCollection<PlayerClothingViewModel> _playerVMs;

        private PlayerViewModelBase _selectedPlayer;

        private bool _isVisible = true;

        #endregion

        #region Properties

        public Pairing Pairing
        {
            get { return _pairing; }
            set 
            {
                _pairing = value; 
                OnPropertyChanged("Pairing");

                loadPlayers();
            }
        }
        
        public ObservableCollection<PlayerClothingViewModel> PlayerVMs
        {
            get { return _playerVMs; }
            set { _playerVMs = value; OnPropertyChanged("PlayerVMs"); }
        }

        public PlayerViewModelBase SelectedPlayer
        {
            get { return _selectedPlayer; }
            set 
            { 
                _selectedPlayer = value; 
                OnPropertyChanged("SelectedPlayer");

                if (_selectedPlayer != null)
                {
                    OnSelectPlayer(_selectedPlayer.Player);
                }
                
            }
        }

        public bool IsVisible
        {
            get { return _isVisible; }
            set { _isVisible = value; OnPropertyChanged("IsVisible"); }
        }

        #endregion

        #region Constructor

        public PairingViewModel(Pairing pairing)
        {
            _pairing = pairing;

            _playerVMs = new ObservableCollection<PlayerClothingViewModel>();

            loadPlayers();
        }

        #endregion

        #region Private Methods

        private void loadPlayers()
        {
            ObservableCollection<PlayerClothingViewModel> playerVMs = new ObservableCollection<PlayerClothingViewModel>();

            foreach (Player player in _pairing.Players)
            {
                switch (player.Status)
                {
                    case 2:
                        player.TotalScoreStr = "CUT";
                        break;
                    case 3:
                        player.TotalScoreStr = "WD";
                        break;
                    case 4:
                        player.TotalScoreStr = "DQ";
                        break;
                }

                PlayerClothingViewModel playerVM = new PlayerClothingViewModel(player);
                playerVMs.Add(playerVM);                
            }

            PlayerVMs = playerVMs;
        }

        #endregion
    }
}
