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

builder.Services.AddCors(opt =>
    opt.AddDefaultPolicy(policy =>
        policy.WithOrigins("http://localhost:3039").AllowAnyHeader().AllowAnyMethod()
    )
);

builder.Services.AddEndpointsApiExplorer().AddSwaggerGen();

var app = builder.Build();

app.AddEndPoints();
app.UseCors();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await db.Database.MigrateAsync();
}

app.UseHttpsRedirection();

await app.RunAsync();
