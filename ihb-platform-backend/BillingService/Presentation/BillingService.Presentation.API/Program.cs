using System.Text;
using BillingService.Domain.Interfaces.Repositories;
using BillingService.Domain.Interfaces.Services;
using BillingService.Infrastructure.EntityFramework.Contexts;
using BillingService.Infrastructure.Repositories;
using BillingService.Application.Services;
using BillingService.Infrastructure.Messaging;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OpenTelemetry.Metrics;
using RabbitMQ.Client;
using Shared.RabbitMq;
using Shared.RabbitMq.Interfaces;
using OpenTelemetry.Resources;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("billingdbconnection");
builder.Services.AddDbContext<BillingDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddOpenTelemetry()
    .WithMetrics(metrics =>
    {
        metrics
            .SetResourceBuilder(OpenTelemetry.Resources.ResourceBuilder.CreateDefault().AddService("BillingService"))
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
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ServiceOnly", policy =>
        policy.RequireClaim("type", "service")
            .RequireClaim("scope", "billing.internal"));
});

// ===== Controllers + OpenAPI =====
builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "BillingService API", Version = "v1" });

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


builder.Services.AddEndpointsApiExplorer();


// Repositories
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();

// Services
builder.Services.AddScoped<IAccountService, AccountService>();


// RabbitMQ
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
    return await factory.CreateConnectionAsync();
});

builder.Services.AddSingleton(async sp =>
{
    var connection = await sp.GetRequiredService<Task<IConnection>>();
    var channel = await connection.CreateChannelAsync();
    return channel;
});

builder.Services.AddSingleton<IEventConsumer, RabbitMqEventConsumer>();
builder.Services.AddScoped<IRabbitMqConsumer, RabbitMqUserEventConsumer>();
builder.Services.AddHostedService<RabbitMqConsumersBackgroundService>();

var app = builder.Build();
app.MapPrometheusScrapingEndpoint();
app.MapHealthChecks("/health");
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();