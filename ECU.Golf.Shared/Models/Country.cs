using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ECU.Golf.Shared.Models
{
    public class Country : ModelBase
    {
        #region Private Members

        private int _countryId;
        private string _countryName;
        private Uri _flag;
        private string _abbrev;

        #endregion

        #region Properties

        public int CountryID
        {
            get { return _countryId; }
            set { _countryId = value; }
        }

        public string CountryName
        {
            get { return _countryName; }
            set { _countryName = value; }
        }

        public Uri Flag
        {
            get { return _flag; }
            set { _flag = value; }
        }

        public string Abbrev
        {
            get { return _abbrev; }
            set { _abbrev = value; }
        }

        #endregion
    }
}
