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

    [ObservableProperty]
    private bool _canEditItem;

    [ObservableProperty]
    private bool _canDeleteItem;

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
                var currentUserId = _authService.CurrentUser?.Id ?? 0;
                CanRequestRental = Item.OwnerId != currentUserId;
                CanEditItem = Item.OwnerId == currentUserId;
                CanDeleteItem = Item.OwnerId == currentUserId;
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

    [RelayCommand]
    private async Task EditItemAsync()
    {
        await Shell.Current.GoToAsync($"EditItemPage?itemId={ItemId}");
    }

    [RelayCommand]
    private async Task DeleteItemAsync()
    {
        var confirm = await Shell.Current.DisplayAlert(
            "Delete Item",
            "Are you sure you want to delete this item?",
            "Yes", "No");

        if (!confirm) return;

        try
        {
            IsBusy = true;
            await _itemService.DeleteItemAsync(ItemId);
            await Shell.Current.DisplayAlert("Success", "Item deleted successfully!", "OK");
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
}