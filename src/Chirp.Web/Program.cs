using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json");
var tempFolder = Path.GetTempPath();
var dbFileName = "chirp2.db";
var dbPath = Path.Combine(tempFolder, dbFileName);
builder.Configuration["ConnectionStrings:ChirpDbConnectionSQlite"] = $"Data Source={dbPath}";

var connString = builder.Configuration.GetConnectionString("ChirpDbConnectionSQlite");


// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddScoped<ICheepRepository, CheepRepository>(); // Scoped to fit with DBContext
builder.Services.AddDbContext<ChirpDBContext>(
    options => options.UseSqlite(connString));



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

using (var scope = app.Services.CreateScope()) // https://learn.microsoft.com/en-us/aspnet/core/data/ef-rp/intro?view=aspnetcore-7.0&tabs=visual-studio
{
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<ChirpDBContext>();
    context.Database.EnsureCreated();
    DbInitializer.SeedDatabase(context);
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.MapRazorPages();

app.Run();

public partial class Program { } //To enable the testing
