namespace TodoApi.Contracts.TodoItem;

public record TodoItemResponse(
    Guid Id,
    string Name,
    bool IsComplete
);
