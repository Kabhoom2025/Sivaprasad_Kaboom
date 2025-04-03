using System.Text;
using Kaboom.Interfaces;
using Kaboom.Models;
using Kaboom.Services;
using Kaboom.Services.Auth;
using Kaboom.Services.Repository;
using Kaboom.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Configuration.SetBasePath(Directory.GetCurrentDirectory()).
    AddJsonFile("appsettings.json",optional: false, reloadOnChange: true).
    AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true,reloadOnChange:true)
    .AddEnvironmentVariables();

var JwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.UTF8.GetBytes(JwtSettings["SecurityKey"]);
builder.Services.AddAuthentication(Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = JwtSettings["Issuer"],
            ValidAudience = JwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ClockSkew = TimeSpan.Zero // No tolerance for token expiry
        };
    });
builder.Services.AddAuthorization();
// SQL server configuration
var SQLconnectionstring = builder.Configuration.GetConnectionString("SQLConnection");
if (string.IsNullOrEmpty(SQLconnectionstring))
{
    throw new ArgumentNullException("SqlServerConnection", "SQL Server connection string is missing in appsettings.json.");
}
//sql entity framework
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("SQLConnection");

    options.UseSqlServer(connectionString);

    // ✅ Enable logging and sensitive data logging
    options.EnableSensitiveDataLogging(); // Enables logging of sensitive data (use cautiously in production)
    options.LogTo(Console.WriteLine, LogLevel.Information); // Logs SQL queries and errors to the console
});

//MongoDB
var MongoConnectionString = builder.Configuration.GetConnectionString("MongoDbConnection");
if (string.IsNullOrEmpty(MongoConnectionString))
{
    throw new ArgumentNullException("MongoDbConnection", "MongoDB connection string is missing in appsettings.json.");
}
//register Mongo DB
builder.Services.AddSingleton<IMongoClient>(new MongoClient(MongoConnectionString));
builder.Services.AddSingleton<IMongoClient>(sp =>
    new MongoClient(MongoConnectionString)
);
//Register Database services
builder.Services.AddSingleton<IDataBaseService, DatabaseService>(); //Default to SQL

// Register Repository Factory
builder.Services.AddScoped<RepositoryFactory>();
builder.Services.AddScoped<IDataBaseService,DatabaseService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddSignalR();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthorization();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapHub<OrderHub>("/orderHub");
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.MapControllers();

app.Run();
