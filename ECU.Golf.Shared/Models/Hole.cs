using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ECU.Golf.Shared.Models
{
    public class Hole : ModelBase
    {
        public int HoleID { get; set; }
        public int HoleNumber { get; set; }
        public int Par { get; set; }
        public int Yardage { get; set; }
        public string TeeLocation { get; set; }
        public string PinLocation { get; set; }
        public string ApproachLocation { get; set; }

        public List<HoleStatLine> HoleStats { get; set; }
    }
}
