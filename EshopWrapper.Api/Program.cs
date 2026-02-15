using EshopWrapper.Core;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddHttpClient<IEshopClient, EshopClient>(client =>
{
    client.BaseAddress = new Uri("https://restapi.e-shops.co.il/");
});

// Configure the client with the key from appsettings
builder.Services.AddScoped<IEshopClient>(sp => 
{
    var httpClient = sp.GetRequiredService<IHttpClientFactory>().CreateClient(nameof(EshopClient));
    var configuration = sp.GetRequiredService<IConfiguration>();
    var key = configuration["EshopApi:Key"];
    if (string.IsNullOrEmpty(key))
    {
        throw new InvalidOperationException("EshopApi:Key is missing in configuration.");
    }
    return new EshopClient(httpClient, key);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
