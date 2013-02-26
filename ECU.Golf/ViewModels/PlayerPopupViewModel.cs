using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ECU.Golf.Shared.Models;
using System.Windows.Input;
using ECU.Golf.Commands;
using System.Collections.ObjectModel;
using ECU.Golf.DataAccess;
using System.ComponentModel;
using System.Diagnostics;

namespace ECU.Golf.ViewModels
{
    public class PlayerPopupViewModel : ViewModelBase
    {
        #region Private Members

        private Player _player;

        private string _totalScoreStr;
        private string _todayScoreStr;

        private ScorecardViewModel _scorecardVM;

        private int _selectedRound;

        private string _selectedRoundDrivingDist;
        private string _selectedRoundFairways;
        private string _selectedRoundFairwaysPct;
        private string _selectedRoundGIR;
        private string _selectedRoundGIRPct;
        private string _selectedRoundPutts;
        private string _selectedRoundPuttsGIR;

        private string _tourneyDrivingDist;
        private string _tourneyFairways;
        private string _tourneyFairwaysPct;
        private string _tourneyGIR;
        private string _tourneyGIRPct;
        private string _tourneyPutts;
        private string _tourneyPuttsGIR;

        private string _outScore;
        private string _inScore;

        private string _outPar;
        private string _inPar;

        private string _oldClothing;
        private string _playerClothing;

        public delegate void HidePopupEventHandler();
        public event HidePopupEventHandler HidePopupEvent;

        private DelegateCommand _hidePopupCommand;
        private DelegateCommand _savePlayerClothingCommand;

        private bool _saved = false;

        #endregion

        #region Properties

        public Player Player
        {
            get { return _player; }
            set { _player = value; OnPropertyChanged("Player"); }
        }

        public string TotalScoreStr
        {
            get { return _totalScoreStr; }
            set { _totalScoreStr = value; OnPropertyChanged("TotalScoreStr"); }
        }

        public string TodayScoreStr
        {
            get { return _todayScoreStr; }
            set { _todayScoreStr = value; OnPropertyChanged("TodayScoreStr"); }
        }

        public ScorecardViewModel ScorecardVM
        {
            get { return _scorecardVM; }
            set { _scorecardVM = value; OnPropertyChanged("ScorecardVM"); }
        }

        public string SelectedRoundDrivingDistance
        {
            get { return _selectedRoundDrivingDist; }
            set { _selectedRoundDrivingDist = value; OnPropertyChanged("SelectedRoundDrivingDistance"); }
        }

        public string SelectedRoundFairways
        {
            get { return _selectedRoundFairways; }
            set { _selectedRoundFairways = value; OnPropertyChanged("SelectedRoundFairways"); }
        }

        public string SelectedRoundFairwaysPct
        {
            get { return _selectedRoundFairwaysPct; }
            set { _selectedRoundFairwaysPct = value; OnPropertyChanged("SelectedRoundFairwaysPct"); }
        }

        public string SelectedRoundGIR
        {
            get { return _selectedRoundGIR; }
            set { _selectedRoundGIR = value; OnPropertyChanged("SelectedRoundGIR"); }
        }

        public string SelectedRoundGIRPct
        {
            get { return _selectedRoundGIRPct; }
            set { _selectedRoundGIRPct = value; OnPropertyChanged("SelectedRoundGIRPct"); }
        }

        public string SelectedRoundPutts
        {
            get { return _selectedRoundPutts; }
            set { _selectedRoundPutts = value; OnPropertyChanged("SelectedRoundPutts"); }
        }

        public string SelectedRoundPuttsGIR
        {
            get { return _selectedRoundPuttsGIR; }
            set { _selectedRoundPuttsGIR = value; OnPropertyChanged("SelectedRoundPuttsGIR"); }
        }

        public string TourneyDrivingDistance
        {
            get { return _tourneyDrivingDist; }
            set { _tourneyDrivingDist = value; OnPropertyChanged("TourneyDrivingDistance"); }
        }

        public string TourneyFairways
        {
            get { return _tourneyFairways; }
            set { _tourneyFairways = value; OnPropertyChanged("TourneyFairways"); }
        }

        public string TourneyFairwaysPct
        {
            get { return _tourneyFairwaysPct; }
            set { _tourneyFairwaysPct = value; OnPropertyChanged("TourneyFairwaysPct"); }
        }

        public string TourneyGIR
        {
            get { return _tourneyGIR; }
            set { _tourneyGIR = value; OnPropertyChanged("TourneyGIR"); }
        }

        public string TourneyGIRPct
        {
            get { return _tourneyGIRPct; }
            set { _tourneyGIRPct = value; OnPropertyChanged("TourneyGIRPct"); }
        }

        public string TourneyPutts
        {
            get { return _tourneyPutts; }
            set { _tourneyPutts = value; OnPropertyChanged("TourneyPutts"); }
        }

        public string TourneyPuttsGIR
        {
            get { return _tourneyPuttsGIR; }
            set { _tourneyPuttsGIR = value; OnPropertyChanged("TourneyPuttsGIR"); }
        }

        public string PlayerClothing
        {
            get { return _playerClothing; }
            set { _playerClothing = value; OnPropertyChanged("PlayerClothing"); }
        }

        public int SelectedRound
        {
            get { return _selectedRound; }
            set 
            { 
                _selectedRound = value; 
                OnPropertyChanged("SelectedRound");
                
                updateStats();
            }
        }

        #endregion

        #region Constructor

        public PlayerPopupViewModel(Player player, int currentRound)
        {
            _player = player;

            _playerClothing = player.Clothing;
            _oldClothing = _playerClothing;

            _selectedRound = currentRound;

            ScorecardVM = new ScorecardViewModel();

            updateStats();

            //ScorecardVM = new ScorecardViewModel(_player.Scorecards.SingleOrDefault(s => s.Round == _selectedRound), _outScore, _inScore);
        }

        #endregion

        #region Public Methods

        public void Update(Player player)
        {
            Player = player;            
            updateStats();
        }

        #endregion

        #region Private Methods

        private void updateStats()
        {
            TotalScoreStr = _player.TotalScoreStr;
            TodayScoreStr = _player.TodayScoreStr;

            Scorecard card = DbConnection.GetScorecard(_player.PlayerID, _selectedRound);

            //Scorecard card = (Scorecard)_player.Scorecards.SingleOrDefault(s => s.Round == _selectedRound);
            
            ScorecardVM.Scorecard = card;            

            int holesPlayed = card.Holes.Count(h => h.Score != null && h.Score > 0);

            _outScore = "";
            _inScore = "";

            _outPar = card.Holes.Where(h => h.HoleNumber <= 9).Sum(h => h.Par).ToString();
            _inPar = card.Holes.Where(h => h.HoleNumber > 9).Sum(h => h.Par).ToString();

            ScorecardVM.OutPar = _outPar;
            ScorecardVM.InPar = _inPar;

            if (holesPlayed == 18)
            {
                _outScore = card.Holes.Where(h => h.HoleNumber <= 9).Sum(h => h.Score).ToString();
                _inScore = card.Holes.Where(h => h.HoleNumber > 9).Sum(h => h.Score).ToString();
            }
            else if (holesPlayed > 9)
            {
                int? holeScore = card.Holes.SingleOrDefault(h => h.HoleNumber == 9).Score;

                if (holeScore != null && holeScore > 0)
                {
                    _outScore = card.Holes.Where(h => h.HoleNumber <= 9).Sum(h => h.Score).ToString();
                    _inScore = "";
                }
                else
                {
                    _inScore = card.Holes.Where(h => h.HoleNumber > 9).Sum(h => h.Score).ToString();
                    _outScore = "";
                }

            }  

            ScorecardVM.OutScore = _outScore;
            ScorecardVM.InScore = _inScore;

            SelectedRoundDrivingDistance = getDrivingDistance(card.Holes).ToString();
            SelectedRoundFairways = getFairways(card.Holes, ref _selectedRoundFairwaysPct);
            SelectedRoundGIR = getGIR(card.Holes, ref _selectedRoundGIRPct);
            SelectedRoundPutts = getPutts(card.Holes).ToString();

            decimal? puttsGIR = getPuttsGIR(card.Holes);

            if (puttsGIR != null)
            {
                SelectedRoundPuttsGIR = Math.Round(Convert.ToDouble(puttsGIR), 2).ToString();
            }

            var query = _player.Scorecards.SelectMany(s => s.Holes);
            ObservableCollection<PlayerHole> tourneyHoles = new ObservableCollection<PlayerHole>(query);

            TourneyDrivingDistance = getDrivingDistance(tourneyHoles).ToString();
            TourneyFairways = getFairways(tourneyHoles, ref _tourneyFairwaysPct);
            TourneyGIR = getGIR(tourneyHoles, ref _tourneyGIRPct);
            TourneyPutts = getPutts(tourneyHoles).ToString();

            puttsGIR = getPuttsGIR(tourneyHoles);

            if (puttsGIR != null)
            {
                TourneyPuttsGIR = Math.Round(Convert.ToDouble(puttsGIR), 2).ToString();
            } 
        }

        private decimal? getDrivingDistance(ObservableCollection<PlayerHole> holes)
        {
            decimal? avg = null;

            int numOfHoles = holes.Count(h => h.DriveDistance != null);
            decimal? totalDist = holes.Sum(h => h.DriveDistance);

            if (numOfHoles > 0 & totalDist != null)
            {
                avg = Math.Round(Convert.ToDecimal(totalDist / numOfHoles), 2);
            }
            
            return avg;
        }

        private string getFairways(ObservableCollection<PlayerHole> holes, ref string pct)
        {
            string fairways = "";

            double numOfHoles = holes.Count(h => h.Score > 0 && h.Score != null && h.Par > 3);
            double fairwaysHit = holes.Count(h => h.Score > 0 && h.Score != null & h.Fairway == true);

            if (numOfHoles > 0)
            {
                fairways = fairwaysHit.ToString() + " of " + numOfHoles.ToString();
                pct = "(" + (Math.Round(fairwaysHit / numOfHoles, 2) * 100).ToString() + "%)";
            }
            else
            {
                pct = "";
            }

            return fairways;
        }

        private string getGIR(ObservableCollection<PlayerHole> holes, ref string pct)
        {
            string gir = "";

            double numOfHoles = holes.Count(h => h.Score > 0 && h.Score != null);
            double greensHit = holes.Count(h => h.Score > 0 && h.Score != null & h.GIR == true);

            if (numOfHoles > 0)
            {
                gir = greensHit.ToString() + " of " + numOfHoles.ToString();
                pct = "(" + (Math.Round(greensHit / numOfHoles, 2) * 100).ToString() + "%)";

            }
            else
            {
                pct = "";
            }            

            return gir;
        }

        private decimal? getPutts(ObservableCollection<PlayerHole> holes)
        {
            decimal? putts = holes.Sum(h => h.Putts);

            return putts;
        }

        private decimal? getPuttsGIR(ObservableCollection<PlayerHole> holes)
        {
            decimal? puttsGIR = null;

            decimal? totalPutts = holes.Where(h => h.GIR == true).Sum(h => h.Putts);
            int greensHit = holes.Count(h => h.Score > 0 && h.Score != null & h.GIR == true);

            if (greensHit > 0)
            {
                puttsGIR = totalPutts / greensHit; 
            }

            return puttsGIR;
        }

        private void hidePopup()
        {
            if (_saved == false)
            {
                PlayerClothing = _oldClothing;
            }

            OnHidePopup();
        }

        private void OnHidePopup()
        {
            HidePopupEventHandler handler = HidePopupEvent;

            if (handler != null)
            {
                handler();
            }
        }

        private void savePlayerClothing()
        {
            _saved = DbConnection.SavePlayerClothing(Player, _playerClothing);
        }

        #endregion

        #region Commands

        public ICommand HidePopupCommand
        {
            get
            {
                if (_hidePopupCommand == null)
                {
                    _hidePopupCommand = new DelegateCommand(hidePopup);
                }
                return _hidePopupCommand;
            }
        }

        public ICommand SavePlayerClothingCommand
        {
            get
            {
                if (_savePlayerClothingCommand == null)
                {
                    _savePlayerClothingCommand = new DelegateCommand(savePlayerClothing);
                }
                return _savePlayerClothingCommand;
            }
        }
        
        #endregion
    }
}
