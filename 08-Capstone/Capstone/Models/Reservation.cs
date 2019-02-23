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
        public int Site_number { get; set; }
        public string Campground { get; set; }


        public override string ToString()
        {
            return $"{From_date.ToShortDateString()}  {To_date.ToShortDateString()}  {Reservation_id}  {Name}  {Site_number}  {Campground}  {Create_date.ToLongDateString()}";
        }
    }
}
