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
    public class DepartmentSqlDALTest
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
                SqlCommand cmd = new SqlCommand("INSERT INTO department(name) VALUES ('TestDepartment')", conn);
                cmd.ExecuteNonQuery();
            }
        }

        [TestCleanup]
        public void Cleanup()
        {
            tran.Dispose();
        }

        [TestMethod]
        public void GetDepartmentsTest()
        {
            DepartmentSqlDAL departmentSqlDAL = new DepartmentSqlDAL(connectionString);
            IList<Department> departments = departmentSqlDAL.GetDepartments();

            Assert.IsNotNull(departments);

            bool collectionResult = false;
            foreach (Department department in departments)
            {

                if ((department.Name == "TestDepartment"))
                {
                    collectionResult = true;
                }
            }

            Assert.IsTrue(collectionResult);
        }

        [TestMethod]
        public void UpdateDepartmentTest()
        {
            DepartmentSqlDAL departmentSqlDAL = new DepartmentSqlDAL(connectionString);
            Department department = new Department();
            department.Name = "testDepartmentNameChange";
            int testDepartmentID = departmentSqlDAL.CreateDepartment(department);
            department.Id = testDepartmentID;
            department.Name = "testDepartmentChanged";
            bool result = departmentSqlDAL.UpdateDepartment(department);
            Assert.IsTrue(result);
        }


        [TestMethod]
        public void CreateDepartmentTest()
        {
            DepartmentSqlDAL departmentSqlDAL = new DepartmentSqlDAL(connectionString);
            Department department = new Department();
            department.Name = "TestCreateDepartment";
            int departmentID = departmentSqlDAL.CreateDepartment(department);
            IList<Department> depList = departmentSqlDAL.GetDepartments();

            bool result = false;
            foreach (Department departments in depList)
            {
                if (departments.Id == departmentID && departments.Name == "TestCreateDepartment")
                {
                    result = true;
                }
            }

            Assert.IsTrue(result);

        }
    }
}
