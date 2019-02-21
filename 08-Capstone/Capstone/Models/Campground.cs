using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Models
{
    class Campground
    {
        public int Campground_id { get; set; }
        public int Park_id { get; set; }
        public string Name { get; set; }
        public DateTime Open_from_mm { get; set; }
        public DateTime Open_to_mm { get; set; }
        public decimal Daily_fee { get; set; }

        public override string ToString() //TODO: Campground ToString Fotmatting
        {
            return "";
        }
    }
}
