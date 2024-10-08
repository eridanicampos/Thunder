using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ProjectTest.Application.AutoMapper;
using ProjectTest.Configuration;
using ProjectTest.Infrastructure.Data.Configurations;
using ProjectTest.Infrastructure.Data.Context;
using ProjectTest.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();


var configuration = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
               .AddEnvironmentVariables()
               .Build();

builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient();
builder.Services.AddInfrastructure(configuration);
builder.Services.AddApiConfig(configuration);
builder.Services.AddAuthorization();
builder.Services.AddSingleton(builder.Configuration);
builder.Services.AddTransient<ExceptionHandlingMiddleware>();

//AutoMapper
builder.Services.AddAutoMapper(typeof(AutoMapperSetup));
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}



app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.UseApiConfig(app.Environment);

app.Run();
