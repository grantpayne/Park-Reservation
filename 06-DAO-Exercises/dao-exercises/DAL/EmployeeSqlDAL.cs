using System;
using System.Collections.Generic;
using System.Text;
using dao_exercises.Models;
using System.Data.SqlClient;

namespace dao_exercises.DAL
{
    class EmployeeSqlDAL
    {
        private string connectionString;
        private const string SQL_GetAllEmployees = "SELECT * FROM employee";
        private const string SQL_SearchEmployees = "SELECT * FROM employee WHERE first_name LIKE @firstNameInput OR last_name LIKE @lastNameInput";
        private const string SQL_FindUnassignedEmployees = "SELECT * FROM employee WHERE employee.employee_id NOT IN (SELECT employee_id FROM project_employee)";

        // Single Parameter Constructor
        public EmployeeSqlDAL(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        /// <summary>
        /// Returns a list of all of the employees.
        /// </summary>
        /// <returns>A list of all employees.</returns>
        public IList<Employee> GetAllEmployees()
        {
            List<Employee> output = new List<Employee>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(SQL_GetAllEmployees, conn);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Employee employee = new Employee();
                        employee.EmployeeId = Convert.ToInt32(reader["employee_id"]);
                        employee.DepartmentId = Convert.ToInt32(reader["department_id"]);
                        employee.FirstName = Convert.ToString(reader["first_name"]);
                        employee.LastName = Convert.ToString(reader["last_name"]);
                        employee.JobTitle = Convert.ToString(reader["job_title"]);
                        employee.BirthDate = Convert.ToDateTime(reader["birth_date"]);
                        employee.Gender = Convert.ToString(reader["gender"]);
                        employee.HireDate = Convert.ToDateTime(reader["hire_date"]);

                        output.Add(employee);
                    }

                }
            }
            catch (Exception)
            {

                throw;
            }

            return output;
        }

        /// <summary>
        /// Searches the system for an employee by first name or last name.
        /// </summary>
        /// <remarks>The search performed is a wildcard search.</remarks>
        /// <param name="firstname"></param>
        /// <param name="lastname"></param>
        /// <returns>A list of employees that match the search.</returns>
        public IList<Employee> Search(string firstname, string lastname)
        {
            List<Employee> output = new List<Employee>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(SQL_SearchEmployees, conn);
                    cmd.Parameters.AddWithValue("@firstNameInput", "%" + firstname + "%");
                    cmd.Parameters.AddWithValue("@lastNameInput", "%" + lastname + "%");
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Employee employee = new Employee();
                        employee.EmployeeId = Convert.ToInt32(reader["employee_id"]);
                        employee.DepartmentId = Convert.ToInt32(reader["department_id"]);
                        employee.FirstName = Convert.ToString(reader["first_name"]);
                        employee.LastName = Convert.ToString(reader["last_name"]);
                        employee.JobTitle = Convert.ToString(reader["job_title"]);
                        employee.BirthDate = Convert.ToDateTime(reader["birth_date"]);
                        employee.Gender = Convert.ToString(reader["gender"]);
                        employee.HireDate = Convert.ToDateTime(reader["hire_date"]);

                        output.Add(employee);
                    }

                }
            }
            catch (Exception)
            {
                throw;
            }

            return output;
        }

        /// <summary>
        /// Gets a list of employees who are not assigned to any active projects.
        /// </summary>
        /// <returns></returns>
        public IList<Employee> GetEmployeesWithoutProjects()
        {
            List<Employee> output = new List<Employee>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(SQL_FindUnassignedEmployees, conn);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Employee employee = new Employee();
                        employee.EmployeeId = Convert.ToInt32(reader["employee_id"]);
                        employee.DepartmentId = Convert.ToInt32(reader["department_id"]);
                        employee.FirstName = Convert.ToString(reader["first_name"]);
                        employee.LastName = Convert.ToString(reader["last_name"]);
                        employee.JobTitle = Convert.ToString(reader["job_title"]);
                        employee.BirthDate = Convert.ToDateTime(reader["birth_date"]);
                        employee.Gender = Convert.ToString(reader["gender"]);
                        employee.HireDate = Convert.ToDateTime(reader["hire_date"]);

                        output.Add(employee);
                    }

                }
            }
            catch (Exception)
            {

                throw;
            }

            return output;
        }
    }
}
