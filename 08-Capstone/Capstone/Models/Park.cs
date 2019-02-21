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
        public DateTime DateEstablished { get; set; } //TODO double check this data type
        public int Area { get; set; }
        public int Visitors { get; set; }
        public string Description { get; set; }

        public override string ToString() //TODO fill out the override string
        {
            return base.ToString();
        }
    }
}
