using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ECU.Golf.Shared.Models
{
    public class SpotterPairing
    {
        public int PairingID { get; set; }
        public string Description { get; set; }
        public List<SpotterPlayer> Players { get; set; }
    }
}
