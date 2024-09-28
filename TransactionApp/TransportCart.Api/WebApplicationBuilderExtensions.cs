using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Transactions.Core.Services;
using Transactions.Core.Services.Incentives;
using Transactions.Core.Services.Transports;
using Transactions.Core.Utils.Cookies;
using TransportCart.Api.Data;
using Transactions.Core.Utils.Tokens;
using TransportCart.Api.Services.Carts;
using Transactions.Core.Utils;

namespace TransportCart.Api;

public static class WebApplicationBuilderExtensions
{
	public static void ConfigureDatabase(this WebApplicationBuilder builder)
	{
		string DefaultConnectionString = builder.Configuration.GetConnectionString("DefaultConnection")!;
		builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(DefaultConnectionString));
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

	public static void ConfigureMapper(this WebApplicationBuilder builder)
	{
		IMapper mapper = MappingConfig.RegisterMaps().CreateMapper();
		builder.Services.AddSingleton(mapper);
		//builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
	}
	public static void ConfigureHttpClient(this WebApplicationBuilder builder)
	{
		builder.Services.AddHttpClient();
        builder.Services.AddScoped<BackendApiAuthenticationHttpClientHandler>();

   //     builder.Services.AddHttpClient<ITransportsService, TransportsService>("Transport")
			//.AddHttpMessageHandler<BackendApiAuthenticationHttpClientHandler>();

		builder.Services.AddHttpClient<IIncentiveService, IncentiveService>("Incentive")
			.AddHttpMessageHandler<BackendApiAuthenticationHttpClientHandler>();
    }

    public static void ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IBaseService, BaseService>();
        builder.Services.AddScoped<ICookiesHelper, CookiesHelper>();
        builder.Services.AddScoped<ICookieToken, CookieToken>();
        builder.Services.AddScoped<ITransportsService, TransportsService>();
        builder.Services.AddScoped<IIncentiveService, IncentiveService>();
        builder.Services.AddScoped<ICartService, CartService>();
    }

    public static void ConfigureBackendApiAuthenticationHttpClientHandler(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<BackendApiAuthenticationHttpClientHandler>();

        builder.Services
            .AddHttpClient("Transports", u => u.BaseAddress = new Uri(Constants.TransportAPI))
            .AddHttpMessageHandler<BackendApiAuthenticationHttpClientHandler>();

        builder.Services
            .AddHttpClient("Incentive", u => u.BaseAddress = new Uri(Constants.BonusAPI))
            .AddHttpMessageHandler<BackendApiAuthenticationHttpClientHandler>();
    }

    public static void ConfigureAuth(this WebApplicationBuilder builder)
	{
		IConfigurationSection settingsSection = builder.Configuration.GetSection("ApiSettings")!;

		string secret = settingsSection.GetValue<string>("Secret")!;
		string issuer = settingsSection.GetValue<string>("Issuer")!;
		string audience = settingsSection.GetValue<string>("Audience")!;

		byte[] key = Encoding.ASCII.GetBytes(secret);

		builder.Services.AddAuthentication(x =>
		{
			x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
			x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
		}).AddJwtBearer(x =>
		{
			x.TokenValidationParameters = new TokenValidationParameters
			{
				ValidateIssuerSigningKey = true,
				IssuerSigningKey = new SymmetricSecurityKey(key),
				ValidateIssuer = true,
				ValidIssuer = issuer,
				ValidAudience = audience,
				ValidateAudience = true
			};
		});
	}
}
