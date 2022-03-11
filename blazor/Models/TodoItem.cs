namespace blazor.Models;

public class TodoItem
{
    // public TodoItem(string Id, string Name, bool IsComplete)
    // {
    //     this.Id = Id;
    //     this.Name = Name;
    //     this.IsComplete = IsComplete;
    // }

    public string Id { get; set; }
    public string? Name { get; set; }
    public bool IsComplete { get; set; }
}