using StarterApp.Database.Models;
using StarterApp.Repositories;

namespace StarterApp.Services;

public class RentalService : IRentalService
{
    private readonly IRentalRepository _rentalRepository;
    private readonly IItemRepository _itemRepository;
    private readonly IAuthenticationService _authService;

    public RentalService(IRentalRepository rentalRepository, IItemRepository itemRepository, IAuthenticationService authService)
    {
        _rentalRepository = rentalRepository;
        _itemRepository = itemRepository;
        _authService = authService;
    }

    public async Task<List<Rental>> GetMyRentalsAsync()
    {
        var userId = _authService.CurrentUser?.Id ?? 0;
        return await _rentalRepository.GetRentalsByRenterAsync(userId);
    }

    public async Task<List<Rental>> GetIncomingRequestsAsync()
    {
        var userId = _authService.CurrentUser?.Id ?? 0;
        return await _rentalRepository.GetRentalsByItemOwnerAsync(userId);
    }

    public async Task<Rental> RequestRentalAsync(int itemId, DateTime startDate, DateTime endDate, string? notes)
    {
        var days = (endDate - startDate).Days;
        if (days <= 0) throw new Exception("End date must be after start date");

        var rental = new Rental
        {
            ItemId = itemId,
            RenterId = _authService.CurrentUser?.Id ?? 0,
            StartDate = DateTime.SpecifyKind(startDate, DateTimeKind.Utc),
            EndDate = DateTime.SpecifyKind(endDate, DateTimeKind.Utc),
            TotalCost = days * 5,
            Status = RentalStatus.Requested,
            Notes = notes,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        return await _rentalRepository.CreateRentalAsync(rental);
    }

    public async Task<Rental> ApproveRentalAsync(int rentalId)
    {
        return await _rentalRepository.UpdateRentalStatusAsync(rentalId, RentalStatus.Approved);
    }

    public async Task<Rental> RejectRentalAsync(int rentalId)
    {
        return await _rentalRepository.UpdateRentalStatusAsync(rentalId, RentalStatus.Rejected);
    }
}