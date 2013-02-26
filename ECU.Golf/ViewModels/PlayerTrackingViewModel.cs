using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ECU.Golf.Shared.Models;

namespace ECU.Golf.ViewModels
{
    public class PlayerTrackingViewModel : PlayerViewModelBase
    {
        #region Constructor

        public PlayerTrackingViewModel(Player player) : base(player)
        {
            _player = player;
        }

        #endregion
    }
}
