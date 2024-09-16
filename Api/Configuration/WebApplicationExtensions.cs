using Api.Endpoints.Doctors;
using Api.Endpoints.Patients;
using Microsoft.EntityFrameworkCore;

namespace Api.Configuration;

public static class WebApplicationExtensions
{
    public static WebApplication UseSwaggerInDevelopment(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        return app;
    }

    public static WebApplication MapEndpoints(this WebApplication app)
    {
        app.MapDoctorsEndpoints();
        app.MapPatientsEndpoints();

        return app;
    }

    public static async Task RunMigrations(this WebApplication app)
    {
        await using var scope = app.Services.CreateAsyncScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
        await context.Database.MigrateAsync();
    }
}
