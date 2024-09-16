using Api.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder
    .AddSwagger()
    .AddDbContext();

var app = builder.Build();

app
    .UseSwaggerInDevelopment()
    .UseHttpsRedirection();

app.MapEndpoints();

await app.RunMigrations();

app.Run();
