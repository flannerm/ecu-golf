using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ECU.Golf.DataAccess;
using ECU.Golf.Shared.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;
using ECU.Golf.Commands;

namespace ECU.Golf.ViewModels
{
    public class TrackingScreenViewModel : ViewModelBase
    {
        #region Private Members

        private ObservableCollection<TrackingHoleViewModel> _trackingHoleVMs;
        
        private DelegateCommand _loadHomeScreenCommand;

        private MapViewModel _mapViewContent;

        //private bool _mapShowing = false;

        #endregion

        #region Properties

        public ObservableCollection<TrackingHoleViewModel> TrackingHoleVMs
        {
            get { return _trackingHoleVMs; }
            set { _trackingHoleVMs = value; OnPropertyChanged("TrackingHoleVMs"); }
        }

        public MapViewModel MapViewContent
        {
            get { return _mapViewContent; }
            set { _mapViewContent = value; OnPropertyChanged("MapViewContent"); }
        }

        #endregion

        #region Constructor

        public TrackingScreenViewModel()
        {
            loadPairingVMs();

            MapViewContent = new MapViewModel();

        }

        #endregion

        #region Public Methods

        public override void Update()
        {
            if (AppData.Pairings != null && AppData.Pairings.Count > 0)
            {
                loadPairingVMs();
            }

            //if (_mapShowing)
            //{
                if (_mapViewContent != null)
                {
                    _mapViewContent.Update();
                }
            //}
        }

        #endregion

        #region Private Methods

        private void trackingVM_SelectPlayerEvent(Player player)
        {
            OnSelectPlayer(player);
        }

        private void loadPairingVMs()
        {
            if (AppData.Pairings != null && AppData.Pairings.Count > 0)
            {
                ObservableCollection<TrackingHoleViewModel> trackingHoleVMs = new ObservableCollection<TrackingHoleViewModel>();

                ObservableCollection<Pairing> pairings = new ObservableCollection<Pairing>(AppData.Pairings.Where(p => p.Round == AppData.Tournament.CurrentRound));

                for (int i = 1; i <= 18; i++)
                {
                    TrackingHoleViewModel trackingHoleVM = trackingHoleVM = new TrackingHoleViewModel();
                    trackingHoleVM.SelectPlayerEvent += new SelectPlayerEventHandler(trackingVM_SelectPlayerEvent);

                    Pairing pairing;

                    trackingHoleVM.Hole = i.ToString();

                    //TEE
                    pairing = pairings.FirstOrDefault(p => p.OnCourse == i.ToString() + "T");

                    if (pairing != null)
                    {
                        if (pairing.Players != null)
                        {
                            ObservableCollection<PlayerTrackingViewModel> teePlayers = new ObservableCollection<PlayerTrackingViewModel>();

                            foreach (Player player in pairing.Players)
                            {
                                PlayerTrackingViewModel playerTrackingVM = new PlayerTrackingViewModel(player);
                                teePlayers.Add(playerTrackingVM);
                            }

                            if (teePlayers.Count < 3)
                            {
                                for (int p = teePlayers.Count; p <= 3; p++)
                                {
                                    teePlayers.Add(null);
                                }
                            }

                            trackingHoleVM.TeePlayers = teePlayers;
                        }
                    }

                    //FAIRWAY
                    pairing = pairings.FirstOrDefault(p => p.OnCourse == i.ToString() + "F");

                    if (pairing != null)
                    {
                        if (pairing.Players != null)
                        {
                            ObservableCollection<PlayerTrackingViewModel> fairwayPlayers = new ObservableCollection<PlayerTrackingViewModel>();

                            foreach (Player player in pairing.Players)
                            {
                                PlayerTrackingViewModel playerTrackingVM = new PlayerTrackingViewModel(player);
                                fairwayPlayers.Add(playerTrackingVM);
                            }

                            if (fairwayPlayers.Count < 3)
                            {
                                for (int p = fairwayPlayers.Count; p <= 3; p++)
                                {
                                    fairwayPlayers.Add(null);
                                }
                            }

                            trackingHoleVM.FairwayPlayers = fairwayPlayers;
                        }
                    }

                    //GREEN                
                    pairing = pairings.FirstOrDefault(p => p.OnCourse == i.ToString() + "G");

                    if (pairing != null)
                    {
                        if (pairing.Players != null)
                        {
                            ObservableCollection<PlayerTrackingViewModel> greenPlayers = new ObservableCollection<PlayerTrackingViewModel>();

                            foreach (Player player in pairing.Players)
                            {
                                PlayerTrackingViewModel playerTrackingVM = new PlayerTrackingViewModel(player);
                                greenPlayers.Add(playerTrackingVM);
                            }

                            if (greenPlayers.Count < 3)
                            {
                                for (int p = greenPlayers.Count; p <= 3; p++)
                                {
                                    greenPlayers.Add(null);
                                }
                            }

                            trackingHoleVM.GreenPlayers = greenPlayers;
                        }
                    }

                    trackingHoleVMs.Add(trackingHoleVM);

                } //i < 18

                TrackingHoleVMs = trackingHoleVMs;
            }
        }

        //private void loadMap()
        //{
        //    if (_mapShowing)
        //    {
        //        _mapShowing = false;
        //        MapViewContent = null;
        //    }
        //    else
        //    {
        //        _mapShowing = true;
        //        MapViewContent = new MapViewModel();
        //    }
            
        //}

        #endregion

        #region Commands

        //public ICommand ShowMapCommand
        //{
        //    get
        //    {
        //        if (_loadHomeScreenCommand == null)
        //        {
        //            _loadHomeScreenCommand = new DelegateCommand(loadMap);
        //        }
        //        return _loadHomeScreenCommand;
        //    }
        //}

        #endregion
    }
}
