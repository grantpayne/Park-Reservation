using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.SqlClient;
using System.Transactions;
using Capstone.DAL;
using System.Collections.Generic;
using Capstone.Models;

namespace Capstone.Tests
{
    [TestClass]
    public class CampgroundDALTest
    {

        private TransactionScope tran;
        private string connectionString = @"Data Source=.\sqlexpress;Initial Catalog=NationalParkReservation;Integrated Security=True"; //TODO: Should this be hard-coded here?
        private int createdParkID;

        [TestInitialize]
        public void Initialize()
        {
            tran = new TransactionScope();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO park(name, location, establish_date, area, visitors, description) VALUES('Test Park', 'Test Location', '2019-02-21', 1, 1, 'This is a test descripton.'); SELECT CAST(SCOPE_IDENTITY() AS int);", conn);
                createdParkID = (int)cmd.ExecuteScalar();
                cmd = new SqlCommand("INSERT INTO campground(park_id, name, open_from_mm, open_to_mm, daily_fee) VALUES(@park_id, 'Test Campground', 1, 12, 100)", conn);
                cmd.Parameters.AddWithValue("@park_id", createdParkID);
                cmd.ExecuteNonQuery();
            }

        }

        [TestCleanup]
        public void Cleanup()
        {
            tran.Dispose();
        }


        [TestMethod]
        public void GetCampgroundListTest()
        {
            IList<Campground> testCampgroundList = new List<Campground>();
            CampgroundDAL campgroundDAL = new CampgroundDAL(connectionString);
            testCampgroundList = campgroundDAL.GetCampgroundList(createdParkID);

            
            bool result = false;

            foreach (Campground campground in testCampgroundList)
            {
                if (campground.Name == "Test Campground" && campground.Park_id == createdParkID)
                {
                    result = true;
                }
            }

            Assert.IsNotNull(testCampgroundList);
            Assert.IsTrue(result);
        }
    }
}
