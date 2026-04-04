using Microsoft.EntityFrameworkCore;
using DotNetEnv;
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


string connStr = $"Host={DB_HOST};Port={DB_PORT};Database={DB_NAME};Username={DB_USER};Password={DB_PASSWORD};Pooling=true;";

builder.Services.AddDbContext<DictionaryContext>(options =>
    options.UseNpgsql(connStr)
);

builder.Services.AddControllersWithViews();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "StaDictionaryApi", Version = "v1" });
});

var app = builder.Build();

// if (!app.Environment.IsDevelopment())
// {
//     
// }
app.UseExceptionHandler("/Error");
app.UseHsts();

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

app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();








