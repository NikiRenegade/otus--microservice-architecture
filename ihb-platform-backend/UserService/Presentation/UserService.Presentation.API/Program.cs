using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using UserService.Domain.Entities;
using UserService.Domain.Interfaces.Repositories;
using UserService.Domain.Interfaces.Services;
using UserService.Infrastructure.EntityFramework.Contexts;
using UserService.Infrastructure.Repositories;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using RabbitMQ.Client;
using Shared.RabbitMq;
using Shared.RabbitMq.Interfaces;
using UserService.Domain.Interfaces.Publishers;
using UserService.Infrastructure.EntityFramework;
using UserService.Infrastructure.Messaging;
using UserService.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("userdbconnection");
builder.Services.AddDbContext<UserDbContext>(options =>
    options.UseNpgsql(connectionString));

// ===== Identity =====
builder.Services.AddDataProtection();
builder.Services
    .AddIdentity<User, IdentityRole<Guid>>(options =>
    {
        options.User.RequireUniqueEmail = true;
        options.Password.RequireDigit = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequiredLength = 6;
    })
    .AddEntityFrameworkStores<UserDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();


// ===== Open Telemetry =====
builder.Services.AddOpenTelemetry()
    .WithMetrics(metrics =>
    {
        metrics
            .SetResourceBuilder(OpenTelemetry.Resources.ResourceBuilder.CreateDefault().AddService("UserService"))
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddRuntimeInstrumentation()
            .AddPrometheusExporter();
    });

builder.Services.AddHealthChecks().AddNpgSql(connectionString);

// ===== JWT Authentication =====
builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.MapInboundClaims = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],

            ValidateAudience = true,
            ValidAudiences = builder.Configuration
                .GetSection("Jwt:Audiences")
                .Get<string[]>(),

            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),

            ValidateLifetime = true
        };
    });
builder.Services.AddAuthorization();

// ===== Controllers + OpenAPI =====
builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "UserService API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Введите JWT токен в формате: Bearer {токен}"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// ===== RabbitMQ =====
var rabbitConfig = builder.Configuration.GetSection("RabbitMQ");
builder.Services.AddSingleton(async sp =>
{
    var factory = new ConnectionFactory
    {
        HostName = rabbitConfig["HostName"],
        Port = int.Parse(rabbitConfig["Port"]!),
        UserName = rabbitConfig["UserName"],
        Password = rabbitConfig["Password"]
    };

    var connection = await factory.CreateConnectionAsync();
    return connection;
});

builder.Services.AddSingleton(async sp =>
{
    var connection = await sp.GetRequiredService<Task<IConnection>>();
    var channel = await connection.CreateChannelAsync();
    return channel;
});
builder.Services.AddScoped<IEventPublisher, RabbitMqEventPublisher>();
builder.Services.AddScoped<IUserEventPublisher, RabbitMqUserEventPublisher>();


// ===== Application Services =====
builder.Services.AddScoped<IUserService, UserService.Application.Services.UserService>();

// ===== Infrastructure Services =====
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
builder.Services.AddScoped<IIdentityService, IdentityService>();

// ===== Repositories =====
builder.Services.AddScoped<IUserRepository, UserRepository>();




var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    await RoleSeeder.SeedRolesAsync(services);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapPrometheusScrapingEndpoint();
app.MapHealthChecks("/health");
// ===== Middleware =====
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();

