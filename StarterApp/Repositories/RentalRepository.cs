using Microsoft.EntityFrameworkCore;
using StarterApp.Database.Data;
using StarterApp.Database.Models;

namespace StarterApp.Repositories;

public class RentalRepository : IRentalRepository
{
    private readonly IDbContextFactory<AppDbContext> _contextFactory;

    public RentalRepository(IDbContextFactory<AppDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task<List<Rental>> GetRentalsByRenterAsync(int renterId)
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        return await context.Rentals
            .Include(r => r.Item)
            .Include(r => r.Renter)
            .Where(r => r.RenterId == renterId)
            .ToListAsync();
    }

    public async Task<List<Rental>> GetRentalsByItemOwnerAsync(int ownerId)
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        return await context.Rentals
            .Include(r => r.Item)
            .Include(r => r.Renter)
            .Where(r => r.Item!.OwnerId == ownerId)
            .ToListAsync();
    }

    public async Task<Rental?> GetRentalByIdAsync(int id)
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        return await context.Rentals
            .Include(r => r.Item)
            .Include(r => r.Renter)
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<Rental> CreateRentalAsync(Rental rental)
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        context.Entry(rental).State = Microsoft.EntityFrameworkCore.EntityState.Added;
        await context.SaveChangesAsync();
        return rental;
    }

    public async Task<Rental> UpdateRentalStatusAsync(int id, string status)
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        var rental = await context.Rentals.FindAsync(id);
        if (rental == null) throw new Exception("Rental not found");
        rental.Status = status;
        rental.UpdatedAt = DateTime.UtcNow;
        context.Rentals.Update(rental);
        await context.SaveChangesAsync();
        return rental;
    }
}