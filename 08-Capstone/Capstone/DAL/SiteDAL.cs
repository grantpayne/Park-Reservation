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
        private string SQL_GetUnreservedCampsitesByCampground = @"SELECT TOP 5 * FROM site JOIN campground ON site.campground_id = campground.campground_id WHERE (@reqToMM BETWEEN open_from_mm AND open_to_mm) AND (@reqFromMM BETWEEN open_from_mm AND open_to_mm) AND (site.site_id IN (SELECT DISTINCT site.site_id FROM site LEFT JOIN reservation ON reservation.site_id = site.site_id WHERE (reservation_id IS NULL AND site.campground_id = @campgroundID) OR ((site.campground_id = @campgroundID AND NOT ((@reqFromDate <= reservation.to_date AND @reqToDate >= reservation.from_date) OR (reservation.from_date <= @reqFromDate AND reservation.to_date >= @reqToDate))))))";
        private const string SQL_GetCost = @"SELECT daily_fee FROM campground WHERE campground_id = @campgroundID";
        private const string SQL_Advanced_Occupancy = "AND (max_occupancy >= @occupancy)";
        private const string SQL_Advanced_Accessibility = "AND (accessible = 1)";
        private const string SQL_Advanced_RvLength = "AND (max_rv_length >= @maxRvLength)";
        private const string SQL_Advanced_Utilities = "AND (utilities = 1)";
        private decimal cost;
        

        public SiteDAL(string DatabaseConnection)
        {
            connectionString = DatabaseConnection;
        }

        public IList<Site> GetUnreservedCampsites(string reqFromDate, string reqToDate, int campgroundID, int occupancy, bool accessibility, int maxRvLength, bool utilities)
        {
            if (occupancy > 0)
            {
                SQL_GetUnreservedCampsitesByCampground += SQL_Advanced_Occupancy;
            }
            if (accessibility)
            {
                SQL_GetUnreservedCampsitesByCampground += SQL_Advanced_Accessibility;
            }
            if (maxRvLength > 0)
            {
                SQL_GetUnreservedCampsitesByCampground += SQL_Advanced_RvLength;
            }
            if (utilities)
            {
                SQL_GetUnreservedCampsitesByCampground += SQL_Advanced_Utilities;
            }
            IList<Site> resultList = new List<Site>();
            int reqFromMM = CLIHelper.ExtractMonth(reqFromDate);
            int reqToMM = CLIHelper.ExtractMonth(reqToDate);

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(SQL_GetCost, conn);
                    cmd.Parameters.AddWithValue("@campgroundID", campgroundID);
                    cost = Convert.ToDecimal(cmd.ExecuteScalar());

                    cmd = new SqlCommand(SQL_GetUnreservedCampsitesByCampground, conn);
                    cmd.Parameters.AddWithValue("@campgroundID", campgroundID);
                    cmd.Parameters.AddWithValue("@reqFromDate", reqFromDate);
                    cmd.Parameters.AddWithValue("@reqToDate", reqToDate);
                    cmd.Parameters.AddWithValue("@reqFromMM", reqFromMM);
                    cmd.Parameters.AddWithValue("@reqToMM", reqToMM);
                    cmd.Parameters.AddWithValue("@occupancy", occupancy);
                    cmd.Parameters.AddWithValue("@maxRvLength", maxRvLength);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Site site = new Site();
                        site.SiteID = Convert.ToInt32(reader["site_id"]);
                        site.MaxOccupancy = Convert.ToInt32(reader["max_occupancy"]);
                        site.Accessible = Convert.ToBoolean(reader["accessible"]);
                        site.MaxRVLength = Convert.ToInt32(reader["max_rv_length"]);
                        site.Utilities = Convert.ToBoolean(reader["utilities"]);
                        site.SiteNumber = Convert.ToInt32(reader["site_number"]);
                        site.Cost = cost;
                        resultList.Add(site);
                    }

                    
                }
            }
            catch (Exception)
            {

                throw;
            }

            return resultList;
        }
    }
}

