using System.Threading.Tasks;
using Funda.Models;

namespace Funda.Services
{
    public interface IHouseFeedClient
    {
        Task<HouseFeedModel> GetHouseFeed(string type, string zo, int? page, int? pageSize );
    }
}