using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;

namespace Capstone.DAL
{
    public class CampgroundDAL
    {
        private string connectionString;
        private const string SQL_GetCampgroundList = @"SELECT * FROM campground WHERE park_id = @park_id";

        public CampgroundDAL(string DatabaseConnection)
        {
            connectionString = DatabaseConnection;
        }

        public IList<Campground> GetCampgroundList(int parkID)
        {
            List<Campground> campgroundList = new List<Campground>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(SQL_GetCampgroundList, conn);
                    cmd.Parameters.AddWithValue("@park_id", parkID);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Campground campground = new Campground();

                        campground.Campground_id = Convert.ToInt32(reader["campground_id"]);
                        campground.Park_id = Convert.ToInt32(reader["park_id"]);
                        campground.Name = Convert.ToString(reader["name"]);
                        campground.Open_from_mm = Convert.ToInt32(reader["open_from_mm"]);
                        campground.Open_to_mm = Convert.ToInt32(reader["open_to_mm"]);
                        campground.Daily_fee = Convert.ToDecimal(reader["daily_fee"]);

                        campgroundList.Add(campground);

                    }


                }
            }
            catch (Exception ex)
            {

                Console.WriteLine("There was an error connecting/retrieving to the database"); //TODO: refine error handling
            }

            return campgroundList;
        }
    }
}
