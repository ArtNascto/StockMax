using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using StockMax.API;
using StockMax.API.Handlers;
using StockMax.Domain.Interfaces.Database;
using StockMax.Domain.Models.Entity;
using StockMax.Infra.CrossCutting;
using StockMax.Infra.Data.Repositories;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "cors_config",
                  builder =>
                  {
                      builder.AllowAnyOrigin();
                      builder.AllowAnyHeader();
                      builder.AllowAnyMethod();
                  });
});

builder.Services.AddControllers(options =>
{
    options.Conventions.Add(new RouteConvention());
}).SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
.AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
})
.AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ContractResolver = new DefaultContractResolver
    {
    };
});

DependencyInjectionExtensions.ConfigureServices(builder.Services);
builder.Services.AddSwaggerGenNewtonsoftSupport();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "API Stock Max" });
});

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<StockMaxDbContext>(
            dbContextOptions => dbContextOptions
                .UseSqlServer(connectionString)
                .LogTo(Console.WriteLine, LogLevel.Information)
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors()
        );
builder.Services.AddScoped<DbContext, StockMaxDbContext>();
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequiredLength = 4;
    options.Password.RequireLowercase = false;
    options.Password.RequireDigit = false;
    options.Password.RequiredUniqueChars = 0;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.User.RequireUniqueEmail = true;
    options.SignIn.RequireConfirmedEmail = false;
});

builder.Services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<StockMaxDbContext>()
                .AddUserValidator<CustomUserValidator<User>>()
                .AddDefaultTokenProviders();
builder.Services.AddScoped<IIUserStore, UserStore>();
builder.Services.AddScoped<JwtHandler>();
builder.Services.AddTransient<ISeed, Seed>();
IdentityModelEventSource.ShowPII = true;
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["validIssuer"],
        ValidAudience = jwtSettings["validAudience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
            .GetBytes(jwtSettings.GetSection("securityKey").Value))
    };
});
var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();
app.UseCors("cors_config");

app.MapControllers();
var scopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
using (var scope = scopeFactory.CreateScope())
{
    try
    {
        var context = scope.ServiceProvider.GetRequiredService<StockMaxDbContext>();

        await context.Database.MigrateAsync();
        var seed = scope.ServiceProvider.GetRequiredService<ISeed>();
        await seed.SeedInitialUsers();
        await seed.SeedColors();
    }
    catch (Exception)
    {
        throw;
    }
}
app.Run();