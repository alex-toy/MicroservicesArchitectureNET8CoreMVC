using AutoMapper;
using Bonus.API.Mappers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Products.API.DbContexts;

namespace Bonus.API;

public static class WebApplicationBuilderExtensions
{
    public static void ConfigureDatabase(this WebApplicationBuilder builder)
    {
        string DefaultConnectionString = builder.Configuration["ConnectionStrings:DefaultConnection"]!;
        builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(DefaultConnectionString));
    }

    public static void ConfigureMapper(this WebApplicationBuilder builder)
    {
        IMapper mapper = MappingConfig.RegisterMaps().CreateMapper();
        builder.Services.AddSingleton(mapper);
    }
}
