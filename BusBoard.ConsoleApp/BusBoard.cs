namespace BusBoard.ConsoleApp;

public class BusBoard
{
    const string appId = "a5babc51";
    private const string appKey = "f057beb130e643c45171f1ae19b3e4fd";
    private static APIRequester apiRequester = new APIRequester();

    public PostcodeData PerformPostcodeLookup(string postcode)
    {
        return apiRequester.RequestAndDeserialize<PostcodeData>("https://api.postcodes.io/postcodes/" + postcode).Result;
    }
    
    public List<BusStop> GetBusStopsFromPostcode(PostcodeData postcodeData, int numberOfStops)
    {
        List<BusStop> busStopList = apiRequester.RequestAndDeserialize<BusStopData>(
                "https://transportapi.com/v3/uk/places.json?&app_id=" + appId +
                "&app_key=" + appKey +
                "&lat=" + postcodeData.result.latitude +
                "&lon=" + postcodeData.result.longitude +
                "&type=bus_stop")
            .Result.member;
        return busStopList.OrderBy(o => o.distance).Take(numberOfStops).ToList();
    }

    public List<BusData> GetDeparturesAtStop(BusStop busStop)
    {
        BusStop busStopResult = apiRequester.RequestAndDeserialize<BusStop>("https://transportapi.com/v3/uk/bus/stop/" +
                                                           busStop.atcocode + "/live.json?&app_id=" + appId +
                                                           "&app_key=" + appKey + "&group=no&limit=5").Result;
        if (busStopResult != null && busStopResult.departures.ContainsKey("all"))
        {
            return busStopResult.departures["all"];
        }

        return new List<BusData>();
    }

    public List<string> GetRoute(BusData bus)
    {
        // Get route info for this bus
        var busRoute = apiRequester.RequestAndDeserialize<BusRoute>(bus.id).Result;
        return busRoute.stops.Select(x => x.name).ToList();
    }
}