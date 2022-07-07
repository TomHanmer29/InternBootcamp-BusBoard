using Newtonsoft.Json;

namespace BusBoard.ConsoleApp;

public class APIRequester
{
    HttpClient client = new HttpClient();

    public async Task<dataType> RequestAndDeserialize<dataType>(string address)
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
        var deserialized = JsonConvert.DeserializeObject<dataType>(result);
        if (deserialized != null)
        {
            return deserialized;
        }
        throw new JsonException("Failed to deserialize JSON object.");
    }
}