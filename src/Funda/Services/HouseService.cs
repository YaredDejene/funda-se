using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Funda.Data;
using Funda.Models;
using Funda.Repositories;

namespace Funda.Services
{
    public class HouseService : IHouseService
    {
        private readonly IRepository<House> _houseRepository;
        private readonly IMapper _mapper;
        public HouseService(IRepository<House> houseRepository, IMapper mapper)
        {
            _houseRepository = houseRepository ?? throw new ArgumentNullException(nameof(houseRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        public AggregateModel GetMaakelaarWithHighestNumberOfListing(bool hasTuin = false)
        {
            var top10 = GetTop10MaakelaarsWithHighestNumberOfListing(hasTuin);
            return top10.FirstOrDefault();
        }

        public IEnumerable<AggregateModel> GetTop10MaakelaarsWithHighestNumberOfListing(bool hasTuin = false)
        {
            // Fetch houses from db
            var houses = hasTuin == true ? _houseRepository.Filter(h => h.HasTuin) : _houseRepository.GetAll();

            // Aggregation by count and select top 10
            var result = houses.GroupBy(x => x.MakelaarNaam)
                        .Select(g => new AggregateModel {
                            MakelaarNaam = g.Key,
                            TotalHouses = g.Count()
                        })
                        .OrderByDescending(o => o.TotalHouses)
                        .Take(10);
            
            return result;
        }

        public async Task SaveHouses(IEnumerable<HouseModel> houses)
        {
            foreach(var house in houses){
                var savedHouse = _houseRepository.Filter(h => h.Id == house.Id).FirstOrDefault();

                // new house entry 
                if(savedHouse == null){
                    var newHouse = _mapper.Map<House>(house);
                    await _houseRepository.AddAsync(newHouse);
                }
                else{
                    savedHouse.HasTuin = house.HasTuin;
                    await _houseRepository.UpdateAsync(savedHouse);
                }
            }
        }
    }
}