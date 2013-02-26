using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ECU.Golf.Shared.Models
{
    public class ChatUser
    {
        #region Private Members

        private int _userId;
        private string _userName;
        private int _userType;

        #endregion

        #region Properties

        public int UserId
        {
            get { return _userId; }
            set { _userId = value; }
        }

        public string UserName
        {
            get { return _userName; }
            set { _userName = value; }
        }

        public int UserType
        {
            get { return _userType; }
            set { _userType = value; }
        }

        #endregion

    }
}
