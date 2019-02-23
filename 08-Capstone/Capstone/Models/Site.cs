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

        public override string ToString() 
        {
            string accessibleString = (Accessible == true) ? "Yes" : "No";

            string maxRvLength = (MaxRVLength > 0) ? MaxRVLength.ToString() : "N/A";

            string utilityString = (Accessible == true) ? "Yes" : "N/A";

            string siteString = $"{SiteID}  {MaxOccupancy}  {accessibleString} {maxRvLength}  {utilityString}  {Cost:C}";

            return siteString;
        }

        public string ToString(int lengthOfStay)
        {
            string accessibleString = (Accessible == true) ? "Yes" : "No";

            string maxRvLength = (MaxRVLength > 0) ? MaxRVLength.ToString() : "N/A";

            string utilityString = (Accessible == true) ? "Yes" : "N/A";

            string siteString = $"{SiteID}  {MaxOccupancy}  {accessibleString} {maxRvLength}  {utilityString}  {lengthOfStay * Cost:C}";

            return siteString;
        }
    }
}
