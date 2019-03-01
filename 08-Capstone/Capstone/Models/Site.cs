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


        public string ToString(int lengthOfStay, bool parkwideSearch) //This ToString takes a boolean to determine if the search is parkwide or not.
        {
            string accessibleString = (Accessible == true) ? "Yes" : "No";

            string maxRvLength = (MaxRVLength > 0) ? MaxRVLength.ToString() : "N/A";

            string utilityString = (Utilities == true) ? "Yes" : "N/A";

            string shortenedCampgroundName;
            string parkwideAdditionString;

            if (CampgroundName.Length > 22)
            {
                shortenedCampgroundName = CampgroundName.Substring(0, 18);
                shortenedCampgroundName += "...";
                parkwideAdditionString = $"{shortenedCampgroundName}".PadRight(25);
            }
            else
            {
                parkwideAdditionString = $"{CampgroundName}".PadRight(25);
            }

            string baseString = $"{SiteID}".PadRight(10).PadLeft(5) + $"{MaxOccupancy}".PadRight(14) + $"{accessibleString}".PadRight(10) + $"{maxRvLength}".PadRight(12) + $"{utilityString}".PadRight(12) + $"{lengthOfStay * Cost:C}";

            string result;

            if (parkwideSearch)
            {
                result = parkwideAdditionString + baseString;

            }

            else
            {
                result = baseString;
            }

            return result;
        }

    }
}
