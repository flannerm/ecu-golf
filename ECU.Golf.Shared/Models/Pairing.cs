using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace ECU.Golf.Shared.Models
{
    public class Pairing
    {
        #region Private Members

        private int _pairingId;
        private int _courseId;
        private DateTime _teeTime;
        private string _formattedTeeTime;
        private int _startHole;
        private int _round;
        private string _onCourse;
        private List<Player> _players;

        #endregion

        #region Properties

        public int PairingID
        {
            get { return _pairingId; }
            set { _pairingId = value; }
        }

        public int CourseID
        {
            get { return _courseId; }
            set { _courseId = value; }
        }

        public DateTime TeeTime
        {
            get { return _teeTime; }
            set { _teeTime = value; }
        }

        public string FormattedTeeTime
        {
            get { return _formattedTeeTime; }
            set { _formattedTeeTime = value; }
        }

        public int StartHole
        {
            get { return _startHole; }
            set { _startHole = value; }
        }

        public int Round
        {
            get { return _round; }
            set { _round = value; }
        }

        public string OnCourse
        {
            get { return _onCourse; }
            set { _onCourse = value; }
        }

        public List<Player> Players
        {
            get { return _players; }
            set { _players = value; }
        }

        #endregion
        
    }
}
