﻿using Newtonsoft.Json.Linq;

namespace BusBoard.Api;

public class BusBoard
{
    const string appId = "4cc50d0f";
    private const string appKey = "0f096d67c0b8ca6ae264fd10d28a4697";
    private static APIRequester apiRequester = new APIRequester();

    public PostcodeData PerformPostcodeLookup(string postcode)
    {
        return apiRequester.MakeAPIRequest<PostcodeData>("https://api.postcodes.io/postcodes/" + postcode);
    }
    
    public List<BusStop> GetBusStopsFromPostcode(PostcodeData postcodeData, int numberOfStops)
    {
        var busStopResult = apiRequester.MakeAPIRequest<BusStopData>(
            "https://transportapi.com/v3/uk/places.json?&app_id=" + appId +
            "&app_key=" + appKey +
            "&lat=" + postcodeData.result.latitude +
            "&lon=" + postcodeData.result.longitude +
            "&type=bus_stop");
        if (busStopResult.member == null)
        {
            return new List<BusStop>();
        }
        List<BusStop> busStopList = busStopResult.member;
        return busStopList.OrderBy(o => o.distance).Take(numberOfStops).ToList();
    }

    public List<BusData> GetDeparturesAtStop(BusStop busStop)
    {
        BusStop busStopResult = apiRequester.MakeAPIRequest<BusStop>("https://transportapi.com/v3/uk/bus/stop/" +
                                                           busStop.atcocode + "/live.json?&app_id=" + appId +
                                                           "&app_key=" + appKey + "&group=no&limit=5");
        if (busStopResult != null && busStopResult.departures.ContainsKey("all"))
        {
            return busStopResult.departures["all"];
        }

        return new List<BusData>();
    }

    public List<string> GetRoute(BusData bus)
    {
        // Get route info for this bus
        if (bus.id == null)
        {
            return new List<string>();
        }
        var busRoute = apiRequester.MakeAPIRequest<BusRoute>(bus.id);
        if (busRoute.stops == null)
        {
            return new List<string>();
        }
        return busRoute.stops.Select(x => x.name).ToList();
    }

    public bool ValidatePostcode(string postcode)
    {
        PostcodeValidation valid = apiRequester.MakeAPIRequest<PostcodeValidation>("https://api.postcodes.io/postcodes/" + postcode +"/validate");
        if(valid.result == false)
        {
            Console.WriteLine("Invalid postcode.");
            return false;
        }
        return true;
    }
}