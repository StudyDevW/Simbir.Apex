using DotNetEnv;
using DotNetEnv.Configuration;
using Microsoft.EntityFrameworkCore;

namespace DataBaseImplement {
    public class Program {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            // .env read
            builder.Configuration.AddDotNetEnv(".env", LoadOptions.TraversePath());
            // Add database conntect string
            builder.Services.AddDbContext<DataBase>(options => {
                var connectString = builder.Configuration["DATABASE_CONNECT_M"];
                if (connectString != null) { options.UseNpgsql(connectString); }
            });

            var app = builder.Build();

            await app.RunAsync();
        }
    }
}
