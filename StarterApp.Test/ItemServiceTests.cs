using Microsoft.EntityFrameworkCore;
using StarterApp.Database.Data;
using StarterApp.Database.Models;
using Xunit;

namespace StarterApp.Test;

public class ItemRepositoryTests
{
    private AppDbContext CreateInMemoryContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        return new AppDbContext(options);
    }

    [Fact]
    public async Task AddItem_SavesItemToDatabase()
    {
        using var context = CreateInMemoryContext();
        var item = new Item { Title = "Power Drill", Description = "A drill", DailyRate = 5.99m, Category = "Tools", Location = "Edinburgh", OwnerId = 1, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow };
        context.Items.Add(item);
        await context.SaveChangesAsync();
        Assert.Equal(1, await context.Items.CountAsync());
    }

    [Fact]
    public async Task GetItems_ReturnsAllItems()
    {
        using var context = CreateInMemoryContext();
        context.Items.AddRange(
            new Item { Title = "Drill", Description = "A drill", DailyRate = 5.99m, Category = "Tools", Location = "Edinburgh", OwnerId = 1, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new Item { Title = "Tent", Description = "A tent", DailyRate = 10.00m, Category = "Camping", Location = "Glasgow", OwnerId = 2, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow }
        );
        await context.SaveChangesAsync();
        var items = await context.Items.ToListAsync();
        Assert.Equal(2, items.Count);
    }

    [Fact]
    public async Task GetItemById_ReturnsCorrectItem()
    {
        using var context = CreateInMemoryContext();
        var item = new Item { Title = "Drill", Description = "A drill", DailyRate = 5.99m, Category = "Tools", Location = "Edinburgh", OwnerId = 1, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow };
        context.Items.Add(item);
        await context.SaveChangesAsync();
        var result = await context.Items.FindAsync(item.Id);
        Assert.NotNull(result);
        Assert.Equal("Drill", result.Title);
    }

    [Fact]
    public async Task UpdateItem_UpdatesItemInDatabase()
    {
        using var context = CreateInMemoryContext();
        var item = new Item { Title = "Drill", Description = "A drill", DailyRate = 5.99m, Category = "Tools", Location = "Edinburgh", OwnerId = 1, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow };
        context.Items.Add(item);
        await context.SaveChangesAsync();
        item.Title = "Updated Drill";
        context.Items.Update(item);
        await context.SaveChangesAsync();
        var updated = await context.Items.FindAsync(item.Id);
        Assert.Equal("Updated Drill", updated!.Title);
    }

    [Fact]
    public async Task DeleteItem_RemovesItemFromDatabase()
    {
        using var context = CreateInMemoryContext();
        var item = new Item { Title = "Drill", Description = "A drill", DailyRate = 5.99m, Category = "Tools", Location = "Edinburgh", OwnerId = 1, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow };
        context.Items.Add(item);
        await context.SaveChangesAsync();
        context.Items.Remove(item);
        await context.SaveChangesAsync();
        Assert.Equal(0, await context.Items.CountAsync());
    }

    [Fact]
    public async Task AddRental_SavesRentalToDatabase()
    {
        using var context = CreateInMemoryContext();
        var rental = new Rental { ItemId = 1, RenterId = 2, StartDate = DateTime.UtcNow, EndDate = DateTime.UtcNow.AddDays(1), TotalCost = 5.99m, Status = RentalStatus.Requested, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow };
        context.Rentals.Add(rental);
        await context.SaveChangesAsync();
        Assert.Equal(1, await context.Rentals.CountAsync());
    }

    [Fact]
    public async Task AddRental_WithCorrectStatus_ReturnsRequested()
    {
        using var context = CreateInMemoryContext();
        var rental = new Rental { ItemId = 1, RenterId = 2, StartDate = DateTime.UtcNow, EndDate = DateTime.UtcNow.AddDays(3), TotalCost = 17.97m, Status = RentalStatus.Requested, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow };
        context.Rentals.Add(rental);
        await context.SaveChangesAsync();
        var result = await context.Rentals.FindAsync(rental.Id);
        Assert.Equal(RentalStatus.Requested, result!.Status);
    }

    [Fact]
    public async Task UpdateRentalStatus_ChangesStatusCorrectly()
    {
        using var context = CreateInMemoryContext();
        var rental = new Rental { ItemId = 1, RenterId = 2, StartDate = DateTime.UtcNow, EndDate = DateTime.UtcNow.AddDays(1), TotalCost = 5.99m, Status = RentalStatus.Requested, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow };
        context.Rentals.Add(rental);
        await context.SaveChangesAsync();
        rental.Status = RentalStatus.Approved;
        context.Rentals.Update(rental);
        await context.SaveChangesAsync();
        var result = await context.Rentals.FindAsync(rental.Id);
        Assert.Equal(RentalStatus.Approved, result!.Status);
    }

    [Fact]
    public async Task GetRentalsByRenter_ReturnsCorrectRentals()
    {
        using var context = CreateInMemoryContext();
        context.Rentals.AddRange(
            new Rental { ItemId = 1, RenterId = 1, StartDate = DateTime.UtcNow, EndDate = DateTime.UtcNow.AddDays(1), TotalCost = 5.99m, Status = RentalStatus.Requested, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new Rental { ItemId = 1, RenterId = 2, StartDate = DateTime.UtcNow, EndDate = DateTime.UtcNow.AddDays(1), TotalCost = 5.99m, Status = RentalStatus.Requested, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow }
        );
        await context.SaveChangesAsync();
        var rentals = await context.Rentals.Where(r => r.RenterId == 1).ToListAsync();
        Assert.Single(rentals);
    }

    [Fact]
    public async Task Item_DailyRate_CalculatesTotalCostCorrectly()
    {
        using var context = CreateInMemoryContext();
        var item = new Item { Title = "Drill", Description = "A drill", DailyRate = 5.99m, Category = "Tools", Location = "Edinburgh", OwnerId = 1, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow };
        context.Items.Add(item);
        await context.SaveChangesAsync();
        var days = 3;
        var totalCost = item.DailyRate * days;
        Assert.Equal(17.97m, totalCost);
    }

    [Fact]
    public async Task Item_IsAvailable_DefaultsToTrue()
    {
        using var context = CreateInMemoryContext();
        var item = new Item { Title = "Drill", Description = "A drill", DailyRate = 5.99m, Category = "Tools", Location = "Edinburgh", OwnerId = 1, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow };
        context.Items.Add(item);
        await context.SaveChangesAsync();
        var result = await context.Items.FindAsync(item.Id);
        Assert.True(result!.IsAvailable);
    }
}
