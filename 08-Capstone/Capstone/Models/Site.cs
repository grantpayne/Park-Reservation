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

            string baseString = $"{SiteID}".PadRight(10).PadLeft(5) + $"{MaxOccupancy}".PadRight(14) + $"{accessibleString}".PadRight(10) + $"{maxRvLength}".PadRight(12) + $"{utilityString}".PadRight(12) + $"{lengthOfStay * Cost:C}";

            string result;

            string shortenedCampgroundName;

            string parkwideAdditionString;

            if (parkwideSearch)
            {

                if (CampgroundName.ToString().Length > 22) //This statement adjusts the length of long campground names to protect formatting
                {
                    shortenedCampgroundName = CampgroundName.ToString().Substring(0, 18);
                    shortenedCampgroundName += "...";
                    parkwideAdditionString = $"{shortenedCampgroundName}".PadRight(25);
                }
                else
                {
                    parkwideAdditionString = $"{CampgroundName}".PadRight(25);
                }

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
