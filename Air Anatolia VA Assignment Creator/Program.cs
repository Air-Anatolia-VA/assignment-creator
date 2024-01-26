﻿using Air_Anatolia_VA_Assignment_Creator.Class;

namespace Air_Anatolia_VA_Assignment_Creator
{
    internal class Program
    {
        public static int[] ConvertStringToDaysOfWeekArray(string input)
        {
            int[] days = new int[input.Length];
            for (int i = 0; i < input.Length; i++)
            {
                days[i] = (input[i] - '0');
            }
            return days;
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Air Anatolia VA Assignment Creator. Please follow the instructions clearly.\nFirst the program will read the schedule data file then will ask questions.");

            Dictionary<string, Flight> flightList = new Dictionary<string, Flight>();

            try
            {
                using (StreamReader reader = new StreamReader("schedules.csv"))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] parts = line.Split(',');
                        flightList.Add(parts[1], (new Flight
                        {
                            code = parts[0],
                            flightnum = parts[1],
                            depicao = parts[2],
                            arricao = parts[3],
                            route = parts[4],
                            aircraft = parts[5],
                            distance = Double.Parse(parts[7]),
                            deptime = parts[8],
                            arrtime = parts[9],
                            flighttime = Double.Parse(parts[10]) * 100,
                            flighttype = Char.Parse(parts[13]),
                            daysofweek = ConvertStringToDaysOfWeekArray(parts[14]),
                        }));
                        ;
                    }

                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error while reading the file!, error is:\n" + e.ToString());
                throw;
            }

            Console.WriteLine("Write the ICAO of the departure airport, it should be 4 letters:\n");
            string icaoSelection;
            do
            {
                icaoSelection = Console.ReadLine().ToUpper();
            } while (icaoSelection == null || icaoSelection.Length != 4 || icaoSelection.All(char.IsDigit));

            int domesticOrInternational = 0;
            if (icaoSelection.StartsWith("LT"))
            {
                Console.WriteLine("Select one of the following:\nDomestic = 0\nInternational = 1\n");
                domesticOrInternational = Convert.ToInt32(Console.ReadLine());
            }

            Console.WriteLine("Select the flight time by writing the corresponding number:\n0-1 hours = 0\n1-2 hours = 1\n 2-3 hours = 2\n3-4 hours = 3\n4-5 hours = 4\n5-6 hours = 5\n6-7 hours = 6\n7-8 hours = 7\n8-9 hours = 8\n9-10 hours = 9\n10-11 hours = 10\n11-12 hours = 11\n12-13 hours = 12\n13-14 hours = 13\n14+ hours = 14\n");
            string input = Console.ReadLine();
            int flightTimeSelection = Convert.ToInt32(input) * 100;

            List<Flight> flights = new List<Flight>();
            int wk = (int)DateTime.Now.DayOfWeek;

            foreach (var selectedFlight in flightList.Values)
            {
                if (selectedFlight != null)
                {
                    if (Array.IndexOf(selectedFlight.daysofweek, wk) != -1)
                    {
                        if (selectedFlight.depicao.StartsWith(icaoSelection))
                        {
                            if (domesticOrInternational == 0 && selectedFlight.arricao.StartsWith("LT"))
                            {
                                if (selectedFlight.flighttime >= flightTimeSelection && selectedFlight.flighttime <= flightTimeSelection + 100)
                                {
                                    flights.Add(selectedFlight);
                                }
                            }
                            else if (flightTimeSelection == 14 && selectedFlight.flighttime >= 1400)
                            {
                                flights.Add(selectedFlight);
                            }
                            else if (selectedFlight.flighttime >= flightTimeSelection && selectedFlight.flighttime <= (flightTimeSelection + 100))
                            {
                                flights.Add(selectedFlight);
                            }
                        }
                    }
                }
            }

            Console.WriteLine("\n\nHere is the flight assignment:\n");

            Random rnd = new Random();
            int assignedFlight= rnd.Next(0, flights.Count);

            Console.WriteLine(flights[assignedFlight].code + " " + flights[assignedFlight].flightnum + " | " + flights[assignedFlight].depicao.ToUpper() + " - " + flights[assignedFlight].arricao.ToUpper() + " | Dep Time in XXXX UTC format:" + flights[assignedFlight].deptime + ", Arrival Time in XXXX UTC format: " + flights[assignedFlight].arrtime + " | " + flights[assignedFlight].flighttime);
        }
    }
}
