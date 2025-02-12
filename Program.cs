using Thesis_backend.Data_Structures;
using Microsoft.EntityFrameworkCore;
using System;
using Serilog;
using Serilog.AspNetCore;
using Serilog.Debugging;
using Serilog.Sinks.Graylog;
using Microsoft.AspNetCore;
namespace Thesis_backend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            string domain = Environment.GetEnvironmentVariable("DOMAIN") ?? "localhost";
            string graylog_ip = Environment.GetEnvironmentVariable("GRAYLOG_IP") ?? "none";
            int graylog_port=0;
            int.TryParse((Environment.GetEnvironmentVariable("GRAYLOG_PORT")) ?? "0",out graylog_port);
            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var connectionStrings = new List<string>()
            {
                builder.Configuration.GetConnectionString("ThesisDbContext") ?? "",
            };

            builder.Services.AddDistributedMemoryCache();

            builder.Services.AddSession(options =>
            {
                options.Cookie.Name = "loggedInUserId";
                options.IdleTimeout = TimeSpan.FromSeconds(3600);
                options.Cookie.HttpOnly = false;
                options.Cookie.IsEssential = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.Cookie.SameSite = SameSiteMode.None;
            });

            builder.Services.AddCors(options =>
             {
                 options.AddPolicy("AllowTrusted", // This is the open house we talked about!
                     builder =>
                     {
                         builder.WithOrigins("https://zsonbi.github.io/Thesis/game/", "https://zsonbi.github.io", $"https://{domain}", $"http://{domain}")
                             .AllowAnyHeader() // With any type of headers...
                             .AllowAnyMethod() // And any HTTP methods. Such a jolly party indeed!
                             .AllowCredentials()
                             .SetIsOriginAllowedToAllowWildcardSubdomains()
                             .WithExposedHeaders("Set-Cookie", "Content-Type", "Authorization", "X-Requested-With");
                     });
             });


            Console.WriteLine(graylog_ip);
            Console.WriteLine(graylog_port);
            // Configure Serilog with Graylog only in production
            if (graylog_ip != "none" && graylog_port != 0)
            {
                Log.Logger = new LoggerConfiguration()
                   .MinimumLevel.Warning() // Set the minimum log level
                   .WriteTo.Console()
                   .WriteTo.Graylog(new GraylogSinkOptions
                   {
                       HostnameOrAddress = graylog_ip, // Replace with your Graylog server address
                       Port = graylog_port, // Default GELF UDP port
                       TransportType = Serilog.Sinks.Graylog.Core.Transport.TransportType.Tcp,
                       HostnameOverride="thesis_backend",

                   })
                   .CreateLogger();

                builder.Host.UseSerilog();
            }

            builder.Services.AddDbContext<ThesisDbContext>(options =>
            options.UseMySql(connectionStrings[0],

            new MySqlServerVersion(new Version(10, 5, 9))));
            if (domain != "localhost")
            {
                builder.Services.AddHttpsRedirection(options =>
                {
                    options.RedirectStatusCode = StatusCodes.Status307TemporaryRedirect;
                    options.HttpsPort = 8001;
                });
            }
            var app = builder.Build();

            // Enable CORS globally
            app.UseCors("AllowTrusted");


            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseSession();
            if (domain != "localhost")
            {
                app.UseHttpsRedirection();
            }

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}