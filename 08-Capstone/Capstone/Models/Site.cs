using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Models
{
    public class Site
    {
        public int SiteID { get; set; }
        public int CampgroundID { get; set; }
        public int SiteNumber { get; set; }
        public int MaxOccupancy { get; set; }
        public bool Accessible { get; set; }
        public int MaxRVLength { get; set; }
        public bool Utilities { get; set; }
        public decimal Cost { get; set; }
        public string CampgroundName { get; set; }


        public string ToString(int lengthOfStay) //This ToString is used for the parkwide search. We needed to return the campground name as a result
        {
            string accessibleString = (Accessible == true) ? "Yes" : "No";

            string maxRvLength = (MaxRVLength > 0) ? MaxRVLength.ToString() : "N/A";

            string utilityString = (Utilities == true) ? "Yes" : "N/A";

            string siteString = $"{CampgroundName}".PadRight(25) + $"{SiteID}".PadRight(10).PadLeft(5) + $"{MaxOccupancy}".PadRight(14) + $"{accessibleString}".PadRight(10) + $"{maxRvLength}".PadRight(12) + $"{utilityString}".PadRight(12) + $"{lengthOfStay * Cost:C}";

            return siteString;
        }

        public string SpecificCampsiteToString(int lengthOfStay) //This ToString is used specifically when printing from a designated campground
        {
            string accessibleString = (Accessible == true) ? "Yes" : "No";

            string maxRvLength = (MaxRVLength > 0) ? MaxRVLength.ToString() : "N/A";

            string utilityString = (Utilities == true) ? "Yes" : "N/A";

            string siteString = $"{SiteID}".PadRight(10).PadLeft(5) + $"{MaxOccupancy}".PadRight(14) + $"{accessibleString}".PadRight(10) + $"{maxRvLength}".PadRight(12) + $"{utilityString}".PadRight(12) + $"{lengthOfStay * Cost:C}";

            return siteString;
        }
    }
}
