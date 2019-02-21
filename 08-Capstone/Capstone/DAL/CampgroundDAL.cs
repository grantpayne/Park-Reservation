using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.DAL
{
    public class CampgroundDAL
    {
        private string connectionString;

        public CampgroundDAL(string DatabaseConnection)
        {
            connectionString = DatabaseConnection;
        } 
    }
}
