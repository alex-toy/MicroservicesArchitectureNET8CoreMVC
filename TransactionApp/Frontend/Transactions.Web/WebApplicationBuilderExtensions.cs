using Transactions.Web.Services;
using Transactions.Web.Services.Incentives;
using Transactions.Web.Utils;

namespace Transactions.Web;

public static class WebApplicationBuilderExtensions
{
    public static void ConfigureHttpClient(this WebApplicationBuilder builder)
    {
        builder.Services.AddHttpClient();
        builder.Services.AddHttpClient<IIncentiveService, IncentiveService>();
    }

    public static void ConfigureAPIBases(this WebApplicationBuilder builder, string[] args)
    { 
        using IHost host = Host.CreateDefaultBuilder(args).Build();
        IConfiguration config = host.Services.GetRequiredService<IConfiguration>();
        
        Constants.BonusAPIBase = config.GetValue<string>("ServiceUrls:BonusAPI")!;
    }

    public static void ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IBaseService, BaseService>();
        builder.Services.AddScoped<IIncentiveService, IncentiveService>();
    }
}
