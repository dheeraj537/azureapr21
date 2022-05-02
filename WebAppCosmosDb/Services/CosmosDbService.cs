using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAppCosmosDb.Models;

namespace WebAppCosmosDb.Services
{
    public class CosmosDbService : ICosmosDbService
    {
        Container container;

        public CosmosDbService(CosmosClient dbClient,
            string databaseName, string containerName)
        {
            container = dbClient.GetContainer(databaseName, containerName);
        }
        public async Task AddItemAsync(Item item)
        {
            await container.CreateItemAsync(item, new PartitionKey(item.Id));
        }

        public async Task DeleteItemAsync(string id)
        {
            await container.DeleteItemAsync<Item>(id, new PartitionKey(id));
        }

        public Task<Item> GetItemAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Item>> GetItemsAsync(string query)
        {
            var query1 = container.GetItemQueryIterator<Item>(
                new QueryDefinition(query));
            List<Item> results = new List<Item>();
            while (query1.HasMoreResults)
            {
                var response = await query1.ReadNextAsync();
                results.AddRange(response.ToList());
            }
            return results;
        }

        public async Task UpdateItemAsync(string id, Item item)
        {
            await container.UpsertItemAsync<Item>(item, new PartitionKey(id));
        }
    }
}