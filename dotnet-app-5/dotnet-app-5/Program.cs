using DotnetApp5;
using GherkinHome.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public class Program
{
    public static WebApplication CreateHost(string[] args = null)
    {
        var builder = WebApplication.CreateBuilder(args ?? new string[0]);

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
        builder.Services.AddScoped<IMyService, MyService>();

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", policy =>
            {
                policy
            .WithOrigins(
                "http://angular5.138.2.131.203.nip.io:31787",
                "http://angular5.138.2.131.203.nip.io"
            )
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
            });
        });
        var app = builder.Build();
        app.UseRouting(); 
        app.UseCors("AllowAll");
        app.MapControllers();

        return app;
    }

    public static void Main(string[] args)
    {
        var app = CreateHost(args);
        app.Run();
    }

}