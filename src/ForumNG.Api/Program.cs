using FluentResults;
using ForumNG.Api.Endpoints;
using ForumNG.Application.Commands.Posts.CreatePost;
using ForumNG.Application.Commands.Topics.CreateTopic;
using ForumNG.Application.Mappings;
using ForumNG.Application.Queries.Posts.GetPostById;
using ForumNG.Application.Queries.Topics.GetAllTopics;
using ForumNG.Application.Queries.Topics.GetTopicById;
using ForumNG.Application.Queries.Users.GetUserById;
using ForumNG.Domain.DTOs;
using ForumNG.Domain.Interfaces;
using ForumNG.Infrastructure.Data;
using ForumNG.Infrastructure.Repositories;
using Mediator;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire components.
builder.AddServiceDefaults();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddProblemDetails();

var connectionString =
    builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connectionString));

builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddScoped<ITopicRepository, TopicRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddMediator(options =>
{
    options.ServiceLifetime = ServiceLifetime.Scoped;
});

MapsterRegister.RegisterMappings();

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration["Redis:Configuration"];
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

CategoriesEndpoints.MapCategoriesEndpoints(app);
PostsEndpoints.MapPostsEndpoints(app);
TopicsEndpoints.MapTopicsEndpoints(app);
UsersEndpoints.MapUsersEndpoints(app);

app.Run();
