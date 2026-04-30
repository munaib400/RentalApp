using StarterApp.Database.Models;

namespace StarterApp.Services;

public interface IRentalService
{
    Task<List<Rental>> GetMyRentalsAsync();
    Task<List<Rental>> GetIncomingRequestsAsync();
    Task<Rental> RequestRentalAsync(int itemId, DateTime startDate, DateTime endDate, string? notes);
    Task<Rental> ApproveRentalAsync(int rentalId);
    Task<Rental> RejectRentalAsync(int rentalId);
}