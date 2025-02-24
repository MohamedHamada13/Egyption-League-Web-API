using Microsoft.EntityFrameworkCore;
using WebAPI_Revision.AutoMapperMappingProfiles;
using WebAPI_Revision.CustomLogging;
using WebAPI_Revision.Repos_DP;
using WebAPI_Revision.Repos_DP.Interfaces;
using WebAPI_Revision.Repos_DP.Repos_Classes;
using WebAPI_Revision.Data;

// Apply Onion architecture

var builder = WebApplication.CreateBuilder(args);

/// Add services to the App container.

builder.Services.AddControllers(); // Enable the API to add controllers and deal with it.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Db Registeration
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(Options => Options.UseSqlServer(connectionString));

// AutoMapper Profile Registeration
builder.Services.AddAutoMapper(typeof(MappingProfile));

// EFC Logger is automatically regestred as it is empeded in the EFC
// CustomLogger Regestration
builder.Services.AddSingleton<ICustomLog, CustomLog>();

// Generic & specific Repositories Regestration
builder.Services.AddScoped(typeof(IGeneRepos<>), typeof(GeneRepos<>));
builder.Services.AddScoped<ITeamRepos, TeamRepos>();    // Team Repository Regestration
builder.Services.AddScoped<IPlayerRepos, PlayerRepos>(); // Player Repository Regestration


// Build the app with the predefined services.
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection(); // To Apply Https for more security [Https encrypt the transformed data]

app.UseAuthorization();

app.MapControllers();

app.Run();
