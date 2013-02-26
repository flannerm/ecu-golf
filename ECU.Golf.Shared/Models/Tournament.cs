using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ECU.Golf.Shared.Models
{
    public class Tournament : ModelBase
    {
        public string TournamentName { get; set; }
        public int CurrentRound { get; set; }
        public string ProjectedCut { get; set; }
        public string TournamentLogo { get; set; }

        public Event Event { get; set; }


    }
}
