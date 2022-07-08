using BusBoard.Api;

namespace BusBoard.ConsoleApp
{
    class Program
    {
        static Api.BusBoard busBoard = new Api.BusBoard();
        
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to BusBoard!");
            while (true)
            {
                PostcodeData postcodeData = busBoard.PerformPostcodeLookup(PostcodeEntry());
                var busStopList = busBoard.GetBusStopsFromPostcode(postcodeData, numberOfStops:2);
                OutputBusTimetable(busStopList);
            }
        }

        
        private static void OutputBusTimetable(List<BusStop> busStopList)
        {
            foreach (var busStop in busStopList)
            {
                Console.WriteLine("\n" + busStop.name);
                PrintBusData(busBoard.GetDeparturesAtStop(busStop));
            }
        }

        private static string PostcodeEntry()
        {
            string postcode;
            do
            {
                Console.WriteLine("\nEnter your postcode:");
                postcode = Console.ReadLine();
            } while (!busBoard.ValidatePostcode(postcode));
            return postcode;

        }

        private static void PrintBusRoute(List<string> route)
        {
            Console.WriteLine(string.Join(" -> ", route));
        }

        private static void PrintBusData(List<BusData> busList)
        {
            Console.WriteLine("{0,-15}{1,-35}{2,-15}{3,-15}", "Line", "Destination", "Time", "Expected");
            Console.WriteLine("================================================================================");
            
            foreach (var bus in busList)
            {
                // Display all information for this bus from this stop
                Console.WriteLine("{0,-15}{1,-35}{2,-15}{3,-15}", bus.line, bus.direction, bus.aimed_departure_time,
                    bus.expected_departure_time);
                //PrintBusRoute(busBoard.GetRoute(bus));
            }
        }
    }
}

//0500CCITY436
