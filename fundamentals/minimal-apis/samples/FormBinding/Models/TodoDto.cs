namespace TodoApi.Models;

public class TodoDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public Visibility Visibility { get; set; }

    public string? Attachment { get; set; }

    public TodoDto(Todo todoItem) 
        => (Id, Name, Visibility, Attachment) = (todoItem.Id, todoItem.Name, todoItem.Visibility, todoItem.Attachment);
}
