using Microsoft.AspNetCore.Authentication.Cookies;
using Transactions.Core.Services;
using Transactions.Core.Services.Incentives;
using Transactions.Core.Services.Transports;
using Transactions.Core.Utils;
using Transactions.Core.Utils.Cookies;
using Transactions.Core.Utils.Tokens;
using Transactions.Web.Services.Auth;

namespace Transactions.Web;

public static class WebApplicationBuilderExtensions
{
    public static void ConfigureHttpClient(this WebApplicationBuilder builder)
    {
        builder.Services.AddHttpClient();
        builder.Services.AddHttpClient<IIncentiveService, IncentiveService>();
        builder.Services.AddHttpClient<IAuthService, AuthService>();
    }

    public static void ConfigureAPIBases(this WebApplicationBuilder builder, string[] args)
    { 
        using IHost host = Host.CreateDefaultBuilder(args).Build();
        IConfiguration config = host.Services.GetRequiredService<IConfiguration>();
        
        Constants.AuthAPI = config.GetValue<string>("ServiceUrls:AuthAPI")!;
        Constants.BonusAPI = config.GetValue<string>("ServiceUrls:BonusAPI")!;
        Constants.TransportAPI = config.GetValue<string>("ServiceUrls:TransportAPI")!;
        Constants.TransportCartAPI = config.GetValue<string>("ServiceUrls:TransportCartAPI")!;
	}

    public static void ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IBaseService, BaseService>();
        builder.Services.AddScoped<ICookiesHelper, CookiesHelper>();
        builder.Services.AddScoped<ICookieToken, CookieToken>();
        builder.Services.AddScoped<IAuthService, AuthService>();
        builder.Services.AddScoped<IIncentiveService, IncentiveService>();
        builder.Services.AddScoped<ITransportsService, TransportsService>();
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
