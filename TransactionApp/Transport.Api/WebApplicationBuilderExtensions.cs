﻿using AutoMapper;
using Bonus.API.Mappers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Transactions.Core.Services;
using Transactions.Core.Utils.Cookies;
using Transports.Api.Data;
using Transactions.Core.Utils.Tokens;
using Transports.Api.Services.Transports;

namespace Transports.Api;

public static class WebApplicationBuilderExtensions
{
	public static void ConfigureHttpClient(this WebApplicationBuilder builder)
	{
		builder.Services.AddHttpClient();
		builder.Services.AddHttpClient<ITransportService, TransportService>();
	}

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
		builder.Services.AddScoped<ICookiesHelper, CookiesHelper>();
		builder.Services.AddScoped<ICookieToken, CookieToken>();
		builder.Services.AddScoped<IBaseService, BaseService>();
        builder.Services.AddScoped<ITransportService, TransportService>();
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
