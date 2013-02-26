using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ECU.Golf.Shared.Models
{
    public class Event : ModelBase
    {
        public int EventID { get; set; }
        public List<Course> Courses { get; set; }
    }
}
