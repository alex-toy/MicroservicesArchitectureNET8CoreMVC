using AutoMapper;
using Bonus.API.Mappers;
using Microsoft.EntityFrameworkCore;
using Transports.Api.Data;
using Transports.Api.Services;

namespace Transports.Api;

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
        //builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
    }

    public static void ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<ITransportService, TransportService>();
    }
}
