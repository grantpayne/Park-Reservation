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
        const string Command_SelectAcadia = "1";
        const string Command_Arches = "2";
        const string Command_Cuyahoga = "3";
        const string Command_Options = "4";
        const string Command_Quit = "q";

        public void RunCLI()
        {
            while (true)
            {
                Console.Clear();

                string command = CLIHelper.GetString("Select a park for further details\n1) Acadia\n2) Arches\n3) Cuyahoga Valley\n4) Additional Options\nQ) Quit\nSelection:");

                switch (command.ToLower())
                {
                    case Command_SelectAcadia:
                        ParkInfoScreen(1);
                        break;

                    case Command_Arches:
                        ParkInfoScreen(2);
                        break;

                    case Command_Cuyahoga:
                        ParkInfoScreen(3);
                        break;

                    case Command_Options:
                        break;

                    case Command_Quit:
                        return;

                    default:
                        Console.WriteLine("Please enter a valid selection from the menu. Press Enter to try again.");
                        Console.ReadLine();
                        break;
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
                    Console.WriteLine(park.ToString());
                    currentWorkingPark = park;
                    break;
                }
            }

            while (true)
            {
                int command;

                command = CLIHelper.GetInteger("Select a Command\n1) View campgrounds\n2) Search for reservation\n3) Return to previous screen\nSelection:");

                switch (command)
                {
                    case (1):
                        ParkCampgroundScreen(currentWorkingPark);
                        break;

                    case (2):
                        break;

                    case (3):
                        return;

                    default:
                        Console.WriteLine("Please enter a valid number from the menu.  Press Enter to try again.");
                        Console.ReadLine();
                        break;
                }
            }
        }

        public void ParkCampgroundScreen(Park currentWorkingPark) //THIS IS BROKE
        {
            CampgroundDAL campgroundDAL = new CampgroundDAL(DatabaseConnection);
            IList<Campground> campgroundList = campgroundDAL.GetCampgroundList(currentWorkingPark.ParkID);

            Console.Clear();
            Console.WriteLine("Park campgrounds");
            Console.WriteLine($"{currentWorkingPark.Name} National Park");  //Refactor into its own method
            foreach (Campground campground in campgroundList)
            {
                Console.WriteLine(campground.ToString());
            }

            while (true)
            {
                int command;

                command = CLIHelper.GetInteger("Select a Command\n1) Search for available reservation\n2) Return to previous screen\nSelection:");
           
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

        public void CampgroundReservationScreen(Park currentWorkingPark)
        {

            CampgroundDAL campgroundDAL = new CampgroundDAL(DatabaseConnection);
            IList<Campground> campgroundList = campgroundDAL.GetCampgroundList(currentWorkingPark.ParkID);
            Console.Clear();
            Console.WriteLine("Park campgrounds");
            Console.WriteLine($"{currentWorkingPark.Name} National Park");       //Refactor into its own method
            foreach (Campground campground in campgroundList)                   //Refactor into its own method
            {                                                                   //Refactor into its own method
                Console.WriteLine(campground.ToString());                    //Refactor into its own method
            }

            SiteDAL siteDal = new SiteDAL(DatabaseConnection);
            IList<Site> unreservedSites = new List<Site>();

            bool isFirstTry = true;
            do
            {
                
                if (!isFirstTry)
                {
                    Console.WriteLine("There are no open campsites in the selected timeframe for that campground.\nPlease Try again.\n");
                }

                int campgroundNum = CLIHelper.GetInteger("\nWhich campground (enter 0 to cancel)?");
                string reqFromDate = CLIHelper.GetDateTime("What is the arrival date? (MM/DD/YYYY):");
                string reqToDate = CLIHelper.GetDateTime("What is the departure date? (MM/DD/YYYY):");

                unreservedSites = siteDal.GetUnreservedCampsites(reqFromDate, reqToDate, campgroundNum);

                isFirstTry = false;

            } while (unreservedSites.Count == 0);

            Console.WriteLine("Results Matching Your Search Criteria\nSite No.  Max Occup.  Accssible?  Max RV Length  Utility  Cost");
            foreach (Site site in unreservedSites)
            {
                Console.WriteLine(site.ToString());
            }

            ReservationDAL reservationDAL = new ReservationDAL(DatabaseConnection);
        }
    }
}
