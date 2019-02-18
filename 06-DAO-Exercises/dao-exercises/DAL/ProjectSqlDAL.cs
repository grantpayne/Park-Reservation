using System;
using System.Collections.Generic;
using System.Text;
using dao_exercises.Models;
using System.Data.SqlClient;

namespace dao_exercises.DAL
{
    class ProjectSqlDAL
    {
        private string connectionString;
        private const string SQL_GetAllProjects = "SELECT * FROM project";
        private const string SQL_AssignEmployee = "INSERT INTO project_employee(employee_id, project_id) VALUES (@employee, @project)";
        private const string SQL_UnassignEmployee = "DELETE FROM project_employee WHERE employee_id = @employee AND project_id = @project";
        private const string SQL_CreateProject = "INSERT INTO project(name, from_date, to_date) VALUES (@name, @fromDate, @toDate)";
        private const string SQL_GetNewestProjectID = "SELECT MAX(project_id) FROM project";

        // Single Parameter Constructor
        public ProjectSqlDAL(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        /// <summary>
        /// Returns all projects.
        /// </summary>
        /// <returns></returns>
        public IList<Project> GetAllProjects()
        {
            List<Project> output = new List<Project>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(SQL_GetAllProjects, conn);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Project project = new Project();
                        project.ProjectId = Convert.ToInt32(reader["project_id"]);
                        project.Name = Convert.ToString(reader["name"]);
                        project.StartDate = Convert.ToDateTime(reader["from_date"]);
                        project.EndDate = Convert.ToDateTime(reader["to_date"]);

                        output.Add(project);
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
        /// Assigns an employee to a project using their IDs.
        /// </summary>
        /// <param name="projectId">The project's id.</param>
        /// <param name="employeeId">The employee's id.</param>
        /// <returns>If it was successful.</returns>
        public bool AssignEmployeeToProject(int projectId, int employeeId)
        {
            bool result = false;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(SQL_AssignEmployee, conn);
                    cmd.Parameters.AddWithValue("@employee", employeeId);
                    cmd.Parameters.AddWithValue("@project", projectId);

                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        result = true;
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }

            return result;
        }

        /// <summary>
        /// Removes an employee from a project.
        /// </summary>
        /// <param name="projectId">The project's id.</param>
        /// <param name="employeeId">The employee's id.</param>
        /// <returns>If it was successful.</returns>
        public bool RemoveEmployeeFromProject(int projectId, int employeeId)
        {
            bool result = false;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(SQL_UnassignEmployee, conn);
                    cmd.Parameters.AddWithValue("@employee", employeeId);
                    cmd.Parameters.AddWithValue("@project", projectId);

                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        result = true;
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }

            return result;
        }

        /// <summary>
        /// Creates a new project.
        /// </summary>
        /// <param name="newProject">The new project object.</param>
        /// <returns>The new id of the project.</returns>
        public int CreateProject(Project newProject)
        {
            int result = 0;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(SQL_CreateProject, conn);
                    cmd.Parameters.AddWithValue("@name", newProject.Name);
                    cmd.Parameters.AddWithValue("@fromDate", Convert.ToDateTime(newProject.StartDate));
                    cmd.Parameters.AddWithValue("@toDate", Convert.ToDateTime(newProject.EndDate));
                    cmd.ExecuteNonQuery();

                    SqlCommand cmd2 = new SqlCommand(SQL_GetNewestProjectID, conn);
                    result = Convert.ToInt32(cmd2.ExecuteScalar());
                }
            }
            catch (Exception)
            {

                throw;
            }
            return result;
        }

    }
}
