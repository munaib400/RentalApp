using StarterApp.Database.Models;

namespace StarterApp.Repositories;

public interface IItemRepository
{
    Task<List<Item>> GetAllItemsAsync();
    Task<Item?> GetItemByIdAsync(int id);
    Task<List<Item>> GetItemsByOwnerAsync(int ownerId);
    Task<Item> CreateItemAsync(Item item);
    Task<Item> UpdateItemAsync(Item item);
    Task DeleteItemAsync(int id);
}
