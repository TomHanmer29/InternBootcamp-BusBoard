using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BusBoard
{
    class Program
    {
        const string appId = "a5babc51";
        private const string appKey = "f057beb130e643c45171f1ae19b3e4fd";
        static HttpClient client = new HttpClient();
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to BusBoard!");
            while (true)
            {
                PrintBusData(RetrieveData<StopData>("https://transportapi.com/v3/uk/bus/stop/" + BusStopEntry() + "/live.json?&app_id="+appId+"&app_key="+appKey+"&group=no&limit=5").Result);
            }
        }

        private static string BusStopEntry()
        {
            Console.WriteLine("Enter bus stop code:");
            return Console.ReadLine();
        }

        private static async Task<dataType> RetrieveData<dataType>(string address)
        {
            string result="";
            try
            {
                result = await client.GetStringAsync(address);
            }
            catch(HttpRequestException)
            {
                Console.WriteLine("You've entered an invalid input");
            }
            return JsonConvert.DeserializeObject<dataType>(result);
        }

        private static void PrintBusData(StopData stopResult)
        {
            if (stopResult != null)
            {
                foreach (var bus in stopResult.departures["all"])
                {
                    var busRoute = new BusRoute();
                    string routeString = "";
                    if (bus.id != null)
                    {
                        busRoute = RetrieveData<BusRoute>(bus.id).Result;
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
