namespace TodoApi.Contracts.TodoItem;

public record TodoItemResponse(
    long Id,
    string Name,
    bool IsComplete
);
