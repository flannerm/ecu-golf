using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ECU.Golf.Shared.Models;

namespace ECU.Golf.ViewModels
{
    public class HoleStatsHorizontalViewModel : HoleStatsViewModelBase
    {
        public HoleStatsHorizontalViewModel(Hole hole) : base(hole)
        {
            _hole = hole;
        }
    }
}
