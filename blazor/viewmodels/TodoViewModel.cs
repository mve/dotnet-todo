﻿using System.ComponentModel;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using blazor.Models;

namespace blazor.viewmodels;

public class TodoViewModel : ITodoViewModel
{
    private HttpClient Http = new();
    private const string ServiceEndpoint = "https://localhost:7103/api/todo";

    public string? Id { get; set; }
    public string? Name { get; set; }
    public bool IsComplete { get; set; }

    public string newItemName { get; set; }
    private List<TodoItem> _TodoItems;

    public List<TodoItem> TodoItems
    {
        get { return _TodoItems; }
        set { SetValue<List<TodoItem>>(ref _TodoItems, value); }
    }

    public TodoItem? _SelectedItem;

    public TodoItem? SelectedItem
    {
        get { return _SelectedItem; }
        set { SetValue<TodoItem?>(ref _SelectedItem, value); }
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
        var addItem = new TodoItem {Name = newItemName, IsComplete = false};
        await Http.PostAsJsonAsync(ServiceEndpoint, addItem);
        newItemName = string.Empty;
        await GetTodoItems();
    }

    public async Task SaveItem()
    {
        if (SelectedItem is not null)
        {
            await Http.PutAsJsonAsync($"{ServiceEndpoint}/{SelectedItem.Id}",
                SelectedItem);
        }

        await GetTodoItems();
        SelectedItem = null;
    }

    public async Task DeleteItem(string id)
    {
        await Http.DeleteAsync($"{ServiceEndpoint}/{id}");
        await GetTodoItems();
    }

    public event PropertyChangedEventHandler PropertyChanged;

    // changed default propertyName parameter from null to "", both locations
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected void SetValue<T>(
        ref T backingFiled,
        T value,
        [CallerMemberName] string propertyName = "")
    {
        if (EqualityComparer<T>.Default.Equals(backingFiled, value)) return;
        backingFiled = value;
        OnPropertyChanged(propertyName);
    }
}