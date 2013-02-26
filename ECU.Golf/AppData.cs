using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.ComponentModel;

using ECU.Golf.Shared.Models;
using ECU.Golf.ViewModels;

namespace ECU.Golf
{
    public static class AppData
    {
        #region Private Members
            
        #endregion

        #region Properties

        public static Tournament Tournament { get; set; }

        public static List<Player> Players { get; set; }

        public static List<Pairing> Pairings { get; set; }

        public static List<Hole> Holes { get; set; }

        #endregion

    }
}
