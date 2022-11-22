using CosmosDemo.Domain.Models;
using CosmosDemo.Domain.Repositories;
using Microsoft.Azure.Cosmos;

namespace CosmosDemo.Persistence.Repositories
{
    public class NaiveUserRepository : IUserRepository
    {
        private string _cosmosEndpoint;
        private string _cosmosKey;

        public NaiveUserRepository(string cosmosEndpoint, string cosmosKey)
        {
            _cosmosEndpoint = cosmosEndpoint;
            _cosmosKey = cosmosKey;
        }

        public async Task<AppUser> Create(AppUser appUser)
        {
            // New instance of CosmosClient class
            using CosmosClient client = new(
                accountEndpoint: _cosmosEndpoint,
                authKeyOrResourceToken: _cosmosKey!
            );

            Database database = await client.CreateDatabaseIfNotExistsAsync(
                                id: "dotnet20"
                            );

            // Container reference with creation if it does not alredy exist
            Container container = await database.CreateContainerIfNotExistsAsync(
                id: "users",
                partitionKeyPath: "/id"
            );

            return await container.UpsertItemAsync(
                item: appUser
            );
        }

        public async Task<AppUser?> GetByEmail(string email)
        {
            // New instance of CosmosClient class
            using CosmosClient client = new(
                accountEndpoint: _cosmosEndpoint,
                authKeyOrResourceToken: _cosmosKey!
            );

            Database database = await client.CreateDatabaseIfNotExistsAsync(
                                id: "dotnet20"
                            );

            // Container reference with creation if it does not alredy exist
            Container container = await database.CreateContainerIfNotExistsAsync(
                id: "users",
                partitionKeyPath: "/id"
            );

            // Create query using a SQL string and parameters
            var query = new QueryDefinition(
                query: "SELECT * FROM users u WHERE u.email = @email"
            )
                .WithParameter("@email", email);
            using FeedIterator<AppUser> feed = container.GetItemQueryIterator<AppUser>(
                queryDefinition: query
            );

            if (feed.HasMoreResults)
            {
                FeedResponse<AppUser> response = await feed.ReadNextAsync();
                return response.Count > 0 ? response.First() : null;
            }
            return null;
        }

        public async Task<AppUser> GetById(string id)
        {
            // New instance of CosmosClient class
            using CosmosClient client = new(
                accountEndpoint: _cosmosEndpoint,
                authKeyOrResourceToken: _cosmosKey!
            );

            Database database = await client.CreateDatabaseIfNotExistsAsync(
                                id: "dotnet20"
                            );

            // Container reference with creation if it does not alredy exist
            Container container = await database.CreateContainerIfNotExistsAsync(
                id: "users",
                partitionKeyPath: "/id"
            );

            // Point read item from container using the id and partitionKey
            return await container.ReadItemAsync<AppUser>(
                id: id,
                partitionKey: new PartitionKey(id)
            );
        }

        public async Task<IEnumerable<AppUser>> List()
        {
            // New instance of CosmosClient class
            using CosmosClient client = new(
                accountEndpoint: _cosmosEndpoint,
                authKeyOrResourceToken: _cosmosKey!
            );

            Database database = await client.CreateDatabaseIfNotExistsAsync(
                                id: "dotnet20"
                            );

            // Container reference with creation if it does not alredy exist
            Container container = await database.CreateContainerIfNotExistsAsync(
                id: "users",
                partitionKeyPath: "/id"
            );

            // Create query using a SQL string and parameters
            var query = new QueryDefinition(
                query: "SELECT * FROM users u"
            );
            using FeedIterator<AppUser> feed = container.GetItemQueryIterator<AppUser>(
                queryDefinition: query
            );

            if (feed.HasMoreResults)
            {
                FeedResponse<AppUser> response = await feed.ReadNextAsync();
                return response;
            }
            return Enumerable.Empty<AppUser>();
        }
    }
}
