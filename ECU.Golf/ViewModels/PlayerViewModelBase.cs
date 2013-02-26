using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ECU.Golf.Shared.Models;

namespace ECU.Golf.ViewModels
{
    public class PlayerViewModelBase : ViewModelBase
    {
        #region Private Members

        protected Player _player;

        #endregion

        #region Properties

        public Player Player
        {
            get { return _player; }
            set { _player = value; OnPropertyChanged("Player"); }
        }

        #endregion

        #region Constructor

        public PlayerViewModelBase(Player player)
        {
            _player = player;
        }

        #endregion
    }
}
