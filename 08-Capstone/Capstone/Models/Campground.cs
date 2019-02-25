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

        public override string ToString() //This ToString is returned specifically when checking what campsites exist at a specific campground
        {
            DateTimeFormatInfo dtfi = new DateTimeFormatInfo();

            string campgroundString = 

                "# ".PadLeft(5) + 

                $"{Campground_id}".PadRight(20) + 

                $"{Name}".PadRight(41) + 

                $"{dtfi.GetAbbreviatedMonthName(Open_from_mm)}".PadRight(4) + 

                "-".PadRight(2) + 

                $"{dtfi.GetAbbreviatedMonthName(Open_to_mm)}".PadRight(20).PadLeft(3) + 

                $"{Daily_fee:C}";

            return campgroundString;
        }
    }
}