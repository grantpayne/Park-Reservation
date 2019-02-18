using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using dao_exercises.Models;


namespace dao_exercises.DAL
{
    class DepartmentSqlDAL
    {
        private string connectionString;
        private const string SQL_SelectDepartments = @"SELECT * FROM department";
        private const string SQL_CreateDepartment = "INSERT INTO department(name) VALUES (@name)";
        private const string SQL_GetNewestDepartmentID = "SELECT department_id FROM department WHERE name = @name";
        private const string SQL_UpdateDepartment = "UPDATE department SET name = @name WHERE department_id = @id";

        // Single Parameter Constructor
        public DepartmentSqlDAL(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        /// <summary>
        /// Returns a list of all of the departments.
        /// </summary>
        /// <returns></returns>
        public IList<Department> GetDepartments()
        {
            List<Department> departmentList = new List<Department>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(SQL_SelectDepartments, conn);

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Department department = new Department();
                        department.Id = Convert.ToInt32(reader["department_id"]);
                        department.Name = Convert.ToString(reader["name"]);

                        departmentList.Add(department);
                    }

                }
            }
            catch
            {
                throw;
            }
            return departmentList;
        }

        /// <summary>
        /// Creates a new department.
        /// </summary>
        /// <param name="newDepartment">The department object.</param>
        /// <returns>The id of the new department (if successful).</returns>
        public int CreateDepartment(Department newDepartment)
        {
            int result = 0;

            try
            {

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(SQL_CreateDepartment, conn);
                    cmd.Parameters.AddWithValue("@name", newDepartment.Name);
                    cmd.ExecuteNonQuery();

                    SqlCommand cmd2 = new SqlCommand(SQL_GetNewestDepartmentID, conn);
                    cmd2.Parameters.AddWithValue("@name", newDepartment.Name);

                    result = Convert.ToInt32(cmd2.ExecuteScalar());
                }
            }
            catch
            {
                throw;
            }
            return result;
        }

        /// <summary>
        /// Updates an existing department.
        /// </summary>
        /// <param name="updatedDepartment">The department object.</param>
        /// <returns>True, if successful.</returns>
        public bool UpdateDepartment(Department updatedDepartment)
        {
            bool result = false;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(SQL_UpdateDepartment, conn);
                    cmd.Parameters.AddWithValue("@id", updatedDepartment.Id);
                    cmd.Parameters.AddWithValue("@name", updatedDepartment.Name);

                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        result = true;
                    }
                    

                    return result;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}
