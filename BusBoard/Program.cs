using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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
                List<BusStopInfo> busStopList = apiRequester.RequestAndDeserialize<BusStopData>("https://transportapi.com/v3/uk/places.json?&app_id="+appId+"&app_key="+appKey+"&lat="+postcodeData.result.latitude+"&lon="+postcodeData.result.longitude+"&type=bus_stop").Result.member;
                busStopList = busStopList.OrderBy(o => o.distance).Take(2).ToList();
                foreach (var busStop in busStopList)
                {
                    Console.WriteLine(busStop.name);
                    PrintBusData(apiRequester.RequestAndDeserialize<StopData>("https://transportapi.com/v3/uk/bus/stop/" +busStop.atcocode+ "/live.json?&app_id="+appId+"&app_key="+appKey+"&group=no&limit=5").Result);
                }
            }
        }

        private static string PostcodeEntry()
        {
            Console.WriteLine("Enter your postcode:");
            return Console.ReadLine();
        }

        private static void PrintBusData(StopData stopResult)
        {
            if (stopResult != null && stopResult.departures.ContainsKey("all"))
            {
                foreach (var bus in stopResult.departures["all"])
                {
                    var busRoute = new BusRoute();
                    string routeString = "";
                    if (bus.id != null)
                    {
                        busRoute = apiRequester.RequestAndDeserialize<BusRoute>(bus.id).Result;
                        routeString = string.Join(" -> ", busRoute.stops.Select(x => x.name));
                    }
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
