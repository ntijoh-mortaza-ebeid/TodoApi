namespace TodoApi.Models;

public class TodoItem
{
    public Guid Id { get; set; }
    public string Name { get; set; } = "";
    public bool IsComplete { get; set; } // TODO are default values needed?
}
