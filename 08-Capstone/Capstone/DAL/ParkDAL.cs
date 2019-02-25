using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;

namespace Capstone.DAL
{
    public class ParkDAL
    {
        private string connectionString;
        private const string SQL_ListOfParks = @"SELECT * FROM park ORDER BY name";

        public ParkDAL(string DatabaseConnection)
        {
            connectionString = DatabaseConnection;
        }

        public IList<Park> GetParkList()
        {
            List<Park> parkList = new List<Park>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(SQL_ListOfParks, conn);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Park park = new Park();

                        park.ParkID = Convert.ToInt32(reader["park_id"]);
                        park.Name = Convert.ToString(reader["name"]);
                        park.Location = Convert.ToString(reader["location"]);
                        park.DateEstablished = Convert.ToDateTime(reader["establish_date"]);
                        park.Area = Convert.ToInt32(reader["area"]);
                        park.Visitors = Convert.ToInt32(reader["visitors"]);
                        park.Description = Convert.ToString(reader["description"]);

                        parkList.Add(park);

                    }

                    
                }
            }
            catch (Exception)
            {

                Console.WriteLine("The database is not accessible. Please yell at Grant or Patrick!");
            }

            return parkList;
        }

    }
}
