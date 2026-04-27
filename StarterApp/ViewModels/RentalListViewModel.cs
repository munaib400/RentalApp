using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StarterApp.Database.Models;
using StarterApp.Services;

namespace StarterApp.ViewModels;

public partial class RentalListViewModel : BaseViewModel
{
    private readonly IRentalService _rentalService;

    [ObservableProperty]
    private List<Rental> _myRentals = new();

    [ObservableProperty]
    private List<Rental> _incomingRequests = new();

    [ObservableProperty]
    private bool _isRefreshing;

    public RentalListViewModel(IRentalService rentalService)
    {
        _rentalService = rentalService;
        Title = "Rentals";
    }

    [RelayCommand]
    public async Task LoadRentalsAsync()
    {
        try
        {
            IsRefreshing = true;
            MyRentals = await _rentalService.GetMyRentalsAsync();
            IncomingRequests = await _rentalService.GetIncomingRequestsAsync();
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
        }
        finally
        {
            IsRefreshing = false;
        }
    }

    [RelayCommand]
    public async Task ApproveRentalAsync(Rental rental)
    {
        try
        {
            await _rentalService.ApproveRentalAsync(rental.Id);
            await LoadRentalsAsync();
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
        }
    }

    [RelayCommand]
    public async Task RejectRentalAsync(Rental rental)
    {
        try
        {
            await _rentalService.RejectRentalAsync(rental.Id);
            await LoadRentalsAsync();
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
        }
    }
}