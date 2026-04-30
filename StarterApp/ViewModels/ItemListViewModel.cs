using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StarterApp.Database.Models;
using StarterApp.Services;

namespace StarterApp.ViewModels;

public partial class ItemListViewModel : BaseViewModel
{
    private readonly IItemService _itemService;

    [ObservableProperty]
    private List<Item> _items = new();

    [ObservableProperty]
    private bool _isRefreshing;

    public ItemListViewModel(IItemService itemService)
    {
        _itemService = itemService;
        Title = "Available Items";
    }

    [RelayCommand]
    public async Task LoadItemsAsync()
    {
        try
        {
            IsRefreshing = true;
            Items = await _itemService.GetAllItemsAsync();
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
    public async Task GoToCreateItemAsync()
    {
        await Shell.Current.GoToAsync("CreateItemPage");
    }

    [RelayCommand]
    public async Task GoToItemDetailAsync(Item item)
    {
        await Shell.Current.GoToAsync($"ItemDetailPage?itemId={item.Id}");
    }
}