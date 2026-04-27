using StarterApp.Database.Models;
using StarterApp.Repositories;

namespace StarterApp.Services;

public class ItemService : IItemService
{
    private readonly IItemRepository _itemRepository;
    private readonly IAuthenticationService _authService;

    public ItemService(IItemRepository itemRepository, IAuthenticationService authService)
    {
        _itemRepository = itemRepository;
        _authService = authService;
    }

    public async Task<List<Item>> GetAllItemsAsync()
    {
        return await _itemRepository.GetAllItemsAsync();
    }

    public async Task<Item?> GetItemByIdAsync(int id)
    {
        return await _itemRepository.GetItemByIdAsync(id);
    }

    public async Task<List<Item>> GetMyItemsAsync()
    {
        var userId = _authService.CurrentUser?.Id ?? 0;
        return await _itemRepository.GetItemsByOwnerAsync(userId);
    }

    public async Task<Item> CreateItemAsync(string title, string description, decimal dailyRate, string category, string location)
    {
        var item = new Item
        {
            Title = title,
            Description = description,
            DailyRate = dailyRate,
            Category = category,
            Location = location,
            OwnerId = _authService.CurrentUser?.Id ?? 0,
            IsAvailable = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        return await _itemRepository.CreateItemAsync(item);
    }

    public async Task<Item> UpdateItemAsync(int id, string title, string description, decimal dailyRate, string category, string location)
    {
        var item = await _itemRepository.GetItemByIdAsync(id);
        if (item == null) throw new Exception("Item not found");

        item.Title = title;
        item.Description = description;
        item.DailyRate = dailyRate;
        item.Category = category;
        item.Location = location;

        return await _itemRepository.UpdateItemAsync(item);
    }

    public async Task DeleteItemAsync(int id)
    {
        await _itemRepository.DeleteItemAsync(id);
    }
}