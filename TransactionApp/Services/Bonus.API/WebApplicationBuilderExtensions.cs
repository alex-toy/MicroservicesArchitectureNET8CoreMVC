using AutoMapper;
using Bonus.API.Data;
using Bonus.API.Mappers;
using Bonus.API.Services;
using Microsoft.EntityFrameworkCore;

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

    public static void ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IIncentiveService, IncentiveService>();
    }
}
