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

                cmd = new SqlCommand("INSERT INTO project(name, from_date, to_date) VALUES (testProject, 1900-01-01, 2000-01-01); SELECT CAST(SCOPE_IDENTITY() as int);", conn);
                int testID = (int)cmd.ExecuteScalar();

                cmd = new SqlCommand("INSERT INTO project_employee(project_id, employee_id) VALUES (@testID, 1)");
                cmd.Parameters.AddWithValue("@testID", testID);
                cmd.ExecuteNonQuery();

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

            int projectId = project.CreateProject(testProject); //TODO Here is where we were

            
        }

        [TestMethod]
        public void RemoveEmployeeFromProjectTest()
        {

        }

        [TestMethod]
        public void AssignEmployeeToProjectTest()
        {

        }
    }
}
