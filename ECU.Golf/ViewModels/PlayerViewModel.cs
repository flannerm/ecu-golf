using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ECU.Golf.Shared.Models;

namespace ECU.Golf.ViewModels
{
    public class PlayerViewModel : PlayerViewModelBase
    {
        #region Constructor

        public PlayerViewModel(Player player) : base(player)
        {
            _player = player;
        }

        #endregion
    }
}
