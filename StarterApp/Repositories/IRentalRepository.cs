using StarterApp.Database.Models;

namespace StarterApp.Repositories;

public interface IRentalRepository
{
    Task<List<Rental>> GetRentalsByRenterAsync(int renterId);
    Task<List<Rental>> GetRentalsByItemOwnerAsync(int ownerId);
    Task<Rental?> GetRentalByIdAsync(int id);
    Task<Rental> CreateRentalAsync(Rental rental);
    Task<Rental> UpdateRentalStatusAsync(int id, string status);
}