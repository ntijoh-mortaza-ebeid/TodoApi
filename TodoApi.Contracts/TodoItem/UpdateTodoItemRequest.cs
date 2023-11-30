namespace TodoApi.Contracts;

public record UpdateTodoItemRequest
{
    public string? Name { get; set; }

    public bool? IsComplete { get; set; }
};
