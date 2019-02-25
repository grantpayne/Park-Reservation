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

                string validSelectionError = "Please make a valid selection from the menu. Press Enter to try again.";

                string quitPrompt = $"{Command_Quit}) Quit\n";

                string selectionPrompt = "Selection:";

                int commandIfNum;

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
                Console.WriteLine(quitPrompt);

                string command = CLIHelper.GetString(selectionPrompt).ToUpper();

                if (command == Command_Quit)
                {
                    return;
                }
                else if (!int.TryParse(command, out commandIfNum))
                {
                    Console.WriteLine(validSelectionError);
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
                        Console.WriteLine(validSelectionError);
                        Console.ReadLine();
                    }
                }
            }
        }

        public void ParkInfoScreen(int parkID)
        {
            while (true)
            {
                string header = "";

                string selectionMenu = "Select a Command\n----------------\n1) View campgrounds\n2) Search for reservation\n3) View all reservations for the next 30 days\n4) Return to previous screen\n";

                string selectionPrompt = "Selection:";

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

                Console.WriteLine(selectionMenu);
                command = CLIHelper.GetInteger(selectionPrompt);


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
                string selectionMenu = "\nSelect a Command\n----------------\n1) Search for available reservation\n2) Return to previous screen\n";

                string selectionPrompt = "Selection:";

                string validSelectionError = "Please enter a valid number from the menu. Press Enter to try again.";

                int command;

                Console.WriteLine(selectionMenu);
                command = CLIHelper.GetInteger(selectionPrompt);

                switch (command)
                {
                    case (1):
                        CampgroundReservationScreen(currentWorkingPark);
                        break;

                    case (2):
                        return;

                    default:
                        Console.WriteLine(validSelectionError);
                        Console.ReadLine();
                        break;
                }
            }
        }

        public void ParkWideAvailabilitySearch(int parkID)
        {
            string reqFromDate;

            string reqToDate;

            string reservationHeader = $"===================Reservation Confirmation==================\n";

            bool isFirstTry = true;

            IList<Campground> campgroundList = new List<Campground>();
            List<Site> masterSiteList = new List<Site>();
            CampgroundDAL campgroundDAL = new CampgroundDAL(DatabaseConnection);
            campgroundList = campgroundDAL.GetCampgroundList(parkID);

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


                /*----------------------------------------------------Advanced Search------------------------------------------------------*/
                bool advancedSearch = false;
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
                /*---------------------------------------------------End of Advanced Search-------------------------------------------------*/


                foreach (Campground campground in campgroundList)
                {
                    IList<Site> campgroundSiteList = new List<Site>();
                    SiteDAL siteDAL = new SiteDAL(DatabaseConnection);

                    campgroundSiteList = siteDAL.GetUnreservedCampsites(reqFromDate, reqToDate, campground.Campground_id, minOccupancy, accessible, maxRvLength, utilities);

                    foreach (Site site in campgroundSiteList)
                    {
                        site.CampgroundName = campground.Name;
                    }
                    masterSiteList.AddRange(campgroundSiteList); //Uses the SQL query on all campgrounds in a park to return top 5 campsites in all campgrounds
                }

                isFirstTry = false;
            } while (masterSiteList.Count == 0);

            int lengthOfStay = CLIHelper.GetLengthOfStay(reqFromDate, reqToDate);

            Console.Clear();
            Console.WriteLine($"Results Matching Your Search Criteria\n\n" + "Campground".PadRight(22) + "Site #".PadRight(10) + "Max Occup.".PadRight(13) + "Accessible?".PadRight(12) + "RV Size".PadRight(12) + "Utility".PadRight(15) + "Cost\n");
            foreach (Site site in masterSiteList)
            {
                Console.WriteLine(site.ToString(lengthOfStay, true));
            }

            ReservationDAL reservationDAL = new ReservationDAL(DatabaseConnection);

            while (true)
            {
                int campsiteChoice = CLIHelper.GetInteger("\nPlease select your desired Site # from the list (enter 0 to cancel)?");
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
                        Console.Clear();
                        Console.SetCursorPosition((Console.WindowWidth - reservationHeader.Length) / 2, Console.CursorTop);
                        Console.WriteLine(reservationHeader);
                        Console.WriteLine($"\nThe reservation has been made and the confirmation id is {reservationID}.\nPress Enter to continue.");
                        Console.ReadLine();
                        return;
                    }

                }

                Console.WriteLine("Please make a valid campsite ID selection from the list.");

            }

        } //Searches availability for every campground in a given park

        public void Display30DaysReservation(int parkID) //Takes a park ID and displays all reservations for the next 30 days starting at current time
        {

            string headerString = "====================== Reservations in Next 30 Days ======================\n";

            string searchResultString = " Arrival".PadRight(12) + "Departure".PadRight(14) + "Res.ID".PadRight(20) + "Name".PadRight(16) + "Campsite #".PadRight(13) + "Campground".PadRight(26) + "Date Booked\n";

            string returnString = "\nPress Enter to return to the park information menu.";

            string noReservationString = "There are currently no reservations booked for the next 30 days.";

            ReservationDAL reservationDAL = new ReservationDAL(DatabaseConnection);
            IList<Reservation> reservationList = new List<Reservation>();

            reservationList = reservationDAL.Get30DayReservations(parkID);

            Console.Clear();
            Console.SetCursorPosition((Console.WindowWidth - headerString.Length) / 2, Console.CursorTop); //Writes header string to center of console
            Console.WriteLine(headerString);
            Console.WriteLine(searchResultString);

            if (reservationList.Count == 0)
            {
                Console.WriteLine(noReservationString);
            }

            foreach (Reservation reservation in reservationList)
            {
                Console.WriteLine(reservation.ToString());
            }

            Console.WriteLine(returnString);
            Console.ReadLine();

        }

        public void CampgroundReservationScreen(Park currentWorkingPark)
        {

            string campgroundSelectionPrompt = "\nPlease select a 'Campground' ID from the list (enter 0 to cancel)?";

            string campsiteSelectionPrompt = "\nPlease select a 'Campsite ID' from the list (enter 0 to cancel)?";

            string enterValidIDError = "Please make a valid 'Campsite ID' selection from the list.";

            string arrivalDatePrompt = "What is the arrival date? (MM/DD/YYYY):";

            string departureDatePrompt = "What is the departure date? (MM/DD/YYYY):";

            string negativeDateRangeError = "Departure date must be after arrival date. Please try again.\n";

            string noResultsInRangeError = "\nThere are no open campsites in the selected timeframe for that campground.\nTry an alternative date range.";

            string reservationNamePrompt = "What name should the reservation be made under?";

            string searchResult = $"Results Matching Your Search Criteria\n\n" + "Site #".PadRight(9) + "Max Occup.".PadRight(11) + "Accessible?".PadRight(12) + "RV Size".PadRight(12) + "Utility".PadRight(15) + "Cost\n";

            string reqFromDate;             //User desired arrival date

            string reqToDate;               //User desired departure date

            bool isFirstTry = true;         //Used to track first run through campground selection process


            IList<Campground> campgroundList = DisplayCampgrounds(currentWorkingPark);
            List<int> campgroundIdList = new List<int>();

            foreach (Campground campground in campgroundList)
            {
                campgroundIdList.Add(campground.Campground_id);
            }

            IList<Site> unreservedSites = new List<Site>();

            do
            {
                bool negativeDateRangeAttempted = false;

                if (!isFirstTry)
                {
                    Console.WriteLine(noResultsInRangeError);
                }

                int campgroundNum;          //Stores DB value for campgroundID

                do
                {
                    campgroundNum = CLIHelper.GetInteger(campgroundSelectionPrompt); //CLIHelper deals with incorrect inputs, do while just waits for proper input or exit condition

                    if (campgroundNum == 0)
                    {
                        return;
                    }

                } while (!campgroundIdList.Contains(campgroundNum));

                reqFromDate = CLIHelper.GetDateTime(arrivalDatePrompt); //Arrival date is necessarry and CLIHelper handles errors, so no loop necessary

                do
                {
                    if (negativeDateRangeAttempted)
                    {
                        Console.WriteLine(negativeDateRangeError);
                    }

                    reqToDate = CLIHelper.GetDateTime(departureDatePrompt);

                    negativeDateRangeAttempted = DateTime.Parse(reqToDate) <= DateTime.Parse(reqFromDate);

                } while (negativeDateRangeAttempted);

                /*----------------------------------------------------Advanced Search------------------------------------------------------*/
                bool advancedSearch = false;  //TODO Any way to move this to its own method?
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
                /*---------------------------------------------------End of Advanced Search-------------------------------------------------*/

                SiteDAL siteDal = new SiteDAL(DatabaseConnection);
                unreservedSites = siteDal.GetUnreservedCampsites(reqFromDate, reqToDate, campgroundNum, minOccupancy, accessible, maxRvLength, utilities);

                isFirstTry = false;

            } while (unreservedSites.Count == 0);

            int lengthOfStay = CLIHelper.GetLengthOfStay(reqFromDate, reqToDate);

            Console.Clear();
            Console.WriteLine(searchResult);

            foreach (Site site in unreservedSites)
            {
                Console.WriteLine(site.ToString(lengthOfStay, false));
            }

            ReservationDAL reservationDAL = new ReservationDAL(DatabaseConnection);

            while (true)
            {
                int campsiteChoice = CLIHelper.GetInteger(campsiteSelectionPrompt);

                if (campsiteChoice == 0)
                {
                    return;
                }

                foreach (Site site in unreservedSites)
                {
                    if (campsiteChoice == site.SiteID)
                    {
                        string reservationHeader = $"===================Reservation Confirmation==================\n";

                        string reservationName = CLIHelper.GetString(reservationNamePrompt);
                        int reservationID = reservationDAL.MakeReservation(reqFromDate, reqToDate, campsiteChoice, reservationName);

                        Console.Clear();
                        Console.SetCursorPosition((Console.WindowWidth - reservationHeader.Length) / 2, Console.CursorTop);
                        Console.WriteLine(reservationHeader);
                        Console.WriteLine($"\nThe reservation has been made and the confirmation id is {reservationID}.\nPress Enter to continue."); //Can't move this yet
                        Console.ReadLine();
                        return;
                    }

                }

                Console.WriteLine(enterValidIDError);

            }

        } //Used to view open reservations and make reservations in the scope of a campsite at a campground

        public IList<Campground> DisplayCampgrounds(Park currentWorkingPark) //Displays a list of campgrounds within the currently selected park
        {
            string headerString = $"====================== {currentWorkingPark.Name} National Park Campgrounds ======================\n";

            string campgroundHeader = "Campground ID".PadRight(25) + "Campground Name".PadRight(40) + "Open Through".PadRight(25) + "Price per day\n";

            CampgroundDAL campgroundDAL = new CampgroundDAL(DatabaseConnection);
            IList<Campground> campgroundList = campgroundDAL.GetCampgroundList(currentWorkingPark.ParkID);

            Console.Clear();
            Console.SetCursorPosition((Console.WindowWidth - headerString.Length) / 2, Console.CursorTop); //Prints headerString to center of console
            Console.WriteLine(headerString);
            Console.WriteLine(campgroundHeader);

            foreach (Campground campground in campgroundList)
            {
                Console.WriteLine(campground.ToString());
            }

            return campgroundList;
        }
    }
}
