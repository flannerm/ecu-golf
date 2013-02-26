using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ECU.Golf.Shared.Models;

namespace ECU.Golf.ViewModels
{
    public class HoleStatsViewModelBase : ViewModelBase
    {
        #region Private Members

        protected Hole _hole;

        private HoleStatLine _holeStatLine;

        #endregion

        #region Properties

        public Hole Hole
        {
            get { return _hole; }
            set { _hole = value; OnPropertyChanged("Hole"); }
        }

        public HoleStatLine HoleStatLine
        {
            get { return _holeStatLine; }
            set { _holeStatLine = value; OnPropertyChanged("HoleStatLine"); }
        }

        #endregion

        #region Constructor

        public HoleStatsViewModelBase()
        { }

        public HoleStatsViewModelBase(Hole hole)
        {
            _hole = hole;
        }

        #endregion
    }
}
