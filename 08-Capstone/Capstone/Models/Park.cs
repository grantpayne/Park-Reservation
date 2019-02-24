using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Models
{
    public class Park
    {
        public int ParkID { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public DateTime DateEstablished { get; set; }
        public int Area { get; set; }
        public int Visitors { get; set; }
        public string Description { get; set; }

        public override string ToString() 
        {
            string parkString = $"Location: {Location}\nEstablished: {DateEstablished.ToShortDateString()}\nArea: {Area} Acres\nAnnual Visitors: {Visitors}\n\n{Description}";
            return parkString;
        }
    }
}
