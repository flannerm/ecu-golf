using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace ECU.Golf.Shared.Models
{
    public class Player : ModelBase
    {
        #region Private Members

        private Int32 _playerId;

        private string _firstName;
        private string _lastName;
        private string _tvName;
        private Uri _headshot;
        private Country _country;
        private int _age;
        private int _worldRank;
        private int _pgaTourWins;
        private int _euroTourWins;
        private int _majorWins;
        private string _bestTourneyFinish;
        private string _bestMajorFinish;

        private string _totalScoreStr;
        private int _totalScore;
        private int _thru;
        private string _thruStr;
        private string _todayScoreStr;
        private decimal? _todayScore;
        private string _positionStr;
        private int _position;
        private string _displayPosition;

        private string _clothing;
        private string _highlightColor;

        private string _thruColor;

        private bool _notable;

        private ObservableCollection<Scorecard> _scorecards;

        private int _status;

        #endregion

        #region Properties

        public Int32 PlayerID
        {
            get { return _playerId; }
            set { _playerId = value; }
        }

        public string FirstName
        {
            get { return _firstName; }
            set { _firstName = value; }
        }

        public string LastName
        {
            get { return _lastName; }
            set { _lastName = value; }
        }

        public string TvName
        {
            get { return _tvName; }
            set { _tvName = value; }
        }

        public Uri Headshot
        {
            get { return _headshot; }
            set { _headshot = value; }
        }

        public Country Country
        {
            get { return _country; }
            set { _country = value; }
        }

        public int Age
        {
            get { return _age; }
            set { _age = value; }
        }

        public int WorldRank
        {
            get { return _worldRank; }
            set { _worldRank = value; }
        }

        public int PGATourWins
        {
            get { return _pgaTourWins; }
            set { _pgaTourWins = value; }
        }

        public int EuroTourWins
        {
            get { return _euroTourWins; }
            set { _euroTourWins = value; }
        }

        public int MajorWins
        {
            get { return _majorWins; }
            set { _majorWins = value; }
        }

        public string BestTourneyFinish
        {
            get { return _bestTourneyFinish; }
            set { _bestTourneyFinish = value; }
        }

        public string BestMajorFinish
        {
            get { return _bestMajorFinish; }
            set { _bestMajorFinish = value; }
        }

        public string TotalScoreStr
        {
            get { return _totalScoreStr; }
            set { _totalScoreStr = value; }
        }

        public int TotalScore
        {
            get { return _totalScore; }
            set { _totalScore = value; }
        }

        public string TodayScoreStr
        {
            get { return _todayScoreStr; }
            set { _todayScoreStr = value; }
        }

        public decimal? TodayScore
        {
            get { return _todayScore; }
            set { _todayScore = value; }
        }

        public int Thru
        {
            get { return _thru; }
            set { _thru = value; }
        }

        public string ThruStr
        {
            get { return _thruStr; }
            set { _thruStr = value; }
        }

        public string ThruColor
        {
            get { return _thruColor; }
            set { _thruColor = value; }
        }

        public int Position
        {
            get { return _position; }
            set { _position = value; }
        }

        public string PositionStr
        {
            get { return _positionStr; }
            set { _positionStr = value; }
        }

        public string DisplayPosition
        {
            get { return _displayPosition; }
            set { _displayPosition = value; }
        }

        public ObservableCollection<Scorecard> Scorecards
        {
            get { return _scorecards; }
            set { _scorecards = value; }
        }

        public bool Notable
        {
            get { return _notable; }
            set { _notable = value; }
        }

        public string Clothing
        {
            get { return _clothing; }
            set { _clothing = value; }
        }

        public string HighlightColor
        {
            get { return _highlightColor; }
            set { _highlightColor = value; }
        }

        public int Status
        {
            get { return _status; }
            set { _status = value; }
        }

        #endregion

    }
}
