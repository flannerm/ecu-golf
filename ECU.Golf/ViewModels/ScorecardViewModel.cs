using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ECU.Golf.Shared.Models;

namespace ECU.Golf.ViewModels
{
    public class ScorecardViewModel : ViewModelBase
    {
        #region Private Members

        private Scorecard _card;

        private string _outScore;
        private string _inScore;

        private string _outPar;
        private string _inPar;

        #endregion

        #region Properties

        public Scorecard Scorecard
        {
            get { return _card; }
            set { _card = value; OnPropertyChanged("Scorecard"); }
        }

        public string OutScore
        {
            get { return _outScore; }
            set { _outScore = value; OnPropertyChanged("OutScore"); }
        }

        public string InScore
        {
            get { return _inScore; }
            set { _inScore = value; OnPropertyChanged("InScore"); }
        }

        public string OutPar
        {
            get { return _outPar; }
            set { _outPar = value; OnPropertyChanged("OutPar"); }
        }

        public string InPar
        {
            get { return _inPar; }
            set { _inPar = value; OnPropertyChanged("InPar"); }
        }

        #endregion

        #region Constructor

        //Scorecard card, string outScore, string inScore
        public ScorecardViewModel()
        {
            //_card = card;

            //_outScore = outScore;
            //_inScore = inScore;
        }

        #endregion

    }
}
