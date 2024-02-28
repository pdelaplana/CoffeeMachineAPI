using CoffeeMachineAPI;
using CoffeeMachineAPI.Application;
using CoffeeMachineAPI.Endpoints.V1;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMemoryCache();

builder.Services.AddHttpClient();

builder.Services.Configure<WeatherServiceOptions>(options => {
    options.ApiKey = builder.Configuration.GetSection("WeatherService").GetValue<string>("ApiKey") ?? "";
    options.City = builder.Configuration.GetSection("WeatherService").GetValue<string>("City") ?? "";
});

builder.Services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
builder.Services.AddSingleton<IWeatherService, WeatherService>();
builder.Services.AddSingleton<ICoffeeMachine, CoffeeMachineV2>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapV1Endpoints();

app.Run();

// Used this partial class for unit testing
namespace CoffeeMachineAPI
{
    public partial class Program { }
}
