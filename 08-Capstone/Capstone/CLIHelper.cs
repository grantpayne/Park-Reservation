using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone
{
    public class CLIHelper
    {
        public static int GetInteger(string message)
        {
            string userInput = String.Empty;
            int intValue = 0;
            int numberOfAttempts = 0;

            do
            {
                if (numberOfAttempts > 0)
                {
                    Console.WriteLine("Invalid input format. Please try again.\n");
                }

                Console.Write(message + " ");
                userInput = Console.ReadLine();
                numberOfAttempts++;
            }
            while (!int.TryParse(userInput, out intValue));

            return intValue;

        }

        public static bool GetBool(string message)
        {
            string userInput = String.Empty;
            bool boolValue = false;
            int numberOfAttempts = 0;

            do
            {
                if (numberOfAttempts > 0)
                {
                    Console.WriteLine("Invalid input format. Please try again.\n");
                }

                Console.Write(message + " ");
                userInput = Console.ReadLine();
                numberOfAttempts++;
            }
            while (!bool.TryParse(userInput, out boolValue));

            return boolValue;
        }

        public static string GetString(string message)
        {
            string userInput = String.Empty;
            int numberOfAttempts = 0;

            do
            {
                if (numberOfAttempts > 0)
                {
                    Console.WriteLine("You must make a selection to continue.  Please try again.\n");
                }

                Console.Write(message + " ");
                userInput = Console.ReadLine();
                numberOfAttempts++;
            }
            while (String.IsNullOrEmpty(userInput));

            return userInput;
        }

        public static string GetDateTime(string message)
        {
            string userInput = String.Empty;
            int numberOfAttempts = 0;
            DateTime sqlDateTime;
            bool isParsed = true;
            bool isNotInPast = true;

            do
            {

                if (numberOfAttempts > 0 && !isParsed)
                {
                    Console.WriteLine("Invalid input format. Please try again.\n");

                }
                if (!isNotInPast && isParsed)
                {
                    Console.WriteLine("Please enter a date that is in the future.");
                }

                isParsed = false;
                isNotInPast = false;

                Console.Write(message + " ");
                userInput = Console.ReadLine();
                numberOfAttempts++;
                isParsed = DateTime.TryParse(userInput, out sqlDateTime);
                isNotInPast = (sqlDateTime > DateTime.Now);

            } while (!isParsed || !isNotInPast);

            string sqlDateString = sqlDateTime.ToString("yyyy-MM-dd");

            return sqlDateString;
        }

        public static int ExtractMonth(string input)
        {
            string result = "";
            string[] resultArray = input.Split("-");
            result = resultArray[1];
            return int.Parse(result);
        }

        public static int GetLengthOfStay(string reqFromDate, string reqToDate)
        {
            DateTime fromDate = DateTime.Parse(reqFromDate);
            DateTime toDate = DateTime.Parse(reqToDate);
            TimeSpan lengthOfStay = toDate - fromDate;
            return lengthOfStay.Days;
        }
    }
}
