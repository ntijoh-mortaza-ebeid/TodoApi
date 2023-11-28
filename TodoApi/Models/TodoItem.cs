namespace TodoApi.Models;

public class TodoItem
{
    public Guid Id { get; set; }
    public string Name { get; set; } = "";
    public bool IsComplete { get; set; }

    public TodoItem(
        Guid id,
        string name,
        bool isComplete)
    {
        Id = id;
        Name = name;
        IsComplete = isComplete;
    }
}
