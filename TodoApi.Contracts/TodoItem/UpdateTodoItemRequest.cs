namespace TodoApi.Contracts;

public record UpdateTodoItemRequest(
    string? Name,
    bool? IsComplete
);
