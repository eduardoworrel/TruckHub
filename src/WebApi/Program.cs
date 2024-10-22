using Application;
using Infrastructure;
using Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using Presentation;

var builder = WebApplication.CreateBuilder(args);

builder
    .Services.AddSpecificTimeZone()
    .AddDatabase(builder.Configuration)
    .AddRepositories()
    .AddApplication();

builder.Services.AddEndpointsApiExplorer().AddSwaggerGen();

var app = builder.Build();

app.AddEndPoints();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate();
}

app.UseHttpsRedirection();

app.Run();
