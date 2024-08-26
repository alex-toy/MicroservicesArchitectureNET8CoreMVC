using AutoMapper;
using Transports.Api.Dtos;
using Transports.Api.Models;

namespace Bonus.API.Mappers;

public class MappingConfig
{
    public static MapperConfiguration RegisterMaps()
    {
        MapperConfiguration mappingConfig = new (config =>
        {
            config.CreateMap<TransportDto, Transport>().ReverseMap();
        });

        return mappingConfig;
    }
}
