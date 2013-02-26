using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ECU.Golf.Shared.Models;
using System.Windows.Media;

namespace ECU.Golf.ViewModels
{
    public class PlayerShotTrackingViewModel : PlayerViewModelBase
    {
        #region Private Members

        private bool _isChecked = false;

        private SolidColorBrush _indicatorColor;

        public delegate void CheckEventHandler(PlayerShotTrackingViewModel player);

        public event CheckEventHandler CheckEvent;

        #endregion

        #region Properties

        public bool IsChecked 
        { 
            get { return _isChecked; }
            set
            {
                _isChecked = value;
                OnCheck();
            }       
        }

        public SolidColorBrush IndicatorColor
        {
            get { return _indicatorColor; }
            set { _indicatorColor = value; OnPropertyChanged("IndicatorColor"); }
        }

        #endregion

        #region Constructor

        public PlayerShotTrackingViewModel(Player player) : base(player)
        {
            _player = player;
        }

        #endregion

        #region Private Methods

        private void OnCheck()
        {
            CheckEventHandler handler = CheckEvent;

            if (handler != null)
            {
                handler(this);
            }
        }

        #endregion
    }
}
