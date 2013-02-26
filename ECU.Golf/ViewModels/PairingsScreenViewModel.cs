using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ECU.Golf.DataAccess;
using ECU.Golf.Shared.Models;
using System.Collections.ObjectModel;
using System.Windows.Threading;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;

namespace ECU.Golf.ViewModels
{
    public class PairingsScreenViewModel : ViewModelBase
    {
        #region Private Members
        
        private ObservableCollection<PairingViewModel> _pairingVMs;

        private int _selectedRound;

        private PlayerViewModelBase _selectedPlayer;

        private string _playerSearch;
        private PairingViewModel _pairingSearchVM;

        #endregion

        #region Properties

        public ObservableCollection<PairingViewModel> PairingVMs
        {
            get { return _pairingVMs; }
            set { _pairingVMs = value; OnPropertyChanged("PairingVMs"); }
        }

        public int SelectedRound
        {
            get { return _selectedRound; }
            set
            {
                _selectedRound = value;
                OnPropertyChanged("SelectedRound");

                loadPairingVMs(true);
            }
        }

        public PairingViewModel PairingSearchVM
        {
            get { return _pairingSearchVM; }
            set { _pairingSearchVM = value; OnPropertyChanged("PairingSearchVM"); }
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

        #endregion

        #region Constructor

        public PairingsScreenViewModel()
        {
            _selectedRound = AppData.Tournament.CurrentRound;

            loadPairingVMs(true);
        }

        #endregion

        #region Public Methods

        public override void Update()
        {
            loadPairingVMs(false);
        }

        public string PlayerSearch
        {
            get { return _playerSearch; }
            set { _playerSearch = value; OnPropertyChanged("PlayerSearch"); }
        }

        public override void Search(string search)
        {
            try
            {                
                List<PlayerClothingViewModel> playerVMs = (List<PlayerClothingViewModel>)PairingVMs.SelectMany(p => p.PlayerVMs.Where(pl => pl.Player != null && pl.Player.LastName.ToLower().Contains(search.ToLower()))).ToList();

                List<PairingViewModel> pairingVMs = (List<PairingViewModel>)PairingVMs.Where(p => p.PlayerVMs.Contains(playerVMs[0])).ToList();

                if (playerVMs.Count > 0)
                {
                    PairingSearchVM = pairingVMs[0];
                }

                PlayerSearch = search;
            }
            catch (Exception ex)
            {

            }
        }

        #endregion

        #region Private Methods

        public void loadPairingVMs(bool clearList)
        {
            if (AppData.Pairings != null && AppData.Pairings.Count > 0)
            {
                List<Pairing> pairings = AppData.Pairings.Where(p => p.Round == _selectedRound).ToList();

                if (pairings != null)
                {
                    if (PairingVMs == null)
                    {
                        PairingVMs = new ObservableCollection<PairingViewModel>();
                    }

                    ObservableCollection<PairingViewModel> tempPairingVMs;

                    if (clearList)
                    {
                        tempPairingVMs = new ObservableCollection<PairingViewModel>();

                        foreach (Pairing pairing in pairings)
                        {
                            PairingViewModel pairingVM = new PairingViewModel(pairing);
                            pairingVM.SelectPlayerEvent += new SelectPlayerEventHandler(pairingVM_SelectPlayerEvent);
                            PairingVMs.Add(pairingVM);
                            tempPairingVMs.Add(pairingVM);
                        }

                        PairingVMs = tempPairingVMs;
                    }
                    else
                    {
                        tempPairingVMs = PairingVMs;

                        foreach (Pairing pairing in pairings)
                        {
                            PairingViewModel pairingVM = null;

                            pairingVM = PairingVMs.SingleOrDefault(pvm => pvm.Pairing.PairingID == pairing.PairingID);

                            if (pairingVM != null)
                            {
                                pairingVM.Pairing = pairing;
                            }
                            else
                            {
                                pairingVM = new PairingViewModel(pairing);
                                pairingVM.SelectPlayerEvent += new SelectPlayerEventHandler(pairingVM_SelectPlayerEvent);
                                PairingVMs.Add(pairingVM);
                            }
                        }
                    } //clear list 

                } //pairings != null

            } //Pairings != null

        }

        private void pairingVM_SelectPlayerEvent(Player player)
        {
            OnSelectPlayer(player);
        }

        //private void loadPairings(bool clear)
        //{
        //    if (clear)
        //    {
        //        ObservableCollection<PairingViewModel> pairingVMs = new ObservableCollection<PairingViewModel>();

        //        //THIS RELOADS THE ENTIRE COLLECTION
        //        //ObservableCollection<PairingViewModel> pairingVMs = new ObservableCollection<PairingViewModel>();

        //        foreach (Pairing pairing in _pairings.Where(p => p.Round == _selectedRound))
        //        {
        //            PairingViewModel pairingVM = new PairingViewModel(pairing);
        //            pairingVM.SelectPlayerEvent += new SelectPlayerEventHandler(pairingVM_SelectPlayerEvent);
                    
        //            pairingVMs.Add(pairingVM);
        //           // 
        //        }



        //        PairingVMs = pairingVMs;

               
                
        //    }
        //    else
        //    {
        //        //THIS RIPS THRU THE COLLECTION AND UPDATES THE INDIVIDUAL ITEMS
        //        foreach (Pairing pairing in _pairings.Where(p => p.Round == _selectedRound))
        //        {
        //            PairingViewModel pairingVM = null;

        //            if (_pairingVMs == null)
        //            {
        //                _pairingVMs = new ObservableCollection<PairingViewModel>();
        //            }

        //            pairingVM = _pairingVMs.SingleOrDefault(pvm => pvm.Pairing.PairingID == pairing.PairingID);

        //            if (pairingVM != null)
        //            {
        //                pairingVM.Pairing = pairing;
        //            }
        //            else
        //            {
        //                pairingVM = new PairingViewModel(pairing);
        //                pairingVM.SelectPlayerEvent += new SelectPlayerEventHandler(pairingVM_SelectPlayerEvent);
        //                PairingVMs.Add(pairingVM);
        //            }
                   
        //        }
        //    }

        //}

        #endregion

    }
}
