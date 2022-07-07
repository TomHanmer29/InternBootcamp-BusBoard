namespace BusBoard
{
    class Program
    {
        const string appId = "a5babc51";
        private const string appKey = "f057beb130e643c45171f1ae19b3e4fd";
        private static APIRequester apiRequester = new APIRequester();
        
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to BusBoard!");
            
            while (true)
            {
                PostcodeData postcodeData = apiRequester.RequestAndDeserialize<PostcodeData>("https://api.postcodes.io/postcodes/" + PostcodeEntry()).Result;
                var busStopList = GetBusStopsFromPostcode(postcodeData);
                foreach (var busStop in busStopList)
                {
                    Console.WriteLine(busStop.name);
                    PrintBusData(apiRequester.RequestAndDeserialize<BusStop>("https://transportapi.com/v3/uk/bus/stop/" +busStop.atcocode+ "/live.json?&app_id="+appId+"&app_key="+appKey+"&group=no&limit=5").Result);
                }
            }
        }

        private static string PostcodeEntry()
        {
            Console.WriteLine("Enter your postcode:");
            return Console.ReadLine();
        }

        private static List<BusStop> GetBusStopsFromPostcode(PostcodeData postcodeData)
        {
            List<BusStop> busStopList = apiRequester.RequestAndDeserialize<BusStopData>(
                    "https://transportapi.com/v3/uk/places.json?&app_id=" + appId +
                    "&app_key=" + appKey +
                    "&lat=" + postcodeData.result.latitude +
                    "&lon=" + postcodeData.result.longitude +
                    "&type=bus_stop")
                .Result.member;
            return busStopList.OrderBy(o => o.distance).Take(2).ToList();
        }

        private static void PrintBusData(BusStop busStopResult)
        {
            if (busStopResult != null && busStopResult.departures.ContainsKey("all"))
            {
                foreach (var bus in busStopResult.departures["all"])
                {
                    var routeString = "";
                    if (bus.id != null)
                    {
                        // Get route info for this bus
                        var busRoute = apiRequester.RequestAndDeserialize<BusRoute>(bus.id).Result;
                        routeString = string.Join(" -> ", busRoute.stops.Select(x => x.name));
                    }
                    // Display all information for this bus from this stop
                    Console.WriteLine(
                        $"Line: {bus.line}, Destination: {bus.direction}, Time: {bus.aimed_departure_time}, Expected: {bus.expected_departure_time}, Route: {routeString}");
                }
            }
            else
            {
                Console.WriteLine(":(");
            }
        }
    }
}

//0500CCITY436
