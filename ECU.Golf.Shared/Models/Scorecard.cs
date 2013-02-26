using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace ECU.Golf.Shared.Models
{
    public class Scorecard : ModelBase
    {
        #region Private Members

        private int _round;
        private ObservableCollection<PlayerHole> _holes;
        private int _outScore;
        private int _inScore;

        #endregion

        #region Properties

        public int Round
        {
            get { return _round; }
            set { _round = value; }
        }

        public ObservableCollection<PlayerHole> Holes
        {
            get { return _holes; }
            set { _holes = value; }
        }

        public int OutScore
        {
            get { return _outScore; }
            set { _outScore = value; }
        }

        public int InScore
        {
            get { return _inScore; }
            set { _inScore = value; }
        }

        #endregion

    }
}
