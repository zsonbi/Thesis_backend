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
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            builder.Services.AddCors(options =>
             {
                 options.AddPolicy("AllowEverything", // This is the open house we talked about!
                     builder =>
                     {
                         builder.AllowAnyOrigin() // Any origin is welcome...
                             .AllowAnyHeader() // With any type of headers...
                             .AllowAnyMethod(); // And any HTTP methods. Such a jolly party indeed!
                     });
             });

            builder.Services.AddDbContext<ThesisDbContext>(options =>
            options.UseMySql(connectionStrings[0],

            new MySqlServerVersion(new Version(10, 5, 9))));
            builder.Services.AddHttpsRedirection(options =>
            {
                options.RedirectStatusCode = StatusCodes.Status307TemporaryRedirect;
                options.HttpsPort = 5001;
            });
            var app = builder.Build();

            app.UseHttpsRedirection();

            // Enable CORS globally
            app.UseCors("AllowEverything");
            // Here, you're allowing CORS for an array of specific domains.
            //app.UseCors(builder =>
            //        builder
            //        .WithOrigins("http://thesis.picidolgok.hu",
            //                                              "http://picidolgok.hu",
            //                                             "http://localhost"
            //                                             , "https://zsonbi.github.io")
            //        .AllowAnyMethod()
            //        .AllowAnyHeader());
            // Configure the HTTP request pipeline.
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