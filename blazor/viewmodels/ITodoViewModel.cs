using System.ComponentModel;
using blazor.Models;

namespace blazor.viewmodels;

public interface ITodoViewModel : INotifyPropertyChanged
{
    string Id { get; set; }
    string? Name { get; set; }
    bool IsComplete { get; set; }
    string newItemName { get; set; }

    List<TodoItem> TodoItems { get; set; }
    TodoItem SelectedItem { get; set; }

    Task GetTodoItems();
    void EditItem(string id);
    Task ToggleStatus(string id);
    Task AddItem();
    Task SaveItem();
    Task DeleteItem(string id);
}