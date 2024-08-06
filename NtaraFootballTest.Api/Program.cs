using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.EntityFrameworkCore;
using NtaraFootballTest.Api.Database;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddFastEndpoints().SwaggerDocument();

builder.Services.AddDbContext<ApplicationDbContext>(
    o => o.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"))
);

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseDefaultExceptionHandler().UseFastEndpoints().UseSwaggerGen();

app.MapFallbackToFile("/index.html");

app.Run();
