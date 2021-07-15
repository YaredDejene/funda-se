using System;
using System.Linq;
using System.Collections.Generic;
using Funda.Models;
using Funda.Services;
using System.Threading.Tasks;

namespace Funda
{
    public class Application
    {
        private readonly IHouseFeedClient _houseFeedClient;
        private readonly IHouseService _houseService;
        public Application(IHouseFeedClient houseFeedClient, IHouseService houseService)
        {
            _houseFeedClient = houseFeedClient;
            _houseService = houseService;
        }

        public async Task Run()
        {
            // Fetch All houses in Amsterdam
            await FetchAndStoreHouses();

            // Fetch All houses with in Amsterdam
            await FetchAndStoreHouses(true);

            // Populate the makelaar in Amsterdam that has the most object listed for sale.
            var topMakelaar = _houseService.GetMaakelaarWithHighestNumberOfListing();
            var top10Makelaars = _houseService.GetTop10MaakelaarsWithHighestNumberOfListing();

            // Populate the makelaar in Amsterdam that has the most object (with tuin) listed for sale.
            var topMakelaarTuin = _houseService.GetMaakelaarWithHighestNumberOfListing(true);
            var top10MakelaarsTuin = _houseService.GetTop10MaakelaarsWithHighestNumberOfListing(true);

            // Print Results
            Print(new AggregateModel[] {topMakelaar}, "Top Makelaar");
            Print(top10Makelaars.ToArray(), "Top 10 Makelaar");
            Print(new AggregateModel[] {topMakelaarTuin}, "Top Makelaar (houses with tuin)");
            Print(top10MakelaarsTuin.ToArray(), "Top 10 Makelaar (houses with tuin)");
            
            Console.ReadLine();
        }

        private async Task FetchAndStoreHouses(bool hasTuin = false){
            int page = 1;
            int maxPage = 100;
            string zo = "/amsterdam/" + (hasTuin ? "tuin/" : "");

            while (page <= maxPage) {
                // Fetch current page data from remote api
                var houseFeedData = await _houseFeedClient.GetHouseFeed(type: "koop", zo: zo, page: page, pageSize: 50);

                if(houseFeedData == null || !houseFeedData.Success || houseFeedData.Objects?.Count == 0)  break;

                if(hasTuin){
                    UpdateTuinFlag(houseFeedData.Objects, hasTuin);
                }

                // Save Houses in DB
                await _houseService.SaveHouses(houseFeedData.Objects);

                // Update counters for next pass
                maxPage = houseFeedData.Paging.AantalPaginas;
                page ++;
            }          

        }

        private void UpdateTuinFlag(IEnumerable<HouseModel> houses, bool hasTuin){
            foreach(var house in houses){
                house.HasTuin = hasTuin;
            }
        }

        private void Print(AggregateModel[] models, string heading)
        {
            Console.WriteLine();
            Console.WriteLine(heading);
            Console.WriteLine($"-------------------------------------");

            for(int i = 0; i < models.Length; i++){
                Console.WriteLine($"{i+1}. {models[i].MakelaarNaam} ... {models[i].TotalHouses} houses");
            }

            Console.WriteLine();
        }
    }
}