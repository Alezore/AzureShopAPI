using AzureShopAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
/*
string jsonPath = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
Console.WriteLine(" Vérification du fichier appsettings.json : " + jsonPath);
Console.WriteLine(" Existe ? " + File.Exists(jsonPath));*/

var config = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

string connectionString = config.GetConnectionString("DefaultConnection");
//Console.WriteLine(" Connection String: " + connectionString);

//if (string.IsNullOrEmpty(connectionString))
//{
//   Console.WriteLine(" ERREUR chaîne de connexion est vide");
//}


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:4200") 
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddDbContext<AzureShopDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
    );

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseStaticFiles();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors();
app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("index.html"); //page gestion adresse des client TO DO : css

app.Run();