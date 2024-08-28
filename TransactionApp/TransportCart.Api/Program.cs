using TransportCart.Api;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

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
