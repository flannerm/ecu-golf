using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using ECU.Golf.Shared.Models;
using ECU.Golf.DataAccess;
using System.Windows;
using System.Diagnostics;

namespace ECU.Golf.ViewModels
{
    public class HoleStatsScreenViewModel : ViewModelBase
    {
        #region Private Members

        private int _selectedRound;

        private HoleStatTypes _holeStatType = HoleStatTypes.Front;

        private ObservableCollection<ViewModelBase> _holeVMs;

        private Visibility _bySideVisibility = Visibility.Visible;
        private Visibility _allVisibility = Visibility.Collapsed;

        public delegate void SelectRoundEventHandler(int round);

        public event SelectRoundEventHandler SelectRoundEvent;

        #endregion

        #region Properties

        public int SelectedRound
        {
            get { return _selectedRound; }
            set
            {
                _selectedRound = value;
                OnPropertyChanged("SelectedRound");

                OnSelectRound();

                loadHoleVMs();
            }
        }

        public HoleStatTypes HoleStatType
        {
            get { return _holeStatType; }
            set 
            { 
                _holeStatType = value; 
                OnPropertyChanged("HoleStatType");

                if (_holeStatType == HoleStatTypes.AllByHole || _holeStatType == HoleStatTypes.AllByRank)
                {
                    BySideVisibility = Visibility.Collapsed;
                    AllVisibility = Visibility.Visible;
                }
                else
                {
                    BySideVisibility = Visibility.Visible;
                    AllVisibility = Visibility.Collapsed;
                }

                loadHoleVMs();
            }
        }

        public ObservableCollection<ViewModelBase> HoleVMs
        {
            get { return _holeVMs; }
            set { _holeVMs = value; OnPropertyChanged("HoleVMs"); }
        }

        public Visibility BySideVisibility
        {
            get { return _bySideVisibility; }
            set { _bySideVisibility = value; OnPropertyChanged("BySideVisibility"); }
        }

        public Visibility AllVisibility
        {
            get { return _allVisibility; }
            set { _allVisibility = value; OnPropertyChanged("AllVisibility"); }
        }

        #endregion

        #region Constructor

        public HoleStatsScreenViewModel()
        {
            _selectedRound = AppData.Tournament.CurrentRound;

            loadHoleVMs();
        }

        #endregion

        #region Public Methods

        public override void Update()
        {
            loadHoleVMs();

            Debug.WriteLine("Updated holes " + DateTime.Now);
        }

        #endregion

        #region Private Methods

        private void loadHoleVMs()
        {
            if (AppData.Tournament.Event.Courses[0].Holes != null)
            {
                ObservableCollection<ViewModelBase> holeVMs = new ObservableCollection<ViewModelBase>();

                List<Hole> holes = null;

                switch (_holeStatType)
                {
                    case HoleStatTypes.Front:
                        holes = new List<Hole>(AppData.Tournament.Event.Courses[0].Holes.Where(h => h.HoleNumber <= 9));
                        break;
                    case HoleStatTypes.Back:
                        holes = new List<Hole>(AppData.Tournament.Event.Courses[0].Holes.Where(h => h.HoleNumber >= 10));
                        break;
                    case HoleStatTypes.AllByHole:
                        holes = new List<Hole>(AppData.Tournament.Event.Courses[0].Holes.OrderBy(h => h.HoleNumber));
                        break;
                    case HoleStatTypes.AllByRank:
                        holes = new List<Hole>(AppData.Tournament.Event.Courses[0].Holes.OrderByDescending(h => h.HoleStats.FirstOrDefault(r => r.Round == _selectedRound).ScoreDiff));
                        break;
                }

                if (holes != null)
                {
                    foreach (Hole hole in holes)
                    {
                        HoleStatsViewModelBase holeVM;

                        if (_holeStatType == HoleStatTypes.AllByHole || _holeStatType == HoleStatTypes.AllByRank)
                        {
                            holeVM = new HoleStatsHorizontalViewModel(hole);
                        }
                        else
                        {
                            holeVM = new HoleStatsVerticalViewModel(hole);
                        }

                        holeVM.HoleStatLine = hole.HoleStats.SingleOrDefault(h => h.Round == _selectedRound);

                        holeVMs.Add(holeVM);
                    }
                }

                HoleVMs = holeVMs;
            }
        }

        private void OnSelectRound()
        {
            SelectRoundEventHandler handler = SelectRoundEvent;

            if (handler != null)
            {
                handler(_selectedRound);
            }
        }

        #endregion
    }
}
