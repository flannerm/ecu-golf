using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ECU.Golf.Shared.Models
{
    public class PlayerHole : Hole
    {
        #region Private Members

        private int? _score;
        private decimal? _driveDist;
        private bool? _fairway;
        private bool? _gir;
        private int? _putts;

        private decimal _toParDiff;

        #endregion

        #region Properties

        public int? Score
        {
            get { return _score; }
            set 
            { 
                _score = value;

                if (_score != null && _score > 0)
                {
                    ToParDiff = (int)_score - base.Par;
                }
            }
        }

        public decimal? DriveDistance
        {
            get { return _driveDist; }
            set
            {
                if (_score > 0)
                {
                    _driveDist = value;
                }
                else
                {
                    _driveDist = null;
                }
            }
        }

        public bool? Fairway
        {
            get { return _fairway; }
            set
            {
                if (_score > 0 && Par > 3)
                {
                    _fairway = value;
                }
                else
                {
                    _fairway = null;
                }
            }
        }

        public bool? GIR
        {
            get { return _gir; }
            set 
            {
                if (_score > 0)
                {
                    _gir = value;
                }
                else
                {
                    _gir = null;
                }
            }
        }

        public int? Putts
        {
            get { return _putts; }
            set 
            {
                if (_score > 0)
                {
                    _putts = value; 
                }
                else
                {
                    _putts = null;
                }    
            }
        }

        public decimal ToParDiff
        {
            get { return _toParDiff; }
            set { _toParDiff = value; }
        }


        #endregion

    }
}
