using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StarterApp.Services;

namespace StarterApp.ViewModels;

public partial class CreateItemViewModel : BaseViewModel
{
    private readonly IItemService _itemService;

    [ObservableProperty]
    private string _title = string.Empty;

    [ObservableProperty]
    private string _description = string.Empty;

    [ObservableProperty]
    private string _dailyRate = string.Empty;

    [ObservableProperty]
    private string _category = string.Empty;

    [ObservableProperty]
    private string _location = string.Empty;

    public CreateItemViewModel(IItemService itemService)
    {
        _itemService = itemService;
        Title = "Create Item";
    }

    [RelayCommand]
    public async Task CreateItemAsync()
    {
        if (string.IsNullOrWhiteSpace(Title) || string.IsNullOrWhiteSpace(Description) ||
            string.IsNullOrWhiteSpace(DailyRate) || string.IsNullOrWhiteSpace(Category) ||
            string.IsNullOrWhiteSpace(Location))
        {
            await Shell.Current.DisplayAlert("Error", "Please fill in all fields", "OK");
            return;
        }

        if (!decimal.TryParse(DailyRate, out decimal rate))
        {
            await Shell.Current.DisplayAlert("Error", "Please enter a valid daily rate", "OK");
            return;
        }

        try
        {
            IsBusy = true;
            await _itemService.CreateItemAsync(Title, Description, rate, Category, Location);
            await Shell.Current.GoToAsync("..");
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
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