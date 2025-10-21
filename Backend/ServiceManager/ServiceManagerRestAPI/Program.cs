using Contracts.BusinessLogicContracts;
using BusinessLogic.Implements;
using Contracts.CacheContracts;
using CacheRedisLogic;
using Contracts.StorageContracts;
using DataBaseImplement.Implements;

namespace ServiceManagerRestAPI {
    public class Program {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddTransient<ISimbirServiceStorage, DbSimbirServiceStorage>();
            builder.Services.AddSingleton<ISimbirServiceCache, RedisSimbirServiceCache>();
            builder.Services.AddTransient<ISimbirServiceLogic, SimbirServiceLogic>();
            builder.Services.AddStackExchangeRedisCache(optinos => {
                optinos.Configuration = "localhost";
                optinos.InstanceName = "local";
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

            // Configure the HTTP request pipeline.
          
            app.UseSwagger();
            app.UseSwaggerUI(s => {
                s.SwaggerEndpoint("/swagger/v1/swagger.json", "ServiceManagerRestAPI v1");
                s.RoutePrefix  = "manager-swagger";
            });
            

         //   app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();
            app.MapHealthChecksUI();

            app.Run();
        }
    }
}
