using Microsoft.Extensions.DependencyInjection;
using RazorUI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

// Configure HttpClient to access the API REST
builder.Services.AddHttpClient<EspaciosService>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["BaseApiUrl"]); 
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
