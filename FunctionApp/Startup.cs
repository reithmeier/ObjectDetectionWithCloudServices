using System;
using DAL;
using Logic;
using Logic.Cv;
using Logic.Db;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


[assembly: FunctionsStartup(typeof(FunctionApp.Startup))]

namespace FunctionApp {

  public class Startup : FunctionsStartup {
    public override void Configure(IFunctionsHostBuilder builder) {
      IServiceCollection services = builder.Services;

      IConfigurationRoot config = new ConfigurationBuilder()
        .SetBasePath(Environment.CurrentDirectory)
        .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
        .AddEnvironmentVariables()
        .Build();

      //var connectionString = config.GetConnectionStringOrSetting("VotingDatabase");
      //if (connectionString.StartsWith("InMemory"))
      //  services.AddDbContext<VotingContext>(options => options.UseInMemoryDatabase("voting_db"));
      //else
      //  services.AddDbContext<VotingContext>(options => options.UseSqlServer(connectionString));

      //var cognitiveServiceSubscriptionKey = "358b6b82c8444ae88b063ca04d4e6964";
      //var cognitiveServiceEndpoint = "https://westeurope.api.cognitive.microsoft.com/";
      //var cosmoDbEndpoint = "https://clc3-cosmodb.documents.azure.com:443/"; 
      //var cosmoDbPrimaryKey = "Z3Zb67YQKeK3s5lJVpGt9KeVehyxH0uGUK4bwCyxKsNscCP1mT42byr4VeB1n0slZCL8mV6bj0S4s2qssBUeJA=="; 

      services.AddScoped<IImageLogic, DbImageLogic>();
      services.AddScoped<IComputerVisionLogic, CognitiveComputerVisionLogic>();
      services.AddScoped<IImageRepository, CosmosDbImageRepository>();
    }
  }
}