using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ECU.Golf.Shared.Models;
using System.Windows.Input;
using ECU.Golf.Commands;

namespace ECU.Golf.ViewModels
{
    public class PlayerClothingViewModel : PlayerViewModelBase
    {
        #region Constructor

        public PlayerClothingViewModel(Player player) : base(player)
        {
            _player = player;
        }

        #endregion
    }
}
