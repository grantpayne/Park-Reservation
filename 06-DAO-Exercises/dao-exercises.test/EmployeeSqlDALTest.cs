using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Transactions;

namespace dao_exercises.test
{
    [TestClass]
    public class EmployeeSqlDALTest
    {
        private string connectionString = @"Data Source=.\sqlexpress;Initial Catalog=EmployeeDB;Integrated Security=True";

        private TransactionScope tran;

        [TestMethod]
        public void TestMethod1()
        {
        }
    }
}
