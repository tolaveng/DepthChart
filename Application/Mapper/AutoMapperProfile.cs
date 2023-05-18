using Application.Models;
using AutoMapper;
using Domain.Entities;


namespace Application.Mapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<PlayerDto, Player>().ReverseMap();
            CreateMap<ChartDto, Chart>().ReverseMap();
            CreateMap<PositionDto, Position>().ReverseMap();
            CreateMap<TeamDto, Team>().ReverseMap();
        }
    }
}
