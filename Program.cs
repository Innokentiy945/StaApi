using Microsoft.EntityFrameworkCore;
using DotNetEnv;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.OpenApi.Models;
using StaApi.Context;
using StaApi.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using StaApi.Services;

var builder = WebApplication.CreateBuilder(args);

// -------------------- SERVICES --------------------

builder.Services.AddScoped<IDictionarySTA, DictionaryStaService>();
builder.Services.AddScoped<JwtService>();

builder.Services.AddHttpContextAccessor();

Env.Load();

// -------------------- ENV --------------------

string DB_HOST_DICTIONARY = Environment.GetEnvironmentVariable("DB_HOST_DICTIONARY")!;
string DB_PORT_DICTIONARY = Environment.GetEnvironmentVariable("DB_PORT_DICTIONARY")!;
string DB_NAME_DICTIONARY = Environment.GetEnvironmentVariable("DB_NAME_DICTIONARY")!;
string DB_USER_DICTIONARY = Environment.GetEnvironmentVariable("DB_USER_DICTIONARY")!;
string DB_PASSWORD_DICTIONARY = Environment.GetEnvironmentVariable("DB_PASSWORD_DICTIONARY")!;

string DB_HOST_AUTH = Environment.GetEnvironmentVariable("DB_HOST_AUTH")!;
string DB_PORT_AUTH = Environment.GetEnvironmentVariable("DB_PORT_AUTH")!;
string DB_NAME_AUTH = Environment.GetEnvironmentVariable("DB_NAME_AUTH")!;
string DB_USER_AUTH = Environment.GetEnvironmentVariable("DB_USER_AUTH")!;
string DB_PASSWORD_AUTH = Environment.GetEnvironmentVariable("DB_PASSWORD_AUTH")!;

string JWT_KEY = Environment.GetEnvironmentVariable("JWT_KEY")!;
string JWT_ISSUER = Environment.GetEnvironmentVariable("JWT_ISSUER")!;
string JWT_AUDIENCE = Environment.GetEnvironmentVariable("JWT_AUDIENCE")!;

// -------------------- CONNECTION STRINGS --------------------

string connStrDictionary =
    $"Host={DB_HOST_DICTIONARY};" +
    $"Port={DB_PORT_DICTIONARY};" +
    $"Database={DB_NAME_DICTIONARY};" +
    $"Username={DB_USER_DICTIONARY};" +
    $"Password={DB_PASSWORD_DICTIONARY};" +
    $"Pooling=true;";

string connStrAuth =
    $"Host={DB_HOST_AUTH};" +
    $"Port={DB_PORT_AUTH};" +
    $"Database={DB_NAME_AUTH};" +
    $"Username={DB_USER_AUTH};" +
    $"Password={DB_PASSWORD_AUTH};" +
    $"Pooling=true;";

// -------------------- DB --------------------

builder.Services.AddDbContext<DictionaryContext>(options =>
{
    options.UseMySql(
        connStrDictionary,
        ServerVersion.AutoDetect(connStrDictionary));
});

builder.Services.AddDbContext<AuthContext>(options =>
{
    options.UseMySql(
        connStrAuth,
        ServerVersion.AutoDetect(connStrAuth));
});

// -------------------- CORS --------------------

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", policy =>
    {
        policy
            .WithOrigins(
                "http://localhost:8080"
            )
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

// -------------------- AUTH --------------------

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = JWT_ISSUER,
            ValidAudience = JWT_AUDIENCE,

            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(JWT_KEY))
        };
        
        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                Console.WriteLine("AUTH FAILED: " + context.Exception.Message);
                return Task.CompletedTask;
            },
            OnTokenValidated = context =>
            {
                Console.WriteLine("AUTH OK");
                return Task.CompletedTask;
            },
            OnMessageReceived = context =>
            {
                context.Token = context.Request.Cookies["accessToken"];
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});

// -------------------- RATE LIMIT --------------------

builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("api", config =>
    {
        config.PermitLimit = 60;
        config.Window = TimeSpan.FromMinutes(1);
        config.QueueLimit = 0;
    });
});

// -------------------- CONTROLLERS --------------------

builder.Services.AddControllers();

// -------------------- SWAGGER --------------------

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "StaDictionaryApi",
        Version = "v1"
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header"
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

// -------------------- FORWARDED HEADERS --------------------

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders =
        ForwardedHeaders.XForwardedFor |
        ForwardedHeaders.XForwardedProto;

    options.KnownNetworks.Clear();
    options.KnownProxies.Clear();
});

// -------------------- BUILD --------------------

var app = builder.Build();

// -------------------- PIPELINE --------------------

app.UseForwardedHeaders();

app.UseExceptionHandler("/Error");

app.UseHsts();

app.UseSwagger();

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Sta API v1");
});

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors("AllowSpecificOrigin");

app.UseAuthentication();

app.UseAuthorization();

app.UseRateLimiter();

app.MapControllers();

app.Run();