using Microsoft.AspNetCore.Authentication.Cookies;
using Transactions.Core.Services;
using Transactions.Core.Utils;
using Transactions.Core.Utils.Cookies;
using Transactions.Web.Services.Auth;
using Transactions.Web.Services.Incentives;

namespace Transactions.Web;

public static class WebApplicationBuilderExtensions
{
    public static void ConfigureHttpClient(this WebApplicationBuilder builder)
    {
        builder.Services.AddHttpClient();
        builder.Services.AddHttpClient<IIncentiveService, IncentiveService>();
        builder.Services.AddHttpClient<IAuthService, AuthService>();
        //builder.Services.AddHttpClient<ITransportService, TransportService>();
        //builder.Services.AddHttpClient<ITransportCartService, TransportCartService>();
        //builder.Services.AddHttpClient<IOrderService, OrderService>();
    }

    public static void ConfigureAPIBases(this WebApplicationBuilder builder, string[] args)
    { 
        using IHost host = Host.CreateDefaultBuilder(args).Build();
        IConfiguration config = host.Services.GetRequiredService<IConfiguration>();
        
        Constants.AuthAPIBase = config.GetValue<string>("ServiceUrls:AuthAPI")!;
        Constants.BonusAPIBase = config.GetValue<string>("ServiceUrls:BonusAPI")!;
    }

    public static void ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IBaseService, BaseService>();
        builder.Services.AddScoped<ICookiesHelper, CookiesHelper>();
        builder.Services.AddScoped<ICookieToken, CookieToken>();
        builder.Services.AddScoped<IAuthService, AuthService>();
        builder.Services.AddScoped<IIncentiveService, IncentiveService>();
    }

    public static void ConfigureAuth(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.ExpireTimeSpan = TimeSpan.FromHours(10);
                options.LoginPath = "/Auth/Login";
                options.AccessDeniedPath = "/Auth/AccessDenied";
            });
    }
}
