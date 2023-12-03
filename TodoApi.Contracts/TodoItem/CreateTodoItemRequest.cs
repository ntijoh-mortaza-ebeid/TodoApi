namespace TodoApi.Contracts.TodoItem;

public record CreateTodoItemRequest(
    string Name,
    bool? IsComplete
);
