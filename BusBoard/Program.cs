using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BusBoard
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to BusBoard!");
            while (true)
            {
                PrintBusData(RetrieveBusData().Result);
            }
        }

        private static string BusStopEntry()
        {
            Console.WriteLine("Enter bus stop code:");
            return Console.ReadLine();
        }

        private static async Task<StopData> RetrieveBusData()
        {
            HttpClient client = new HttpClient();
            string result="";
            try
            {
                result = await client.GetStringAsync("https://transportapi.com/v3/uk/bus/stop/" + BusStopEntry() + "/live.json?&app_id=a5babc51&app_key=f057beb130e643c45171f1ae19b3e4fd&group=no&limit=5");
            }
            catch(HttpRequestException)
            {
                Console.WriteLine("You've entered an invalid stop");
            }
            return JsonConvert.DeserializeObject<StopData>(result);
        }

        private static void PrintBusData(StopData stopResult)
        {
            if (stopResult != null)
            {
                foreach (var bus in stopResult.departures["all"])
                {
                    Console.WriteLine(
                        $"Line: {bus.line}, Destination: {bus.direction}, Time: {bus.aimed_departure_time}, Expected: {bus.expected_departure_time}");
                }
            }
            else
            {
                Console.WriteLine(":(");
            }
        }
    }
}
