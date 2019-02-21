using System;
using System.Collections.Generic;
using System.Text;

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
}
