using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Capstone.Models
{
    public class Campground
    {
        public int Campground_id { get; set; }
        public int Park_id { get; set; }
        public string Name { get; set; }
        public int Open_from_mm { get; set; }
        public int Open_to_mm { get; set; }
        public decimal Daily_fee { get; set; }

        public override string ToString() //TODO: Campground ToString Fotmatting
        {
            DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
            string campgroundString = "# ".PadLeft(5) + $"{Campground_id}".PadRight(20) + $"{Name}".PadRight(41) + $"{dtfi.GetAbbreviatedMonthName(Open_from_mm)}".PadRight(4) + "-".PadRight(2) + $"{dtfi.GetAbbreviatedMonthName(Open_to_mm)}".PadRight(20).PadLeft(3) + $"{Daily_fee:C}";
            return campgroundString;
        }
    }
}
//"Campground ID".PadRight(20) + "Campground Name".PadRight(25) + "Start of Season".PadRight(10) + "End of Season".PadRight(10) + "Price per day";