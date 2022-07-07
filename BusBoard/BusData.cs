using Newtonsoft.Json.Linq;

namespace BusBoard;

public class StopData
{
    public string name;
    public Dictionary<string, List<BusData>> departures;
}

public class BusData
{
    public string line;
    public string direction;
    public string aimed_departure_time;
    public string expected_departure_time;
    public string id;
}

public class BusRoute
{
    public List<RouteStop> stops;
}

public class RouteStop
{
    public string name;
}
