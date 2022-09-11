using Microsoft.EntityFrameworkCore;

namespace TodoApi.Models;

class TodoDb : DbContext
{
    public TodoDb(DbContextOptions<TodoDb> options)
        : base(options) { }

    public DbSet<Todo> Todos => Set<Todo>();
}

// <snippet_1>
// <snippet>
struct TodoItemRequest
{
    public int Id { get; set; }
    public TodoDb Db { get; set; }
}
// </snippet>
class CreateTodoItemRequest
{
    public TodoItemDTO Dto { get; set; } = default!;
    public TodoDb Db { get; set; } = default!;
}
record EditTodoItemRequest(int Id, TodoItemDTO Dto, TodoDb Db);
// </snippet_1>
