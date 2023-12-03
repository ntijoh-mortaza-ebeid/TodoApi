namespace TodoApi.Contracts;

public record TodoItemResponse(
    long Id,
    string Name,
    bool IsComplete
);
