using Thesis_backend.Data_Structures;
using Microsoft.EntityFrameworkCore;

namespace Thesis_backend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

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
                         builder.WithOrigins("https://zsonbi.github.io/Thesis/game/", "https://zsonbi.github.io", "https://thesis.picidolgok.hu", "http://thesis.picidolgok.hu")
                             .AllowAnyHeader() // With any type of headers...
                             .AllowAnyMethod() // And any HTTP methods. Such a jolly party indeed!
                             .AllowCredentials();
                     });
             });

            builder.Services.AddDbContext<ThesisDbContext>(options =>
            options.UseMySql(connectionStrings[0],

            new MySqlServerVersion(new Version(10, 5, 9))));
            builder.Services.AddHttpsRedirection(options =>
            {
                options.RedirectStatusCode = StatusCodes.Status307TemporaryRedirect;
                options.HttpsPort = 8001;
            });
            var app = builder.Build();

            // Enable CORS globally
            app.UseCors("AllowTrusted");

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.UseSession();

            app.Run();
        }
    }
}