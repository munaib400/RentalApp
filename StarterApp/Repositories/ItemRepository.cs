using Microsoft.EntityFrameworkCore;
using StarterApp.Database.Data;
using StarterApp.Database.Models;

namespace StarterApp.Repositories;

public class ItemRepository : IItemRepository
{
    private readonly IDbContextFactory<AppDbContext> _contextFactory;

    public ItemRepository(IDbContextFactory<AppDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task<List<Item>> GetAllItemsAsync()
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        return await context.Items
            .Include(i => i.Owner)
            .Where(i => i.IsAvailable)
            .ToListAsync();
    }

    public async Task<Item?> GetItemByIdAsync(int id)
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        return await context.Items
            .Include(i => i.Owner)
            .FirstOrDefaultAsync(i => i.Id == id);
    }

    public async Task<List<Item>> GetItemsByOwnerAsync(int ownerId)
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        return await context.Items
            .Where(i => i.OwnerId == ownerId)
            .ToListAsync();
    }

    public async Task<Item> CreateItemAsync(Item item)
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        context.Items.Add(item);
        await context.SaveChangesAsync();
        return item;
    }

    public async Task<Item> UpdateItemAsync(Item item)
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        item.UpdatedAt = DateTime.UtcNow;
        context.Items.Update(item);
        await context.SaveChangesAsync();
        return item;
    }

    public async Task DeleteItemAsync(int id)
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        var item = await context.Items.FindAsync(id);
        if (item != null)
        {
            context.Items.Remove(item);
            await context.SaveChangesAsync();
        }
    }
}