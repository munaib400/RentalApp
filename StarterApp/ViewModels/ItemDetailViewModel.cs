using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StarterApp.Database.Models;
using StarterApp.Services;

namespace StarterApp.ViewModels;

[QueryProperty(nameof(ItemId), "itemId")]
public partial class ItemDetailViewModel : BaseViewModel
{
    private readonly IItemService _itemService;
    private readonly IAuthenticationService _authService;

    [ObservableProperty]
    private int _itemId;

    [ObservableProperty]
    private Item? _item;

    [ObservableProperty]
    private bool _canRequestRental;

    public ItemDetailViewModel(IItemService itemService, IAuthenticationService authService)
    {
        _itemService = itemService;
        _authService = authService;
        Title = "Item Details";
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
            Item = await _itemService.GetItemByIdAsync(id);
            if (Item != null)
            {
                Title = Item.Title;
                CanRequestRental = Item.OwnerId != (_authService.CurrentUser?.Id ?? 0);
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
    private async Task RequestRentalAsync()
    {
        await Shell.Current.GoToAsync($"RequestRentalPage?itemId={ItemId}");
    }
}