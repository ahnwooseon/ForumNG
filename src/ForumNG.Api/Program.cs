using ForumNG.Domain.Interfaces;
using ForumNG.Infrastructure.Data;
using ForumNG.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1. Basic Framework & Infrastructure
builder.AddServiceDefaults();
builder.Services.AddProblemDetails();
builder.Services.AddControllers();

// 2. Database Configuration
var connectionString =
    builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connectionString));

// 3. Caching
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration["Redis:Configuration"];
});

// 4. Business Services
builder.Services.AddMediator(options =>
{
    options.ServiceLifetime = ServiceLifetime.Scoped;
});
builder.Services.AddScoped<IUserRepository, UserRepository>();

var app = builder.Build();

// Configure HTTP Pipeline
app.UseExceptionHandler();
app.MapDefaultEndpoints();
app.MapControllers();

app.Run();
