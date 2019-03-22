using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.SqlClient;
using System.Transactions;
using Capstone.DAL;
using System.Collections.Generic;
using Capstone.Models;

namespace Capstone.Tests
{
    [TestClass]
    public class ReservationDALTest
    {
        private TransactionScope tran;
        private string connectionString = @"Data Source=.\sqlexpress;Initial Catalog=NationalParkReservation;Integrated Security=True"; //TODO: Should this be hard-coded here?
        private int createdParkID;
        private int createdCampgroundID;
        private int createdSiteID;

        [TestInitialize]
        public void Initialize()
        {
            tran = new TransactionScope();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO park(name, location, establish_date, area, visitors, description) VALUES('Test Park', 'Test Location', '2019-02-21', 1, 1, 'This is a test descripton.'); SELECT CAST(SCOPE_IDENTITY() AS int);", conn);
                createdParkID = (int)cmd.ExecuteScalar();
                cmd = new SqlCommand("INSERT INTO campground(park_id, name, open_from_mm, open_to_mm, daily_fee) VALUES(@park_id, 'Test Campground', 1, 12, 100); SELECT CAST(SCOPE_IDENTITY() AS int);", conn);
                cmd.Parameters.AddWithValue("@park_id", createdParkID);
                createdCampgroundID = (int)cmd.ExecuteScalar();
                cmd = new SqlCommand("INSERT INTO site(campground_id, site_number, max_occupancy, accessible, max_rv_length, utilities) VALUES(@createdCampgroundID, 1, 1, 1, 0, 1); SELECT CAST(SCOPE_IDENTITY() AS int);", conn);
                cmd.Parameters.AddWithValue("@createdCampgroundID", createdCampgroundID);
                createdSiteID = (int)cmd.ExecuteScalar();
                cmd = new SqlCommand("INSERT INTO reservation(site_id, name, from_date, to_date, create_date) VALUES(@createdSiteID, 'Smith Family', '2025-01-01', '2025-03-01', GETDATE());", conn);
                cmd.Parameters.AddWithValue("@createdSiteID", createdSiteID);
                cmd.ExecuteNonQuery();
                cmd = new SqlCommand("INSERT INTO reservation(site_id, name, from_date, to_date, create_date) VALUES(@createdSiteID, 'Smith Family', '2025-04-01', '2025-05-01', GETDATE());", conn);
                cmd.Parameters.AddWithValue("@createdSiteID", createdSiteID);
                cmd.ExecuteNonQuery();
                cmd = new SqlCommand("INSERT INTO reservation(site_id, name, from_date, to_date, create_date) VALUES(@createdSiteID, 'Within 30 Days', GETDATE() + 2, GETDATE() + 15, GETDATE());", conn);
                cmd.Parameters.AddWithValue("@createdSiteID", createdSiteID);
                cmd.ExecuteNonQuery();
                cmd = new SqlCommand("INSERT INTO reservation(site_id, name, from_date, to_date, create_date) VALUES(@createdSiteID, 'Beyond 30 Days', GETDATE() + 31, GETDATE() + 40, GETDATE());", conn);
                cmd.Parameters.AddWithValue("@createdSiteID", createdSiteID);
                cmd.ExecuteNonQuery();

            }

        }

        [TestCleanup]
        public void Cleanup()
        {
            tran.Dispose();
        }

        [TestMethod]
        public void MakeReservationTest()
        {
            ReservationDAL reservationDAL = new ReservationDAL(connectionString);

            int testReservationID = reservationDAL.MakeReservation("2030-01-01", "2030-02-01", createdSiteID, "Test Reservation");

            Assert.IsNotNull(testReservationID);
            Assert.IsTrue(testReservationID > 0);
        }

        [TestMethod]
        public void Get30DayReservationTest()
        {
            ReservationDAL reservationDAL = new ReservationDAL(connectionString);

            IList<Reservation> Day30ReservationTestList = new List<Reservation>();

            Day30ReservationTestList = reservationDAL.Get30DayReservations(createdParkID);

            Assert.IsNotNull(Day30ReservationTestList);

            bool containsWithin30Days = false;
            bool containsBeyond30Days = false;

            foreach (Reservation reservation in Day30ReservationTestList)
            {
                if (reservation.Name == "Within 30 Days")
                {
                    containsWithin30Days = true;
                }
                if (reservation.Name == "Beyond 30 Days")
                {
                    containsBeyond30Days = true;
                }
            }

            Assert.IsTrue(containsWithin30Days, "The List returned should include the reservation \"Within 30 Days\"");
            Assert.IsFalse(containsBeyond30Days, "The List returned should not include the reservation \"Beyond 30 Days\"");

        }

    }
}
