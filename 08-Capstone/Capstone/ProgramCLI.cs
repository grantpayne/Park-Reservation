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
            bool done = false;

            Console.WriteLine("Select a park for further details\n");
            Console.WriteLine("1) Acadia");
            Console.WriteLine("2) Arches");
            Console.WriteLine("3) Cuyahoga Valley");
            Console.WriteLine("4) Additional Options");
            Console.WriteLine("Q) Quit");

            while (!done)
            {
                string command = Console.ReadLine();

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
                }
            }
        }


        public void ParkInfoScreen(int parkID)
        {
            bool done = false;

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

            Console.WriteLine("Select a Command\n");
            Console.WriteLine("1) View campgrounds");
            Console.WriteLine("2) Search for reservation");
            Console.WriteLine("3) Return to previous screen");

            while (!done)
            {
                int command;

                try
                {
                    command = int.Parse(Console.ReadLine());
                }
                catch (Exception)
                {
                    command = 0;
                }

                switch (command)
                {
                    case (1):
                        ParkCampgroundScreen(currentWorkingPark);
                        break;

                    case (2):
                        break;

                    case (3):
                        break;

                    default:
                        Console.WriteLine("Please enter a valid selection");
                        break;
                }
            }
        }

        public void ParkCampgroundScreen(Park currentWorkingPark) //THIS IS BROKE
        {
            bool done = false;
            CampgroundDAL campgroundDAL = new CampgroundDAL(DatabaseConnection);
            IList<Campground> campgroundList = campgroundDAL.GetCampgroundList(currentWorkingPark.ParkID);

            Console.Clear();
            Console.WriteLine("Park campgrounds");
            Console.WriteLine($"{currentWorkingPark.Name} National Park");  //Refactor into its own method
            foreach (Campground campground in campgroundList)
            {
                Console.WriteLine(campground.ToString());
            }

            Console.WriteLine("Select a Command");
            Console.WriteLine("1) Search for available reservation");
            Console.WriteLine("2) Return to previous screen");

            while (!done)
            {
                int command;

                try
                {
                    command = int.Parse(Console.ReadLine());
                }
                catch (Exception)
                {
                    command = 0;
                }

                switch (command)
                {
                    case (1):
                        CampgroundReservationScreen(currentWorkingPark);
                        break;

                    case (2):
                        done = true; //NO WORKING
                        return;

                    default:
                        Console.WriteLine("Please enter a valid selection");
                        break;
                }
            }
        }

        public void CampgroundReservationScreen(Park currentWorkingPark)
        {
            bool done = false;
            ReservationDAL reservationDAL = new ReservationDAL(DatabaseConnection);

            CampgroundDAL campgroundDAL = new CampgroundDAL(DatabaseConnection);
            IList<Campground> campgroundList = campgroundDAL.GetCampgroundList(currentWorkingPark.ParkID);
            Console.Clear();
            Console.WriteLine("Park campgrounds");
            Console.WriteLine($"{currentWorkingPark.Name} National Park");       //Refactor into its own method
            foreach (Campground campground in campgroundList)                   //Refactor into its own method
            {                                                                   //Refactor into its own method
                Console.WriteLine(campground.ToString());                    //Refactor into its own method
            }

            Console.WriteLine();
            Console.Write("\nWhich campground (enter 0 to cancel)? ");
            int campgroundNum = int.Parse(Console.ReadLine());


            do
            {
                Console.Write("\nWhich campground (enter 0 to cancel)? ");
                campgroundNum = int.Parse(Console.ReadLine());

                Console.Write("\nWhat is the arrival date? ");

                Console.Write("\nWhat is the departure date? ");
                
            } while (campgroundNum != 0);



        }
    }
}
