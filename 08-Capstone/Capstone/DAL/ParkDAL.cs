using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.DAL
{
    public class ParkDAL
    {
        private string connectionString;
        private const string SQL_ListOfParks = @"SELECT * FROM park ORDER BY name";

        public ParkDAL(string DatabaseConnection)
        {
            connectionString = DatabaseConnection;
        }

    }
}
