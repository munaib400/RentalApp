using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StarterApp.Services;

namespace StarterApp.ViewModels;

[QueryProperty(nameof(ItemId), "itemId")]
public partial class RequestRentalViewModel : BaseViewModel
{
    private readonly IRentalService _rentalService;

    [ObservableProperty]
    private int _itemId;

    [ObservableProperty]
    private DateTime _startDate = DateTime.Now.AddDays(1);

    [ObservableProperty]
    private DateTime _endDate = DateTime.Now.AddDays(2);

    [ObservableProperty]
    private string _notes = string.Empty;

    public RequestRentalViewModel(IRentalService rentalService)
    {
        _rentalService = rentalService;
        Title = "Request Rental";
    }

    [RelayCommand]
    public async Task SubmitRequestAsync()
    {
        if (EndDate <= StartDate)
        {
            await Shell.Current.DisplayAlert("Error", "End date must be after start date", "OK");
            return;
        }

        try
        {
            IsBusy = true;
            await _rentalService.RequestRentalAsync(ItemId, StartDate, EndDate, Notes);
            await Shell.Current.DisplayAlert("Success", "Rental request submitted!", "OK");
            await Shell.Current.GoToAsync("..");
        }
        catch (Exception ex)
        {
            var message = ex.InnerException?.Message ?? ex.Message;
            await Shell.Current.DisplayAlert("Error", message, "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    public async Task CancelAsync()
    {
        await Shell.Current.GoToAsync("..");
    }
}