using AutoMapper;
using Bonus.API.Dtos;
using Bonus.API.Models;

namespace Bonus.API.Mappers;

public class MappingConfig
{
    public static MapperConfiguration RegisterMaps()
    {
        MapperConfiguration mappingConfig = new (config =>
        {
            config.CreateMap<IncentiveDto, Incentive>();
            config.CreateMap<Incentive, IncentiveDto>();
        });

        return mappingConfig;
    }
}
