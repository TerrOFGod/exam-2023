using ZeroX.Infrastructure.SignalR.Hubs;

namespace ZeroX.API.Extensions;

public static class WebAppllicationExtension
{
    public static WebApplication AddMiddlewares(this WebApplication app)
    {
        // Configure the HTTP request pipeline.
        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseCors("CorsPolicy");

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.MapHub<GameHub>("/hub");
        return app;
    }
}