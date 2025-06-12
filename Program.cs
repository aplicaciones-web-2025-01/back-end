using learning_center_back.Shared.Domain;
using learning_center_back.Shared.Infraestructure.Persistence.Repositories;
using learning_center_back.Shared.Infrastructure.Persistence.Configuration;
using learning_center_back.Tutorial.Domain.Services;
using learning_center_back.Tutorials.Application.CommandServices;
using learning_center_back.Tutorials.Application.QueryServices;
using learning_center_back.Tutorials.Domain;
using learning_center_back.Tutorials.Infraestructure;
using Microsoft.EntityFrameworkCore;
using Azure.Monitor.OpenTelemetry.AspNetCore;
using FluentValidation;
using learning_center_back.Tutorials.Domain.Models.Validadors;

var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddOpenTelemetry().UseAzureMonitor();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Add Database Connection
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Verify Database Connection String
if (connectionString is null)
    // Stop the application if the connection string is not set.
    throw new Exception("Database connection string is not set.");

// Configure Database Context and Logging Levels
if (builder.Environment.IsDevelopment())
    builder.Services.AddDbContext<LearningCenterContext>(
        options =>
        {
            options.UseMySQL(connectionString)
                .LogTo(Console.WriteLine, LogLevel.Information)
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors();
        });
else if (builder.Environment.IsProduction())
    builder.Services.AddDbContext<LearningCenterContext>(
        options =>
        {
            options.UseMySQL(connectionString)
                .LogTo(Console.WriteLine, LogLevel.Error)
                .EnableDetailedErrors();
        });

// Configure Dependency Injection

// Shared Bounded Context Injection Configuration
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IBookQueryService, BookQueryService>();
builder.Services.AddScoped<IBookCommandService, BookCommandService>();
builder.Services.AddValidatorsFromAssemblyContaining<CreateBookCommandValidator>();

// News Bounded Context Injection Configuration
builder.WebHost.UseUrls("http://localhost:5000");

var app = builder.Build();

// Verify Database Objects are created
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<LearningCenterContext>();
    context.Database.EnsureCreated();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();