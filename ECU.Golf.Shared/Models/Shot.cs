using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ECU.Golf.Shared.Models
{
    public class Shot
    {
        public int ShotID { get; set; }
        public int ShotNum { get; set; }
        public int HoleNum { get; set; }
        public string ShotLocation { get; set; }
        public int DistanceTraveled { get; set; }
        public int DistanceToPin { get; set; }
        public Club Club { get; set; }
    }
}
