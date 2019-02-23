using Capstone.DAL;
using System;
using System.Collections.Generic;
using System.Text;
using Capstone.Models;

namespace Capstone
{
    public class ProgramCLI
    {
        const string DatabaseConnection = @"Data Source=.\sqlexpress;Initial Catalog=NationalParkReservation;Integrated Security=True";
        const string Command_Quit = "Q";

        public void RunCLI()
        {
            while (true)
            {
                Console.Clear();

                ParkDAL parkDAL = new ParkDAL(DatabaseConnection);
                IList<Park> parkList = new List<Park>();
                parkList = parkDAL.GetParkList();
                Console.WriteLine("Select a Park for Further Details\n");
                foreach (Park park in parkList)
                {
                    Console.WriteLine($"{park.ParkID}) {park.Name}");
                }
                Console.WriteLine($"{Command_Quit}) Quit\n");

                string command = CLIHelper.GetString("Selection:").ToUpper();
                int commandIfNum;
                if (command == Command_Quit)
                {
                    return;
                }
                else if (!int.TryParse(command, out commandIfNum))
                {
                    Console.WriteLine("Please make a valid selection from the menu. Press Enter to try again.");
                    Console.ReadLine();
                    continue;
                }
                else
                {
                    bool commandNumIsInParkList = false;
                    foreach (Park park in parkList)
                    {
                        if (park.ParkID == commandIfNum)
                        {
                            commandNumIsInParkList = true;
                            break;
                        }
                    }

                    if (commandNumIsInParkList)
                    {
                        ParkInfoScreen(commandIfNum);
                    }
                    else
                    {
                        Console.WriteLine("Please make a valid selection from the menu. Press Enter to try again.");
                        Console.ReadLine();
                    }
                }


            }
        }

        public void ParkInfoScreen(int parkID)
        {
            Park currentWorkingPark = new Park();

            ParkDAL parkDAL = new ParkDAL(DatabaseConnection);
            IList<Park> parks = parkDAL.GetParkList();
            foreach (Park park in parks)
            {
                if (park.ParkID == parkID)
                {
                    Console.Clear();
                    Console.WriteLine("Park Information Screen\n");
                    Console.WriteLine(park.ToString() + "\n");
                    currentWorkingPark = park;
                    break;
                }
            }

            while (true)
            {
                int command;

                command = CLIHelper.GetInteger("Select a Command\n1) View campgrounds\n2) Search for reservation\n3) View all reservations for the next 30 days\n4) Return to previous screen\nSelection:");
                

                switch (command)
                {
                    case (1):
                        ParkCampgroundScreen(currentWorkingPark);
                        break;

                    case (2):
                        ParkWideAvailabilitySearch(parkID);//Search reservations by park
                        break;

                    case (3):
                        Display30DaysReservation(parkID);
                        break;

                    case (4):
                        return;

                    default:
                        Console.WriteLine("Please enter a valid number from the menu.  Press Enter to try again.");
                        Console.ReadLine();
                        break;
                }
            }
        }

        public void ParkCampgroundScreen(Park currentWorkingPark)
        {
            DisplayCampgrounds(currentWorkingPark);

            while (true)
            {
                int command;

                command = CLIHelper.GetInteger("\nSelect a Command\n1) Search for available reservation\n2) Return to previous screen\nSelection:");

                switch (command)
                {
                    case (1):
                        CampgroundReservationScreen(currentWorkingPark);
                        break;

                    case (2):
                        return;

                    default:
                        Console.WriteLine("Please enter a valid number from the menu. Press Enter to try again.");
                        Console.ReadLine();
                        break;
                }
            }
        }

        //TODO: Search reservations by park method
        public void ParkWideAvailabilitySearch(int parkID)
        {
            IList<Campground> campgroundList = new List<Campground>();
            List<Site> masterSiteList = new List<Site>();
            CampgroundDAL campgroundDAL = new CampgroundDAL(DatabaseConnection);
            campgroundList = campgroundDAL.GetCampgroundList(parkID);
            SiteDAL siteDAL = new SiteDAL(DatabaseConnection);
            string reqFromDate;
            string reqToDate;
            bool isFirstTry = true;
            do
            {
                if (!isFirstTry)
                {
                    Console.WriteLine("\nThere are no open campsites in the selected timeframe for that park.\nTry an alternative date range.");
                }

                reqFromDate = CLIHelper.GetDateTime("What is the arrival date? (MM/DD/YYYY):");
                reqToDate = CLIHelper.GetDateTime("What is the departure date? (MM/DD/YYYY):");

                foreach (Campground campground in campgroundList)
                {
                    IList<Site> campgroundSiteList = new List<Site>();
                    campgroundSiteList = siteDAL.GetUnreservedCampsites(reqFromDate, reqToDate, campground.Campground_id);
                    masterSiteList.AddRange(campgroundSiteList);
                }

                isFirstTry = false;
            } while (masterSiteList.Count == 0);

            int lengthOfStay = CLIHelper.GetLengthOfStay(reqFromDate, reqToDate);

            Console.WriteLine("Results Matching Your Search Criteria\nCampground  Site No.  Max Occup.  Accssible?  Max RV Length  Utility  Cost");
            Console.WriteLine("Header");
            foreach (Site site in masterSiteList)
            {
                Console.WriteLine($"{site.CampgroundName}  " + site.ToString(lengthOfStay));
            }
        }

        public void Display30DaysReservation(int parkID)
        {
            ReservationDAL reservationDAL = new ReservationDAL(DatabaseConnection);
            IList<Reservation> reservationList = new List<Reservation>();
            reservationList = reservationDAL.Get30DayReservations(parkID);
            Console.Clear();
            Console.WriteLine("Arrival Date  Departure Date  Reservation ID  Name  Campsite #  Campground  Date Booked\n");
            foreach (Reservation reservation in reservationList)
            {
                Console.WriteLine(reservation.ToString());
            }
            Console.WriteLine("\nPress Enter to return to the park information menu.");
            Console.ReadLine();

        }

        public void CampgroundReservationScreen(Park currentWorkingPark)
        {
            DisplayCampgrounds(currentWorkingPark);

            SiteDAL siteDal = new SiteDAL(DatabaseConnection);
            IList<Site> unreservedSites = new List<Site>();
            string reqFromDate;
            string reqToDate;
            bool isFirstTry = true;
            do
            {

                if (!isFirstTry)
                {
                    Console.WriteLine("\nThere are no open campsites in the selected timeframe for that campground.\nTry an alternative date range.");
                }

                int campgroundNum = CLIHelper.GetInteger("\nWhich campground (enter 0 to cancel)?");
                if (campgroundNum == 0)
                {
                    return;
                }
                reqFromDate = CLIHelper.GetDateTime("What is the arrival date? (MM/DD/YYYY):");
                reqToDate = CLIHelper.GetDateTime("What is the departure date? (MM/DD/YYYY):");

                unreservedSites = siteDal.GetUnreservedCampsites(reqFromDate, reqToDate, campgroundNum);

                isFirstTry = false;

            } while (unreservedSites.Count == 0);

            int lengthOfStay = CLIHelper.GetLengthOfStay(reqFromDate, reqToDate);

            Console.WriteLine("Results Matching Your Search Criteria\nSite No.  Max Occup.  Accssible?  Max RV Length  Utility  Cost");
            foreach (Site site in unreservedSites)
            {
                Console.WriteLine(site.ToString(lengthOfStay));
            }

            ReservationDAL reservationDAL = new ReservationDAL(DatabaseConnection);
            while (true)
            {
                int campsiteChoice = CLIHelper.GetInteger("\nWhich campsite(enter 0 to cancel)?");
                if (campsiteChoice == 0)
                {
                    return;
                }

                foreach (Site site in unreservedSites)
                {
                    if (campsiteChoice == site.SiteID)
                    {
                        string reservationName = CLIHelper.GetString("What name should the reservation be made under?");
                        int reservationID = reservationDAL.MakeReservation(reqFromDate, reqToDate, campsiteChoice, reservationName);
                        Console.WriteLine($"\nThe reservation has been made and the confirmation id is {reservationID}.\nPress Enter to continue.");
                        Console.ReadLine();
                        return;
                    }

                }

                Console.WriteLine("Please make a valid selection from the list.");

            }

        }

        public void DisplayCampgrounds(Park currentWorkingPark)
        {
            CampgroundDAL campgroundDAL = new CampgroundDAL(DatabaseConnection);
            IList<Campground> campgroundList = campgroundDAL.GetCampgroundList(currentWorkingPark.ParkID);
            Console.Clear();
            Console.WriteLine("Park campgrounds");
            Console.WriteLine($"{currentWorkingPark.Name} National Park");       
            foreach (Campground campground in campgroundList)                   
            {                                                                   
                Console.WriteLine(campground.ToString());                    
            }
        }
    }
}
