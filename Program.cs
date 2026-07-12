using System.Text.Encodings.Web;
using Microsoft.EntityFrameworkCore;
using DotNetEnv;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.OpenApi.Models;
using StaApi.AutoGeneration.Context;
using StaApi.AutoGeneration.Patterns.PastPatterns;
using StaApi.AutoGeneration.Patterns.PresentPatterns;
using StaApi.AutoGeneration.Patterns.Registry;
using StaApi.AutoGeneration.Service;
using StaApi.AutoGeneration.Service.Generators;
using StaApi.AutoGeneration.Validator;
using StaApi.Context;
using StaApi.Repository.Dictionary;

var builder = WebApplication.CreateBuilder(args);

// -------------------- SERVICES --------------------

builder.Services.AddScoped<IDictionarySTA, DictionaryStaService>();

builder.Services.AddScoped<ExerciseGenerationService>();
builder.Services.AddScoped<ExerciseValidator>();
builder.Services.AddScoped<SlotBasedGeneratorPresent>();
builder.Services.AddScoped<PresentPatternRegistry>();
builder.Services.AddScoped<PastPatternRegistry>();
builder.Services.AddScoped<PresentPositivePatterns>();
builder.Services.AddScoped<PresentNegativePatterns>();
builder.Services.AddScoped<PastPositivePatterns>();


builder.Services.AddHttpContextAccessor();

Env.Load();

// -------------------- ENV --------------------
string DB_HOST_CORE = Environment.GetEnvironmentVariable("DB_HOST_CORE")!;
string DB_PORT_CORE = Environment.GetEnvironmentVariable("DB_PORT_CORE")!;
string DB_NAME_CORE = Environment.GetEnvironmentVariable("DB_NAME_CORE")!;
string DB_USER_CORE = Environment.GetEnvironmentVariable("DB_USER_CORE")!;
string DB_PASSWORD_CORE = Environment.GetEnvironmentVariable("DB_PASSWORD_CORE")!;

string DB_HOST_DICTIONARY = Environment.GetEnvironmentVariable("DB_HOST_DICTIONARY")!;
string DB_PORT_DICTIONARY = Environment.GetEnvironmentVariable("DB_PORT_DICTIONARY")!;
string DB_NAME_DICTIONARY = Environment.GetEnvironmentVariable("DB_NAME_DICTIONARY")!;
string DB_USER_DICTIONARY = Environment.GetEnvironmentVariable("DB_USER_DICTIONARY")!;
string DB_PASSWORD_DICTIONARY = Environment.GetEnvironmentVariable("DB_PASSWORD_DICTIONARY")!;

// -------------------- CONNECTION STRINGS --------------------
string connStrCore =
    $"Host={DB_HOST_CORE};" +
    $"Port={DB_PORT_CORE};" +
    $"Database={DB_NAME_CORE};" +
    $"Username={DB_USER_CORE};" +
    $"Password={DB_PASSWORD_CORE};" +
    $"Protocol=Tcp;" +
    $"Pooling=true;";

string connStrDictionary =
    $"Host={DB_HOST_DICTIONARY};" +
    $"Port={DB_PORT_DICTIONARY};" +
    $"Database={DB_NAME_DICTIONARY};" +
    $"Username={DB_USER_DICTIONARY};" +
    $"Password={DB_PASSWORD_DICTIONARY};" +
    $"Protocol=Tcp;" +
    $"Pooling=true;";

// -------------------- DB --------------------

builder.Services.AddDbContext<CoreContext>(options =>
{
    options.UseMySql(connStrCore, ServerVersion.AutoDetect(connStrCore));
});

builder.Services.AddDbContext<DictionaryContext>(options =>
{
    options.UseMySql(connStrDictionary, ServerVersion.AutoDetect(connStrDictionary));
});

// -------------------- CORS --------------------

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", policy =>
    {
        policy.WithOrigins(
                "http://localhost:5175"
            )
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
});

// -------------------- AUTH --------------------


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
});

// -------------------- FORWARDED HEADERS --------------------

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