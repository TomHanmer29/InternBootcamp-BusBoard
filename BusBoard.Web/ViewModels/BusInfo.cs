using BusBoard.Api;

namespace BusBoard.Web.ViewModels
{
    public class BusInfo
    {
        private Api.BusBoard busBoard = new Api.BusBoard();
        
        public BusInfo(string postCode)
        {
            PostCode = postCode;
        }

        public string PostCode { get; set; }

        public bool ValidatePostcode()
        {
            return busBoard.ValidatePostcode(PostCode);
        }

        public List<BusStop> GetStops()
        {
            return busBoard.GetBusStopsFromPostcode(busBoard.PerformPostcodeLookup(PostCode), 2);
        }

        public List<BusData> GetDeparturesAtStop(BusStop stop)
        {
            return busBoard.GetDeparturesAtStop(stop);
        }

        public string GetRoute(BusData bus)
        {
            var routeList = busBoard.GetRoute(bus);
            return string.Join(" → ", routeList);
        }

        public Dictionary<BusData, string> findMaxRouteLength(List<BusData> busList)
        {
            Dictionary<BusData, string> busAndRoute = new Dictionary<BusData, string>();
            foreach (var bus in busList)
            {
                busAndRoute.Add(bus, GetRoute(bus));
            }

            return busAndRoute;
        }

    }
}
