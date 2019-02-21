using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.SqlClient;
using System.Transactions;
using Capstone.DAL;
using System.Collections.Generic;
using Capstone.Models;

namespace Capstone.Tests
{
    [TestClass]
    public class ParkDALTest
    {
        private TransactionScope tran;
        private string connectionString = @"Data Source=.\sqlexpress;Initial Catalog=NationalParkReservation;Integrated Security=True"; //TODO: Should this be hard-coded here?

        [TestInitialize]
        public void Initialize()
        {
            tran = new TransactionScope();
            
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO park(name, location, establish_date, area, visitors, description) VALUES('Test Park', 'Test Location', '2019-02-21', 1, 1, 'This is a test descripton.')", conn);
                cmd.ExecuteNonQuery();
            }
            
        }

        [TestCleanup]
        public void Cleanup()
        {
            tran.Dispose();
        }

        [TestMethod]
        public void GetParkListTest()
        {
            ParkDAL parkDAL = new ParkDAL(connectionString);
            IList<Park> testParkList = new List<Park>();
            bool result = false;
            testParkList = parkDAL.GetParkList();

            foreach (Park park in testParkList)
            {
                if (park.Name == "Test Park")
                {
                    result = true;
                }
            }

            Assert.IsNotNull(testParkList);
            Assert.IsTrue(result);
        }
    }
}