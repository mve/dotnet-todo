using System.ComponentModel;
using System.Net;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text.Json;
using blazor.Models;

namespace blazor.viewmodels;

public class TodoViewModel : ITodoViewModel
{
    private readonly HttpClient Http = new();
    private const string ServiceEndpoint = "https://localhost:7103/api/todo";

    public string? Id { get; set; }
    public string? Name { get; set; }
    public bool IsComplete { get; set; }

    public string newItemName { get; set; }
    private List<TodoItem> _TodoItems;

    public List<TodoItem> TodoItems
    {
        get => _TodoItems;
        set => SetValue(ref _TodoItems, value);
    }

    private TodoItem? _SelectedItem;

    public TodoItem? SelectedItem
    {
        get => _SelectedItem;
        set => SetValue(ref _SelectedItem, value);
    }

    private string _filter = "all";

    public string filter
    {
        get => _filter;
        set => SetValue(ref _filter, value);
    }

    private string _ErrorMessage = "";

    public string errorMessage
    {
        get => _ErrorMessage;
        set => SetValue(ref _ErrorMessage, value);
    }

    public async Task GetTodoItems()
    {
        TodoItems = await Http.GetFromJsonAsync<List<TodoItem>>(ServiceEndpoint);
    }

    public void EditItem(string id)
    {
        SelectedItem = TodoItems.Single(i => i.Id == id);
    }

    public async Task ToggleStatus(string id)
    {
        var toggleItem = TodoItems.Single(i => i.Id == id);
        toggleItem.IsComplete = !toggleItem.IsComplete;

        await Http.PutAsJsonAsync($"{ServiceEndpoint}/{id}",
            toggleItem);

        await GetTodoItems();
    }

    public async Task AddItem()
    {
        ClearErrorMessage();
        var addItem = new TodoItem {Name = newItemName, IsComplete = false};

        var result = await Http.PostAsJsonAsync(ServiceEndpoint, addItem);

        // If there was an error, update the error message.
        if (!result.IsSuccessStatusCode)
        {
            errorMessage = "Kon todo item niet toevoegen.";
        }

        newItemName = string.Empty;
        await GetTodoItems();
    }

    public async Task SaveItem()
    {
        ClearErrorMessage();
        if (SelectedItem is not null)
        {
            var result = await Http.PutAsJsonAsync($"{ServiceEndpoint}/{SelectedItem.Id}",
                SelectedItem);

            // If there was an error, update the error message.
            if (!result.IsSuccessStatusCode)
            {
                errorMessage = "Kon todo item niet wijzigen.";
            }
        }

        await GetTodoItems();
        SelectedItem = null;
    }

    public async Task DeleteItem(string id)
    {
        await Http.DeleteAsync($"{ServiceEndpoint}/{id}");
        await GetTodoItems();
    }

    public void ClearErrorMessage()
    {
        errorMessage = "";
    }

    public event PropertyChangedEventHandler PropertyChanged;

    // changed default propertyName parameter from null to "", both locations
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private void SetValue<T>(
        ref T backingFiled,
        T value,
        [CallerMemberName] string propertyName = "")
    {
        if (EqualityComparer<T>.Default.Equals(backingFiled, value)) return;
        backingFiled = value;
        OnPropertyChanged(propertyName);
    }
}