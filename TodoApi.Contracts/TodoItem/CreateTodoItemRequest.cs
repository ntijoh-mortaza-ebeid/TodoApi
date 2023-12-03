namespace TodoApi.Contracts;

public record CreateTodoItemRequest(
    string Name,
    bool? IsComplete
);
