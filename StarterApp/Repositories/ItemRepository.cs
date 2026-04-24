using Microsoft.EntityFrameworkCore;
using StarterApp.Database.Data;
using StarterApp.Database.Models;

namespace StarterApp.Repositories;

public class ItemRepository : IItemRepository
{
    private readonly AppDbContext _context;

    public ItemRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Item>> GetAllItemsAsync()
    {
        return await _context.Items
            .Include(i => i.Owner)
            .Where(i => i.IsAvailable)
            .ToListAsync();
    }

    public async Task<Item?> GetItemByIdAsync(int id)
    {
        return await _context.Items
            .Include(i => i.Owner)
            .FirstOrDefaultAsync(i => i.Id == id);
    }

    public async Task<List<Item>> GetItemsByOwnerAsync(int ownerId)
    {
        return await _context.Items
            .Where(i => i.OwnerId == ownerId)
            .ToListAsync();
    }

    public async Task<Item> CreateItemAsync(Item item)
    {
        _context.Items.Add(item);
        await _context.SaveChangesAsync();
        return item;
    }

    public async Task<Item> UpdateItemAsync(Item item)
    {
        item.UpdatedAt = DateTime.UtcNow;
        _context.Items.Update(item);
        await _context.SaveChangesAsync();
        return item;
    }

    public async Task DeleteItemAsync(int id)
    {
        var item = await _context.Items.FindAsync(id);
        if (item != null)
        {
            _context.Items.Remove(item);
            await _context.SaveChangesAsync();
        }
    }
}
