using CosmosDemo.Domain.Repositories;
using CosmosDemo.Persistence.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace CosmosDemo.MAUI
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            //load configurations
            var a = Assembly.GetExecutingAssembly();
            using var stream = a.GetManifestResourceStream("CosmosDemo.MAUI.appsettings.json");

            var config = new ConfigurationBuilder()
                        .AddJsonStream(stream)
                        .Build();

            var cosmosEndpoint = config["COSMOS_ENDPOINT"];
            var cosmosKey = config["COSMOS_KEY"];

            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services
                .AddTransient<MainPage>()
                .AddSingleton<IUserRepository>(_ => new NaiveUserRepository(cosmosEndpoint, cosmosKey));

            return builder.Build();
        }
    }
}