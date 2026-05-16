using Microsoft.EntityFrameworkCore;
using DotNetEnv;
using StaApi.Context;
using StaApi.Repository;
using OpenApiInfo = Microsoft.OpenApi.Models.OpenApiInfo;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.RateLimiting;
using OpenIddict.Validation.AspNetCore;
using StaApi.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<IDictionarySTA, DictionaryStaService>();

Env.Load();

string DB_HOST_DICTIONARY = Environment.GetEnvironmentVariable("DB_HOST") ?? throw new Exception("DB_HOST not found");
string DB_PORT_DICTIONARY = Environment.GetEnvironmentVariable("DB_PORT") ?? throw new Exception("DB_PORT not found");
string DB_NAME_DICTIONARY = Environment.GetEnvironmentVariable("DB_NAME") ?? throw new Exception("DB_NAME not found");
string DB_USER_DICTIONARY = Environment.GetEnvironmentVariable("DB_USER") ?? throw new Exception("DB_USER not found");
string DB_PASSWORD_DICTIONARY = Environment.GetEnvironmentVariable("DB_PASSWORD") ?? throw new Exception("DB_PASSWORD not found");

string connStrDictionary = $"Host={DB_HOST_DICTIONARY};Port={DB_PORT_DICTIONARY};Database={DB_NAME_DICTIONARY};Username={DB_USER_DICTIONARY};Password={DB_PASSWORD_DICTIONARY};Pooling=true;";

string DB_HOST_AUTH = Environment.GetEnvironmentVariable("DB_HOST") ?? throw new Exception("DB_HOST not found");
string DB_PORT_AUTH = Environment.GetEnvironmentVariable("DB_PORT") ?? throw new Exception("DB_PORT not found");
string DB_NAME_AUTH = Environment.GetEnvironmentVariable("DB_NAME") ?? throw new Exception("DB_NAME not found");
string DB_USER_AUTH = Environment.GetEnvironmentVariable("DB_USER") ?? throw new Exception("DB_USER not found");
string DB_PASSWORD_AUTH = Environment.GetEnvironmentVariable("DB_PASSWORD") ?? throw new Exception("DB_PASSWORD not found");

string connStrAuth = $"Host={DB_HOST_AUTH};Port={DB_PORT_AUTH};Database={DB_NAME_AUTH};Username={DB_USER_AUTH};Password={DB_PASSWORD_AUTH};Pooling=true;";

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        policy => policy.WithOrigins("http://localhost:5175") // TESTING FOR WEB CLIENT!
                        .AllowAnyHeader()
                        .AllowAnyMethod());
});

builder.Services.AddDbContext<DictionaryContext>(options =>
{
    options.UseMySql(connStrDictionary, ServerVersion.AutoDetect(connStrDictionary));
});

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseMySql(connStrAuth, ServerVersion.AutoDetect(connStrAuth));
    options.UseOpenIddict();
});

builder.Services.AddOpenIddict().AddCore(options =>
    {
        options.UseEntityFrameworkCore()
            .UseDbContext<AppDbContext>();
    })

    .AddServer(options =>
    {
        options.SetTokenEndpointUris("/connect/token");

        options.AllowPasswordFlow();
        options.AllowRefreshTokenFlow();

        options.AcceptAnonymousClients();

        // DEV ONLY
        options.AddDevelopmentEncryptionCertificate();

        options.AddDevelopmentSigningCertificate();

        options.UseAspNetCore()
            .EnableTokenEndpointPassthrough();

        options.DisableAccessTokenEncryption();

        options.SetAccessTokenLifetime(
            TimeSpan.FromMinutes(15));

        options.SetRefreshTokenLifetime(
            TimeSpan.FromDays(30));
    })

    .AddValidation(options =>
    {
        options.UseLocalServer();

        options.UseAspNetCore();
    });

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme =
        OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<AuditService>();
builder.Services.AddAuthorization();
builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("api", config =>
    {
        config.PermitLimit = 60;

        config.Window = TimeSpan.FromMinutes(1);

        config.QueueLimit = 0;
    });
});


builder.Services.AddControllersWithViews();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "StaDictionaryApi", Version = "v1" });
});

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders =
        ForwardedHeaders.XForwardedFor |
        ForwardedHeaders.XForwardedProto;

    options.KnownNetworks.Clear();
    options.KnownProxies.Clear();
});

var app = builder.Build();

app.UseExceptionHandler("/Error");
app.UseHsts();
app.UseSwagger();
app.UseSwaggerUI();
app.UseRouting();
app.UseResponseCaching();
app.UseStaticFiles();
app.UseCors("AllowSpecificOrigin");
app.UseAuthorization();
app.Run();