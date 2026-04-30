using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StarterApp.Services;

namespace StarterApp.ViewModels;

[QueryProperty(nameof(ItemId), "itemId")]
public partial class EditItemViewModel : BaseViewModel
{
    private readonly IItemService _itemService;

    [ObservableProperty]
    private int _itemId;

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

    public EditItemViewModel(IItemService itemService)
    {
        _itemService = itemService;
        Title = "Edit Item";
    }

    partial void OnItemIdChanged(int value)
    {
        LoadItemAsync(value).ConfigureAwait(false);
    }

    private async Task LoadItemAsync(int id)
    {
        try
        {
            IsBusy = true;
            var item = await _itemService.GetItemByIdAsync(id);
            if (item != null)
            {
                Title = item.Title;
                Description = item.Description;
                DailyRate = item.DailyRate.ToString();
                Category = item.Category;
                Location = item.Location;
            }
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
    public async Task SaveItemAsync()
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
            await _itemService.UpdateItemAsync(ItemId, Title, Description, rate, Category, Location);
            await Shell.Current.DisplayAlert("Success", "Item updated successfully!", "OK");
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