using Newtonsoft.Json;

namespace BusBoard.ConsoleApp;

public class APIRequester
{
    HttpClient client = new HttpClient();

    private async Task<string> Request(string address)
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

        return result;
    }

    private dataType Deserialize<dataType>(string input)
    {
        var deserialized = JsonConvert.DeserializeObject<dataType>(input);
        if (deserialized != null)
        {
            return deserialized;
        }
        throw new JsonException("Failed to deserialize JSON object.");
    }

    public dataType RequestAndDeserialize<dataType>(string address) where dataType : new()
    {
        var result = Request(address).Result;
        if (result == "")
        {
            return new dataType();
        }
        return Deserialize<dataType>(result);
    }
}