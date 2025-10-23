using Contracts.BusinessLogicContracts;
using BusinessLogic.Implements;
using Contracts.CacheContracts;
using CacheRedisLogic;
using Contracts.StorageContracts;
using DataBaseImplement.Implements;
using DotNetEnv;
using DotNetEnv.Configuration;
using DataBaseImplement;
using Microsoft.EntityFrameworkCore;

namespace ServiceManagerRestAPI {
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
            // Add services to the container.
            builder.Services.AddSingleton<ISimbirServiceStorage, DbSimbirServiceStorage>();
            builder.Services.AddSingleton<ISimbirServiceCache, RedisSimbirServiceCache>();
            builder.Services.AddSingleton<ISimbirServiceLogic, SimbirServiceLogic>();
            builder.Services.AddSingleton<IAutoMigrationService, AutoMigrationService>();
            builder.Services.AddStackExchangeRedisCache(optinos => {
                optinos.Configuration = "redis_cache";
                optinos.InstanceName = "6380";  
            });
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(s => s.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo {
                Title = "ServiceManagerRestAPI", 
                Version = "v1",
            }));

            builder.Services.AddHealthChecksUI().AddInMemoryStorage();

            APIRoot.GetConfiguration(builder.Configuration);

            var app = builder.Build();

            using (var serviceScope = app.Services.CreateScope()) {
                var migrations = serviceScope.ServiceProvider.GetService<IAutoMigrationService>();

                if (migrations != null)
                    await migrations.EnsureDatabaseInitializedAsync();
            }

            // Configure the HTTP request pipeline.

            app.UseSwagger();
            app.UseSwaggerUI(s => {
                s.SwaggerEndpoint("/swagger/v1/swagger.json", "ServiceManagerRestAPI v1");
                s.RoutePrefix  = "manager-swagger";
            });
            

            //app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();
            app.MapHealthChecksUI();

            app.Use(async (context, next) => {
                if (context.Request.Path == "/") {
                    context.Response.Redirect("/manager-swagger/");
                }
                else {
                    await next();
                }
            });

            await app.RunAsync();
        }
    }
}
