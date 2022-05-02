using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAppCosmosDb.Models;

namespace WebAppCosmosDb.Services
{
    public interface ICosmosDbService
    {
        Task<IEnumerable<Item>> GetItemsAsync(string query);
        Task<Item> GetItemAsync(string id);
        Task AddItemAsync(Item item);
        Task UpdateItemAsync(string id, Item item);
        Task DeleteItemAsync(string id);
    }
}