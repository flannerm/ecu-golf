using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ECU.Golf.Shared.Models
{
    public class HoleStatLine : ModelBase
    {
        public int Round { get; set; }
        public int Aces { get; set; }
        public int Eagles { get; set; }
        public int Birdies { get; set; }
        public int Pars { get; set; }
        public int Bogeys { get; set; }
        public int Others { get; set; }
        public decimal ScoreAvg { get; set; }
        public decimal ScoreDiff { get; set; }
        public string ScoreRank { get; set; }
        public string FairwaysAvg { get; set; }
        public string GIR { get; set; }
        public decimal PuttsAvg { get; set; } 
    }
}
