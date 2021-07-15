using AutoMapper;
using Funda.Data;
using Funda.Models;

namespace Funda.Mapping
{
    public class GeneralProfile : Profile
    {
        public GeneralProfile()
        {
            CreateMap<House, HouseModel>().ReverseMap();
        }
    }
}