namespace MyProject.Extensions.Midleware;

public static class ConfigureCustomException
{
    public static void ConfigureCustomExceptionMiddleware(this IApplicationBuilder app)
    {
        app.UseMiddleware<ExceptionMiddleware>();
    }
}