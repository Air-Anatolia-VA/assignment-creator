using Air_Anatolia_VA_Assignment_Creator.Class;
using System.IO;
using System.Reflection;

namespace Air_Anatolia_VA_Assignment_Creator
{
    /*
     * TO-DO
     * fix empty input from user because humans?
     */
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
                var assembly = Assembly.GetExecutingAssembly();
                var resourceName = "Air_Anatolia_VA_Assignment_Creator.data.schedules.csv";

                using (Stream stream = assembly.GetManifestResourceStream(resourceName))
                using (StreamReader reader = new StreamReader(stream))
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
                            flighttype = parts[13],
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

            int numberOfFlightsToGenerate = 0;
            do
            {
                Console.WriteLine("\nPlease speciify how many random flight you would like to generate (1-10):");
                
                string input = Console.ReadLine();
                
                if (int.TryParse(input, out _))
                {
                    numberOfFlightsToGenerate = Convert.ToInt32(input);
                }
                else
                {
                    continue;
                }
            } while (numberOfFlightsToGenerate < 1 || numberOfFlightsToGenerate > 10);
            

            Console.WriteLine("\n\nYou have 2 options, either you can select the length of the flight or select from 3 options.\n0 = Specify exact time\n1 = Select from time range");
            int flightTimeTypeSelection = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("\n\nWrite the ICAO of the departure airport, it should be 4 letters:");

            string icaoSelection;

            do
            {
                icaoSelection = Console.ReadLine().ToUpper();
            } while (icaoSelection == null || icaoSelection.Length != 4 || icaoSelection.All(char.IsDigit));

            string cargoSelection;

            do
            {
                Console.WriteLine("\n\nAre you looking for Passenger flight or Cargo flight? Selections available;\nP = Passenger\nC = Cargo");
                cargoSelection = Console.ReadLine().ToUpper();
            } while (!(cargoSelection == null || cargoSelection == "P" || cargoSelection == "C"));

            int domesticOrInternational = 1;

            if (icaoSelection.StartsWith("LT"))
            {
                Console.WriteLine("\n\nSelect one of the following:\n0 = Domestic\n1 = International\n2 = Mix of Domestic and International (REALISTIC)");
                domesticOrInternational = Convert.ToInt32(Console.ReadLine());
            }

            int flightTimeSelection = 0;

            if (flightTimeTypeSelection == 0)
            {
                Console.WriteLine("\nSelect the flight time by writing the corresponding number:\n0 = 0-1 hours\n1 = 1-2 hours\n2 = 2-3 hours\n3 = 3-4 hours\n4 = 4-5 hours\n5 = 5-6 hours\n6 = 6-7 hours\n7 = 7-8 hours\n8 = 8-9 hours\n9 = 9-10 hours\n10 = 10-11 hours\n11 = 11-12 hours\n12 = 12-13 hours\n13 = 13-14 hours\n14 = 14+ hours");
                flightTimeSelection = Convert.ToInt32(Console.ReadLine()) * 100;
            }
            else if (flightTimeTypeSelection == 1)
            {
                if (domesticOrInternational == 0)
                {
                    do
                    {
                        Console.WriteLine("\n\n0 = short domestic flight (0-1 hours)\n1 = medium domestic flight (1-2 hours)\n2 = long domestic flight (2-3 hours)\n3 = mix of short + medium flights");
                        flightTimeSelection = Convert.ToInt32(Console.ReadLine());
                    } while (flightTimeSelection < 0 || flightTimeSelection > 3);
                }
                else if (domesticOrInternational == 1 || domesticOrInternational == 2)
                {
                    do
                    {
                        Console.WriteLine("\n\n0 = short flight (0-3 hours)\n1 = medium flight (3-6 hours)\n2 = long flight (6-13 hours)\n3 = ultra long flight (13+ hours)\n4 = mix of short + medium flights");
                        flightTimeSelection = Convert.ToInt32(Console.ReadLine());
                    } while (flightTimeSelection < 0 || flightTimeSelection > 4);  
                }
            }


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
                            if (domesticOrInternational == 0 && selectedFlight.arricao.StartsWith("LT") && selectedFlight.flighttype == cargoSelection)
                            {
                                if (flightTimeTypeSelection == 0)
                                {
                                    if (selectedFlight.flighttime >= flightTimeSelection && selectedFlight.flighttime <= flightTimeSelection + 100)
                                    {
                                        flights.Add(selectedFlight);
                                    }
                                }
                                else if (flightTimeTypeSelection == 1)
                                {
                                    switch (flightTimeSelection)
                                    {
                                        case 0:
                                            if (selectedFlight.flighttime >= 0 && selectedFlight.flighttime < 100)
                                            {
                                                flights.Add(selectedFlight);
                                            }
                                            break;
                                        case 1:
                                            if (selectedFlight.flighttime >= 100 && selectedFlight.flighttime < 200)
                                            {
                                                flights.Add(selectedFlight);
                                            }
                                            break;
                                        case 2:
                                            if (selectedFlight.flighttime >= 200 && selectedFlight.flighttime < 300)
                                            {
                                                flights.Add(selectedFlight);
                                            }
                                            break;
                                        case 3:
                                            if ((selectedFlight.flighttime >= 0 && selectedFlight.flighttime < 100) || (selectedFlight.flighttime >= 100 && selectedFlight.flighttime < 200))
                                            {
                                                flights.Add(selectedFlight);
                                            }
                                            break;
                                        default:
                                            break;
                                    }
                                }
                            }
                            else if (domesticOrInternational == 1 && !selectedFlight.arricao.StartsWith("LT") && selectedFlight.flighttype == cargoSelection)
                            {
                                if (flightTimeTypeSelection == 0)
                                {
                                    if (selectedFlight.flighttime >= flightTimeSelection && selectedFlight.flighttime < flightTimeSelection + 100)
                                    {
                                        flights.Add(selectedFlight);
                                    }
                                }
                                else if (flightTimeTypeSelection == 1)
                                {
                                    switch (flightTimeSelection)
                                    {
                                        case 0:
                                            if (selectedFlight.flighttime >= 0 && selectedFlight.flighttime < 300)
                                            {
                                                flights.Add(selectedFlight);
                                            }
                                            break;
                                        case 1:
                                            if (selectedFlight.flighttime >= 300 && selectedFlight.flighttime < 600)
                                            {
                                                flights.Add(selectedFlight);
                                            }
                                            break;
                                        case 2:
                                            if (selectedFlight.flighttime >= 600 && selectedFlight.flighttime < 1300)
                                            {
                                                flights.Add(selectedFlight);
                                            }
                                            break;
                                        case 3:
                                            if (selectedFlight.flighttime >= 1300)
                                            {
                                                flights.Add(selectedFlight);
                                            }
                                            break;
                                        case 4:
                                            if ((selectedFlight.flighttime >= 0 && selectedFlight.flighttime < 300) || (selectedFlight.flighttime >= 300 && selectedFlight.flighttime < 600))
                                            {
                                                flights.Add(selectedFlight);
                                            }
                                            break;
                                        default:
                                            break;
                                    }
                                }
                            }
                            else if (flightTimeTypeSelection == 2 && selectedFlight.flighttype == cargoSelection)
                            {
                                if (flightTimeTypeSelection == 0)
                                {
                                    if (selectedFlight.flighttime >= flightTimeSelection && selectedFlight.flighttime < flightTimeSelection + 100)
                                    {
                                        flights.Add(selectedFlight);
                                    }
                                }
                                else if (flightTimeTypeSelection == 1)
                                {
                                    switch (flightTimeSelection)
                                    {
                                        case 0:
                                            if (selectedFlight.flighttime >= 0 && selectedFlight.flighttime < 300)
                                            {
                                                flights.Add(selectedFlight);
                                            }
                                            break;
                                        case 1:
                                            if (selectedFlight.flighttime >= 300 && selectedFlight.flighttime < 600)
                                            {
                                                flights.Add(selectedFlight);
                                            }
                                            break;
                                        case 2:
                                            if (selectedFlight.flighttime >= 600 && selectedFlight.flighttime < 1300)
                                            {
                                                flights.Add(selectedFlight);
                                            }
                                            break;
                                        case 3:
                                            if (selectedFlight.flighttime >= 1300)
                                            {
                                                flights.Add(selectedFlight);
                                            }
                                            break;
                                        case 4:
                                            if ((selectedFlight.flighttime >= 0 && selectedFlight.flighttime < 300) || (selectedFlight.flighttime >= 300 && selectedFlight.flighttime < 600))
                                            {
                                                flights.Add(selectedFlight);
                                            }
                                            break;
                                        default:
                                            break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (flights.Count > 0)
            {
                Random rnd = new Random();

                if (numberOfFlightsToGenerate == 1)
                {
                    Console.WriteLine("\n\nHere is the flight assignment:\n");
                    int assignedFlight = rnd.Next(0, flights.Count);

                    string flightTime = Convert.ToString(flights[assignedFlight].flighttime);
                    if (Convert.ToInt32(flights[assignedFlight].flighttime) < 100)
                    {
                        flightTime = "00:" + Convert.ToInt32(flights[assignedFlight].flighttime).ToString();
                    }
                    else if (Convert.ToInt32(flights[assignedFlight].flighttime) < 1000)
                    {
                        flightTime = "0" + Convert.ToInt32(flights[assignedFlight].flighttime).ToString()[0] + ":" + Convert.ToInt32(flights[assignedFlight].flighttime).ToString().Substring(1);
                    }
                    else
                    {
                        flightTime = flightTime[0] + flightTime[1] + ":" + flightTime[2] + flightTime[3];
                    }

                    Console.WriteLine(flights[assignedFlight].code + " " + flights[assignedFlight].flightnum + " | " + flights[assignedFlight].depicao.ToUpper() + " - " + flights[assignedFlight].arricao.ToUpper() + " | Dep Time: " + flights[assignedFlight].deptime + ", Arrival Time: " + flights[assignedFlight].arrtime + "z | " + flightTime);
                }
                else
                {
                    Console.WriteLine("\n\nHere are the flight assignments:\n");
                    int totalFlights = flights.Count < numberOfFlightsToGenerate ? flights.Count : numberOfFlightsToGenerate;
                    for (int i = 0; i < totalFlights; i++)
                    {
                        int assignedFlight = rnd.Next(0, flights.Count);
                        string flightTime = Convert.ToInt32(flights[assignedFlight].flighttime).ToString();
                        if (Convert.ToInt32(flights[assignedFlight].flighttime) < 100)
                        {
                            flightTime = "00" + Convert.ToInt32(flights[assignedFlight].flighttime).ToString();
                        }
                        else if (Convert.ToInt32(flights[assignedFlight].flighttime) < 1000)
                        {
                            flightTime = "0" + Convert.ToInt32(flights[assignedFlight].flighttime).ToString()[0] + ":" + Convert.ToInt32(flights[assignedFlight].flighttime).ToString().Substring(1);
                        }
                        else
                        {
                            flightTime = flightTime[..2] + ":" + flightTime.Substring(2);
                        }

                        Console.WriteLine(flights[assignedFlight].code + " " + flights[assignedFlight].flightnum + " | " + flights[assignedFlight].depicao.ToUpper() + " - " + flights[assignedFlight].arricao.ToUpper() + " | Dep Time: " + flights[assignedFlight].deptime + "z, Arrival Time: " + flights[assignedFlight].arrtime + "z | " + flightTime);
                    }
                }
            }
            else
            {
                Console.WriteLine("\nNo flight could be found with the requested paramteres.\n");
            }

            Console.WriteLine("Press enter key to exit");
            string exit = Console.ReadLine();
        }
    }
}
