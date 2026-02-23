using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using DotNetEnv;
using Microsoft.OpenApi;
using NSwag;
using StaApi.Context;
using StaApi.Repository;
using OpenApiInfo = Microsoft.OpenApi.Models.OpenApiInfo;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<IDictionarySTA, DictionaryStaService>();

Env.Load();

string DB_HOST = Environment.GetEnvironmentVariable("DB_HOST")
                 ?? throw new Exception("DB_HOST not found");

string DB_PORT = Environment.GetEnvironmentVariable("DB_PORT")
                 ?? throw new Exception("DB_PORT not found");

string DB_NAME = Environment.GetEnvironmentVariable("DB_NAME") 
                 ?? throw new Exception("DB_NAME not found");

string DB_USER = Environment.GetEnvironmentVariable("DB_USER") 
                 ?? throw new Exception("DB_USER not found");

string DB_PASSWORD = Environment.GetEnvironmentVariable("DB_PASSWORD") 
                     ?? throw new Exception("DB_PASSWORD not found");

string connStr = $"Server={DB_HOST};Port={DB_PORT};Database={DB_NAME};User ID={DB_USER};Password={DB_PASSWORD};Charset=utf8;";

//DbContext with MySQL/MariaDB
builder.Services.AddDbContext<DictionaryContext>(options =>
    options.UseMySql(connStr, ServerVersion.AutoDetect(connStr))
);

builder.Services.AddControllersWithViews();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "StaDictionaryApi", Version = "v1" });
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseRouting();
// app.UseCors("AllowSpecificOrigin");
app.UseResponseCaching();
app.UseStaticFiles();

app.UseEndpoints(endpoints =>
{
    endpoints.MapSwagger();
    endpoints.MapControllers();
});
app.UseAuthorization();

app.MapStaticAssets();
app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}").WithStaticAssets();

app.Run();








