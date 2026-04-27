using StarterApp.Database.Models;

namespace StarterApp.Services;

public interface IItemService
{
    Task<List<Item>> GetAllItemsAsync();
    Task<Item?> GetItemByIdAsync(int id);
    Task<List<Item>> GetMyItemsAsync();
    Task<Item> CreateItemAsync(string title, string description, decimal dailyRate, string category, string location);
    Task<Item> UpdateItemAsync(int id, string title, string description, decimal dailyRate, string category, string location);
    Task DeleteItemAsync(int id);
}