using System.Reflection;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using learning_center_back.Shared.Domain;
using learning_center_back.Shared.Infrastructure.Persistence.Configuration;
using learning_center_back.Shared.Infraestructure.Persistence.Repositories;
using learning_center_back.Tutorial.Domain.Services;
using learning_center_back.Tutorials.Application.CommandServices;
using learning_center_back.Tutorials.Application.QueryServices;
using learning_center_back.Tutorials.Domain;
using learning_center_back.Tutorials.Domain.Models.Validadors;
using learning_center_back.Tutorials.Infraestructure;
using learning_center_back.Security.Application;
using learning_center_back.Security.Infraestrucutre;
using learning_center_back.Shared.Application.Commands;
using learning_center_back.Shared.Application.Commands.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Configure Web Host
builder.WebHost.UseUrls("http://localhost:5000");

// Add Controllers
builder.Services.AddControllers();

// Add Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Learning Center App",
        Description = "APIs to handle data for public library in Lima",
        TermsOfService = new Uri("https://example.com/terms"),
        Contact = new OpenApiContact
        {
            Name = "Naldo",
            Email = "naldo@example.com",
            Url = new Uri("https://example.com/contact")
        },
        License = new OpenApiLicense
        {
            Name = "Example License",
            Url = new Uri("https://example.com/license")
        }
    });

    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

// Add CORS Policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllPolicy", policy =>
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader());
});

// Configure Database
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new Exception("Database connection string is not set.");

builder.Services.AddDbContext<LearningCenterContext>(options =>
{
    options.UseMySQL(connectionString);

    if (builder.Environment.IsDevelopment())
    {
        options.LogTo(Console.WriteLine, LogLevel.Information)
               .EnableSensitiveDataLogging()
               .EnableDetailedErrors();
    }
    else if (builder.Environment.IsProduction())
    {
        options.LogTo(Console.WriteLine, LogLevel.Error)
               .EnableDetailedErrors();
    }
});

// Dependency Injection - Shared
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IBookQueryService, BookQueryService>();
builder.Services.AddScoped<IBookCommandService, BookCommandService>();

// Dependency Injection - Security
builder.Services.AddScoped<IUserCommandService, UserCommandService>();
builder.Services.AddScoped<IHashService, HashService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IJwtEncryptService, JwtEncryptService>();

// Validators
builder.Services.AddValidatorsFromAssemblyContaining<CreateBookCommandValidator>();

var app = builder.Build();

// Swagger UI
app.UseSwagger();
app.UseSwaggerUI();

// Ensure DB is created
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<LearningCenterContext>();
    context.Database.EnsureCreated();
}

// Middleware
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseCors("AllowAllPolicy");
app.UseAuthorization();
app.MapControllers();
app.Run();
