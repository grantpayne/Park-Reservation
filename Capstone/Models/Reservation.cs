﻿using System;
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
            string result = string.Format("{0, 10} - {1, 10} {2, 9} {3, 30} {4, 10} {5, 25} {6, 11}", 

                From_date.ToShortDateString().PadRight(10),                                             //{0}

                To_date.ToShortDateString().PadRight(10),                                               //{1}

                Reservation_id.ToString().PadRight(6),                                                  //{2}

                Name.PadRight(30),                                                                      //{3}

                Site_number.ToString().PadRight(5),                                                     //{4}

                Campground.PadRight(25),                                                                //{5}

                Create_date.ToShortDateString().PadRight(10));                                          //{6}

            return result;
        }
    }
}
