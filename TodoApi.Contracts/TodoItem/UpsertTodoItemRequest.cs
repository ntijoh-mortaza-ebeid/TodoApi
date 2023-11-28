namespace TodoApi.Contracts;

public record UpsertTodoItemRequest(
    string Name,
    bool IsComplete
);
