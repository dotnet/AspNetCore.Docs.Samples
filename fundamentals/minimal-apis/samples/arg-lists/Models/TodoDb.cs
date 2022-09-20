using Microsoft.EntityFrameworkCore;

namespace TodoApi.Models;

class TodoDb : DbContext
{
    public TodoDb(DbContextOptions<TodoDb> options)
        : base(options) { }

    public DbSet<Todo> Todos => Set<Todo>();
}

// <snippet>
struct TodoItemRequest
{
    public int Id { get; set; }
    public TodoDb Db { get; set; }
}
// </snippet>

// <snippet_1>
class CreateTodoItemRequest
{
    public TodoItemDTO Dto { get; set; } = default!;
    public TodoDb Db { get; set; } = default!;
}

class EditTodoItemRequest
{
    public int Id { get; set; }
    public TodoItemDTO Dto { get; set; } = default!;
    public TodoDb Db { get; set; } = default!;
}
// </snippet_1>
