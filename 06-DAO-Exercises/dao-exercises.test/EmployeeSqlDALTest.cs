using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.SqlClient;
using System.Transactions;
using dao_exercises.DAL;
using dao_exercises.Models;
using System;
using System.Collections.Generic;

namespace dao_exercises.test
{
    [TestClass]
    public class EmployeeSqlDALTest
    {
        private string connectionString = @"Data Source=.\sqlexpress;Initial Catalog=EmployeeDB;Integrated Security=True";

        private TransactionScope tran;

        [TestInitialize]
        public void Initialize()
        {
            tran = new TransactionScope();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("INSERT INTO employee(department_id, first_name, last_name, job_title, birth_date, gender, hire_date) VALUES (1, 'TestFirstName', 'TestLastName', 'TestTitle', '1900-01-01', 'M', '1900-01-01');", conn);
                cmd.ExecuteNonQuery();

            }
        }

        [TestCleanup]
        public void Cleanup()
        {
            tran.Dispose();
        }

        [TestMethod]
        public void GetAllEmployeesTest()
        {
            EmployeeSqlDAL employeeSqlDAL = new EmployeeSqlDAL((connectionString));
            IList<Employee> employees = employeeSqlDAL.GetAllEmployees();

            Assert.IsNotNull(employees);
        }

        [TestMethod]
        public void SearchEmployeeTest()
        {
            EmployeeSqlDAL employeeSqlDAL = new EmployeeSqlDAL(connectionString);
            IList<Employee> result = employeeSqlDAL.Search("TestFirstName", "TestLastName");

            Assert.IsNotNull(result);
            bool collectionResult = false;

            foreach (Employee employee in result)
            {

                if ((employee.FirstName == "TestFirstName") && (employee.LastName == "TestLastName"))
                {
                    collectionResult = true;
                }
            }

            Assert.IsTrue(collectionResult);

        }

        [TestMethod]
        public void GetEmployeesWithoutProjectsTest()
        {
            EmployeeSqlDAL employeeSqlDAL = new EmployeeSqlDAL((connectionString));
            IList<Employee> employeeResult = employeeSqlDAL.GetEmployeesWithoutProjects();
            Assert.IsNotNull(employeeResult);

            bool collectionResult = false;
            foreach (Employee employee in employeeResult)
            {

                if ((employee.FirstName == "TestFirstName") && (employee.LastName == "TestLastName"))
                {
                    collectionResult = true;
                }
            }

            Assert.IsTrue(collectionResult);
        }
    }
}
