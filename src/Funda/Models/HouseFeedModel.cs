using System.Collections.Generic;

namespace Funda.Models
{
    public class HouseFeedModel
    {
        public HouseFeedModel()
        {
            
        }

        public HouseFeedModel(bool success, string message)
        {
            Success = success;
            Message = message;
        }

        public IList<HouseModel> Objects { get; set; }
        public PagingModel Paging { get; set; }
        public int TotaalAantalObjecten { get; set; }
        public bool Success { get; set; } = true;
        public string Message { get; set; }
    }
}