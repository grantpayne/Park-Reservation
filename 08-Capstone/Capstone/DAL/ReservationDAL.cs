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
        private const string SQL_Get30DayReservations = "SELECT * FROM reservation JOIN site ON reservation.site_id = site.site_id JOIN campground ON site.campground_id = campground.campground_id JOIN park ON campground.park_id = park.park_id WHERE park.park_id = @parkID AND (reservation.from_date BETWEEN GETDATE() AND (GETDATE() + 30)) ORDER BY reservation.from_date;";
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

        public IList<Reservation> Get30DayReservations(int parkID)
        {
            IList<Reservation> reservationList = new List<Reservation>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(SQL_Get30DayReservations, conn);
                    cmd.Parameters.AddWithValue("@parkID", parkID);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while(reader.Read())
                    {
                        Reservation reservation = new Reservation();

                        reservation.From_date = Convert.ToDateTime(reader["reservation.from_date"]);
                        reservation.To_date = Convert.ToDateTime(reader["reservation.to_date"]);
                        reservation.Reservation_id = Convert.ToInt32(reader["reservation.reservation_id"]);
                        reservation.Name = Convert.ToString(reader["reservation.name"]);
                        reservation.Site_number = Convert.ToInt32(reader["site.site_number"]);
                        reservation.Campground = Convert.ToString(reader["campground.name"]);
                        reservation.Create_date = Convert.ToDateTime(reader["reservation.create_date"]);

                        reservationList.Add(reservation);
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            return reservationList;
        }
    }
}
