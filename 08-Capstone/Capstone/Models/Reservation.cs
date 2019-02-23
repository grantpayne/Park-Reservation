using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Models
{
    public class Reservation
    {
        public int Reservation_id { get; set; }
        public int Site_id { get; set; }
        public string Name { get; set; }
        public DateTime From_date { get; set; }
        public DateTime To_date { get; set; }
        public DateTime Create_date { get; set; }

        public override string ToString() //TODO:  Will need to implement this for Bonus showing all reservations out 30 days
        {
            return "";
        }
    }
}
