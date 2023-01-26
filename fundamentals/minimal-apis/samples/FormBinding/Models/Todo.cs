namespace TodoApi.Models;

public class Todo
{
    public int Id { get; set; }
    public required string Name { get; init; }
    public required Visibility Visibility { get; init; }

    public string? Attachment { get; set; }
}

public enum Visibility
{
    Public,
    Private
}
