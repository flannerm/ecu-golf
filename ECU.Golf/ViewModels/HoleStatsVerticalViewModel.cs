using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ECU.Golf.Shared.Models;

namespace ECU.Golf.ViewModels
{
    public class HoleStatsVerticalViewModel : HoleStatsViewModelBase
    {
        public HoleStatsVerticalViewModel(Hole hole) : base(hole)
        {
            _hole = hole;
        }
    }
}
