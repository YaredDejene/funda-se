using System.Collections.Generic;
using System.Threading.Tasks;
using Funda.Models;

namespace Funda.Services
{
    public interface IHouseService
    {
        AggregateModel GetMaakelaarWithHighestNumberOfListing(bool hasTuin = false);
        IEnumerable<AggregateModel> GetTop10MaakelaarsWithHighestNumberOfListing(bool hasTuin = false);
        Task SaveHouses(IEnumerable<HouseModel> houses);
    }
}