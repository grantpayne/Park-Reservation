using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;


namespace Capstone.DAL
{
    public class ReservationDAL
    {
        private const string SQL_MakeReservation = "INSERT INTO reservation(site_id, name, from_date, to_date, create_date) VALUES(@siteID, @name, @fromDate, @toDate, GETDATE()); SELECT CAST(SCOPE_IDENTITY() AS int);";
        private string connectionString;

        public ReservationDAL(string DatabaseConnection)
        {
            connectionString = DatabaseConnection;
        }

        public int MakeReservation(string reqFromDate, string reqToDate, int siteID, string name)
        {
            int result;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(SQL_MakeReservation, conn);
                    cmd.Parameters.AddWithValue("@siteID", siteID);
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@fromDate", reqFromDate);
                    cmd.Parameters.AddWithValue("@toDate", reqToDate);

                    result = (int)cmd.ExecuteScalar();
                }
            }
            catch (Exception)
            {

                throw;
            }

            return result;
        }

    }
}
