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
                string parkSelectPrompt = "====================== Select a Park for Further Details ======================\n";
                Console.Clear();
                Console.SetCursorPosition((Console.WindowWidth - parkSelectPrompt.Length) / 2, Console.CursorTop);
                Console.WriteLine(parkSelectPrompt);
                ParkDAL parkDAL = new ParkDAL(DatabaseConnection);
                IList<Park> parkList = new List<Park>();
                parkList = parkDAL.GetParkList();
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

        /*------------------------------------------Park Information Screen--------------------------------------------------*/
        public void ParkInfoScreen(int parkID)
        {
            while (true)
            {
                string header = "";
                Park currentWorkingPark = new Park();

                ParkDAL parkDAL = new ParkDAL(DatabaseConnection);
                IList<Park> parks = parkDAL.GetParkList();
                foreach (Park park in parks)
                {
                    if (park.ParkID == parkID)
                    {
                        currentWorkingPark = park;
                        Console.Clear();
                        header += $"====================== {currentWorkingPark.Name} National Park Information ======================";
                        Console.SetCursorPosition((Console.WindowWidth - header.Length) / 2, Console.CursorTop);
                        Console.WriteLine(header);
                        Console.WriteLine(park.ToString() + "\n");
                        break;
                    }
                }

                int command;

                Console.WriteLine("Select a Command\n----------------\n1) View campgrounds\n2) Search for reservation\n3) View all reservations for the next 30 days\n4) Return to previous screen\n");
                command = CLIHelper.GetInteger("Selection:");


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

                Console.WriteLine("\nSelect a Command\n----------------\n1) Search for available reservation\n2) Return to previous screen\n");
                command = CLIHelper.GetInteger("Selection:");

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

        public void ParkWideAvailabilitySearch(int parkID)
        {
            IList<Campground> campgroundList = new List<Campground>();
            List<Site> masterSiteList = new List<Site>();
            CampgroundDAL campgroundDAL = new CampgroundDAL(DatabaseConnection);
            campgroundList = campgroundDAL.GetCampgroundList(parkID);
            string reqFromDate;
            string reqToDate;


            bool isFirstTry = true;
            do
            {
                if (!isFirstTry)
                {
                    Console.WriteLine("\nThere are no open campsites in the selected timeframe for that park.\nTry an alternative date range.");
                }

                //TODO: consolidate getDateRange into CLI helper method that returns two string dates once validated
                reqFromDate = CLIHelper.GetDateTime("What is the arrival date? (MM/DD/YYYY):");

                bool negativeDateRangeAttempted = false;

                do
                {
                    if (negativeDateRangeAttempted)
                    {
                        Console.WriteLine("Departure date must be after arrival date. Please try again.\n");
                    }
                    reqToDate = CLIHelper.GetDateTime("What is the departure date? (MM/DD/YYYY):");
                    negativeDateRangeAttempted = DateTime.Parse(reqToDate) <= DateTime.Parse(reqFromDate);

                } while (negativeDateRangeAttempted);

                bool advancedSearch = false;
                //advanced search variables
                int minOccupancy = 0;
                bool accessible = false;
                int maxRvLength = 0;
                bool utilities = false;

                advancedSearch = CLIHelper.GetBool("Use Advanced Search (Y/N)?");

                if (advancedSearch)
                {
                    minOccupancy = CLIHelper.GetInteger("Minimum Occupancy (Enter 0 to skip):");
                    accessible = CLIHelper.GetBool("Do you require accessibility features (Y/N)?");
                    maxRvLength = CLIHelper.GetInteger("What is your RV size (Enter 0 to skip)?");
                    utilities = CLIHelper.GetBool("Do you require a utility hookup (Y/N)?");
                }



                foreach (Campground campground in campgroundList)
                {
                    IList<Site> campgroundSiteList = new List<Site>();
                    SiteDAL siteDAL = new SiteDAL(DatabaseConnection);
                    campgroundSiteList = siteDAL.GetUnreservedCampsites(reqFromDate, reqToDate, campground.Campground_id, minOccupancy, accessible, maxRvLength, utilities);
                    foreach (Site site in campgroundSiteList)
                    {
                        site.CampgroundName = campground.Name;
                    }
                    masterSiteList.AddRange(campgroundSiteList);
                }

                isFirstTry = false;
            } while (masterSiteList.Count == 0);

            int lengthOfStay = CLIHelper.GetLengthOfStay(reqFromDate, reqToDate);

            Console.Clear();
            Console.WriteLine($"Results Matching Your Search Criteria\n" + "Campground".PadRight(20) + "Site No.".PadRight(12) + "Max Occup." + "Accessible?" + "Max RV Length" + "Utility" + "Cost"); //TODO Finish this
            foreach (Site site in masterSiteList)
            {
                Console.WriteLine(site.ToString(lengthOfStay));
            }

            ReservationDAL reservationDAL = new ReservationDAL(DatabaseConnection);
            while (true)
            {
                int campsiteChoice = CLIHelper.GetInteger("\nPlease select a campsite ID from the list (enter 0 to cancel)?");
                if (campsiteChoice == 0)
                {
                    return;
                }

                foreach (Site site in masterSiteList)
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

                Console.WriteLine("Please make a valid campsite ID selection from the list.");

            }

        }

        public void Display30DaysReservation(int parkID)
        {
            string header = "====================== Reservations in Next 30 Days ======================\n";
            ReservationDAL reservationDAL = new ReservationDAL(DatabaseConnection);
            IList<Reservation> reservationList = new List<Reservation>();
            reservationList = reservationDAL.Get30DayReservations(parkID);
            Console.Clear();
            Console.SetCursorPosition((Console.WindowWidth - header.Length) / 2, Console.CursorTop);
            Console.WriteLine(header);
            Console.WriteLine(" Arrival".PadRight(12) + "Departure".PadRight(14) + "Res.ID".PadRight(20) + "Name".PadRight(16) + "Campsite #".PadRight(13) + "Campground".PadRight(26) + "Date Booked\n");

            if (reservationList.Count == 0)
            {
                Console.WriteLine("There are currently no reservations booked for the next 30 days.");
            }
            foreach (Reservation reservation in reservationList)
            {
                Console.WriteLine(reservation.ToString());
            }
            Console.WriteLine("\nPress Enter to return to the park information menu.");
            Console.ReadLine();

        }

        public void CampgroundReservationScreen(Park currentWorkingPark)
        {
            IList<Campground> campgroundList = DisplayCampgrounds(currentWorkingPark);
            List<int> campgroundIdList = new List<int>();
            foreach (Campground campground in campgroundList)
            {
                campgroundIdList.Add(campground.Campground_id);
            }

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

                int campgroundNum;

                do
                {
                    campgroundNum = CLIHelper.GetInteger("\nPlease select a campground ID from the list (enter 0 to cancel)?");
                    if (campgroundNum == 0)
                    {
                        return;
                    }
                } while (!campgroundIdList.Contains(campgroundNum));


                reqFromDate = CLIHelper.GetDateTime("What is the arrival date? (MM/DD/YYYY):");

                bool negativeDateRangeAttempted = false;

                do
                {
                    if (negativeDateRangeAttempted)
                    {
                        Console.WriteLine("Departure date must be after arrival date. Please try again.\n");
                    }
                    reqToDate = CLIHelper.GetDateTime("What is the departure date? (MM/DD/YYYY):");
                    negativeDateRangeAttempted = DateTime.Parse(reqToDate) <= DateTime.Parse(reqFromDate);

                } while (negativeDateRangeAttempted);

                bool advancedSearch = false;
                //advanced search variables
                int minOccupancy = 0;
                bool accessible = false;
                int maxRvLength = 0;
                bool utilities = false;

                advancedSearch = CLIHelper.GetBool("Use Advanced Search (Y/N)?");

                if (advancedSearch)
                {
                    minOccupancy = CLIHelper.GetInteger("Minimum Occupancy (Enter 0 to skip):");
                    accessible = CLIHelper.GetBool("Do you require accessibility features (Y/N)?");
                    maxRvLength = CLIHelper.GetInteger("What is your RV size (Enter 0 to skip)?");
                    utilities = CLIHelper.GetBool("Do you require a utility hookup (Y/N)?");
                }

                SiteDAL siteDal = new SiteDAL(DatabaseConnection);
                unreservedSites = siteDal.GetUnreservedCampsites(reqFromDate, reqToDate, campgroundNum, minOccupancy, accessible, maxRvLength, utilities);

                isFirstTry = false;

            } while (unreservedSites.Count == 0);

            int lengthOfStay = CLIHelper.GetLengthOfStay(reqFromDate, reqToDate);

            Console.Clear();
            Console.WriteLine($"Results Matching Your Search Criteria\n" + "Site No." + "Max Occup." + "Accessible?" + "Max RV Length" + "Utility" + "Cost"); //Finish this
            foreach (Site site in unreservedSites)
            {
                Console.WriteLine(site.ToString(lengthOfStay));
            }

            ReservationDAL reservationDAL = new ReservationDAL(DatabaseConnection);
            while (true)
            {
                int campsiteChoice = CLIHelper.GetInteger("\nPlease select a campsite ID from the list (enter 0 to cancel)?");
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

                Console.WriteLine("Please make a valid campsite ID selection from the list.");

            }

        }

        public IList<Campground> DisplayCampgrounds(Park currentWorkingPark)
        {
            string header = $"====================== {currentWorkingPark.Name} National Park Campgrounds ======================\n";
            string campgroundHeader = "Campground ID".PadRight(25) + "Campground Name".PadRight(40) + "Open Through".PadRight(25) + "Price per day\n";

            CampgroundDAL campgroundDAL = new CampgroundDAL(DatabaseConnection);
            IList<Campground> campgroundList = campgroundDAL.GetCampgroundList(currentWorkingPark.ParkID);

            Console.Clear();
            Console.SetCursorPosition((Console.WindowWidth - header.Length) / 2, Console.CursorTop);
            Console.WriteLine(header);
            Console.WriteLine(campgroundHeader);

            foreach (Campground campground in campgroundList)
            {
                Console.WriteLine(campground.ToString());
            }

            return campgroundList;
        }
    }
}
