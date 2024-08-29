using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using TransportCart.Api;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.ConfigureAPIBases(args);

builder.Services.AddHttpContextAccessor();

builder.ConfigureHttpClient();

builder.ConfigureDatabase();

builder.ConfigureMapper();

builder.ConfigureServices();

//builder.Services.AddScoped<BackendApiAuthenticationHttpClientHandler>();

//builder.Services.AddScoped<IMessageBus, MessageBus>();

//builder.Services.AddHttpClient("Transport", u => u.BaseAddress = new Uri(builder.Configuration["ServiceUrls:TransportAPI"])).AddHttpMessageHandler<BackendApiAuthenticationHttpClientHandler>();
//builder.Services.AddHttpClient("incentive", u => u.BaseAddress = new Uri(builder.Configuration["ServiceUrls:IncentiveAPI"])).AddHttpMessageHandler<BackendApiAuthenticationHttpClientHandler>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.ConfigureAuth();

builder.Services.AddSwaggerGen(option =>
{
	option.AddSecurityDefinition(name: JwtBearerDefaults.AuthenticationScheme, securityScheme: new OpenApiSecurityScheme
	{
		Name = "Authorization",
		Description = "Enter the Bearer Authorization string as following: `Bearer Generated-JWT-Token`",
		In = ParameterLocation.Header,
		Type = SecuritySchemeType.ApiKey,
		Scheme = "Bearer"
	});
	option.AddSecurityRequirement(new OpenApiSecurityRequirement
	{
		{
			new OpenApiSecurityScheme
			{
				Reference= new OpenApiReference
				{
					Type=ReferenceType.SecurityScheme,
					Id=JwtBearerDefaults.AuthenticationScheme
				}
			}, new string[]{}
		}
	});
});

builder.Services.AddAuthorization();








var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
