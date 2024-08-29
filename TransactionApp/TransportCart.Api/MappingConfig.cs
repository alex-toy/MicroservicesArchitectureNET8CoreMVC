using AutoMapper;
using Transactions.Core.Dtos.TransportCarts;
using TransportCart.Api.Models;

namespace TransportCart.Api;

public class MappingConfig
{
	public static MapperConfiguration RegisterMaps()
	{
		var mappingConfig = new MapperConfiguration(config =>
		{
			config.CreateMap<CartHeader, CartHeaderDto>().ReverseMap();
			config.CreateMap<CartDetails, CartDetailsDto>().ReverseMap();
		});

		return mappingConfig;
	}
}
