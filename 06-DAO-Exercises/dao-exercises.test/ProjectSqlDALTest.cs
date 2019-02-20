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
    public class ProjectSqlDALTest
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

                SqlCommand cmd;
            }
        }

        [TestCleanup]
        public void Cleanup()
        {
            tran.Dispose();
        }

        [TestMethod]
        public void CreateProjectTest()
        {
            ProjectSqlDAL project = new ProjectSqlDAL(connectionString);
            Project testProject = new Project();
            testProject.Name = "testProject";
            testProject.StartDate = DateTime.Parse("2018-01-01");
            testProject.EndDate = DateTime.Parse("2018-02-01");

            int projectIdReturnedFromMethod = project.CreateProject(testProject);

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd;

                cmd = new SqlCommand("SELECT * FROM project WHERE name = 'testProject' AND project_id = @testProjectID", conn);
                cmd.Parameters.AddWithValue("@testProjectID", projectIdReturnedFromMethod);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Assert.AreEqual(testProject.Name, Convert.ToString(reader["name"]));
                    Assert.AreEqual(testProject.StartDate, Convert.ToDateTime(reader["from_date"]));
                    Assert.AreEqual(testProject.EndDate, Convert.ToDateTime(reader["to_date"]));
                    Assert.AreEqual(projectIdReturnedFromMethod, Convert.ToInt32(reader["project_id"]));

                }
            }


        }

        [TestMethod]
        public void RemoveEmployeeFromProjectTest()
        {
            ProjectSqlDAL projectSqlDAL = new ProjectSqlDAL(connectionString);
            projectSqlDAL.RemoveEmployeeFromProject(1, 3);

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd;

                cmd = new SqlCommand("SELECT COUNT(*) FROM project_employee WHERE project_id = 1 AND employee_id = 3", conn);
                int result = (int)cmd.ExecuteScalar();
                Assert.AreEqual(0, result);
            }
        }

        [TestMethod]
        public void AssignEmployeeToProjectTest()
        {
            ProjectSqlDAL projectSqlDAL = new ProjectSqlDAL(connectionString);
            projectSqlDAL.AssignEmployeeToProject(1, 7);

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd;

                cmd = new SqlCommand("SELECT COUNT(*) FROM project_employee WHERE project_id = 1 AND employee_id = 7", conn);
                int result = (int)cmd.ExecuteScalar();
                Assert.AreEqual(1, result);
            }
        }

        [TestMethod]
        public void GetAllProjectsTest()
        {
            DepartmentSqlDAL departmentSqlDAL = new DepartmentSqlDAL(connectionString);
            IList<Department> deps = departmentSqlDAL.GetDepartments();

            Assert.IsNotNull(deps);
        }
    }
}
