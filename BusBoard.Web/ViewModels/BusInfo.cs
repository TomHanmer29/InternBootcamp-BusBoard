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

        public List<Api.BusStop> GetStops()
        {
            return busBoard.GetBusStopsFromPostcode(busBoard.PerformPostcodeLookup(PostCode), 2);
        }

    }
}
