using Microsoft.Extensions.DependencyInjection;
using RazorUI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

// Configure HttpClient to access the API REST
builder.Services.AddHttpClient<EspaciosService>(client =>
{
    // Attempt to get the BaseAddress from appsettings.json, if missing fallback to default
    string apiBaseUrl = builder.Configuration["ApiBaseUrl"] ?? "http://localhost:5242/";

    // Check if the BaseAddress is valid
    if (string.IsNullOrEmpty(apiBaseUrl))
    {
        throw new InvalidOperationException("ApiBaseUrl configuration is missing.");
    }

    client.BaseAddress = new Uri(apiBaseUrl); // Set BaseAddress from config

    // Log the BaseApiUrl
    Console.WriteLine($"BaseApiUrl from Configuration: {apiBaseUrl}");
});

// Register EspaciosService
builder.Services.AddScoped<EspaciosService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.MapRazorPages();

app.Run();
