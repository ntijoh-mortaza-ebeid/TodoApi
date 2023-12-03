namespace TodoApi.Contracts.TodoItem;

public record UpdateTodoItemRequest(
    string? Name,
    bool? IsComplete
);
