using CosmosDemo.Domain.Models;
using CosmosDemo.Domain.Repositories;
using CosmosDemo.Persistence.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

//load configurations
var configuration = new ConfigurationBuilder()
     .SetBasePath(Directory.GetCurrentDirectory())
     .AddJsonFile($"appsettings.json");

var config = configuration.Build();
var cosmosEndpoint = config["COSMOS_ENDPOINT"];
var cosmosKey = config["COSMOS_KEY"];

//setup our DI
var serviceProvider = new ServiceCollection()
    .AddSingleton<IUserRepository>(_ => new NaiveUserRepository(cosmosEndpoint, cosmosKey))
    .BuildServiceProvider();

//do the actual work here
var userRepository = serviceProvider.GetService<IUserRepository>()!;

/*
//create user
var newUser = new AppUser
{
    Id = Guid.NewGuid().ToString(),
    Name = "Test User 3",
    Email = "test3@test.com"
};

_ = userRepository.Create(newUser).Result;
*/

var allUsers = userRepository.List().Result;

foreach(var user in allUsers)
{
    Console.WriteLine($"Name: {user.Name}, Email: {user.Email}");
}

Console.WriteLine("--------------");
Console.WriteLine("End of Program");
Console.ReadLine();