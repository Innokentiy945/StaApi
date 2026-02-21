using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using DotNetEnv;
using Microsoft.OpenApi;
using StaApi.Context;

var builder = WebApplication.CreateBuilder(args);

Env.Load();


string dbName = Environment.GetEnvironmentVariable("DB_NAME") 
                ?? throw new Exception("DB_NAME not found");
string dbUser = Environment.GetEnvironmentVariable("DB_USER") 
                ?? throw new Exception("DB_USER not found");
string dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD") 
                    ?? throw new Exception("DB_PASSWORD not found");

//For MariaDB
string dbHost = "127.0.0.1"; 
string dbPort = "3306";      

string connStr = $"Server={dbHost};Port={dbPort};Database={dbName};User ID={dbUser};Password={dbPassword};Charset=utf8;";

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








