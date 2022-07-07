using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BusBoard
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Enter bus stop code:");
            //string userInput = Console.ReadLine();
            HttpClient client = new HttpClient();
            Console.WriteLine("Welcome to BusBoard!");
            //var result = await client.GetStringAsync("https://transportapi.com/v3/uk/bus/stop/"+userInput+"/live.json?&app_id=a5babc51&app_key=f057beb130e643c45171f1ae19b3e4fd&group=no&limit=5");
            var result = await client.GetStringAsync("https://transportapi.com/v3/uk/bus/stop/0500CCITY436/live.json?&app_id=a5babc51&app_key=f057beb130e643c45171f1ae19b3e4fd&group=no&limit=5");
            var stopResult = JsonConvert.DeserializeObject<StopData>(result);


            foreach (var bus in stopResult.departures["all"])
            {
                Console.WriteLine($"Line: {bus.line}, Destination: {bus.direction}, Time: {bus.aimed_departure_time}, Expected: {bus.expected_departure_time}");
            }

            Console.WriteLine();
        }
    }
    
    
}
