using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;


namespace Capstone.DAL
{
    public class SiteDAL
    {
        private string connectionString;

        public SiteDAL(string DatabaseConnection)
        {
            connectionString = DatabaseConnection;
        }
    }
}

