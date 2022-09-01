using Microsoft.EntityFrameworkCore;

namespace TodoApi.Models;

class TodoDb : DbContext
{
    public TodoDb(DbContextOptions<TodoDb> options)
        : base(options) { }

    public DbSet<Todo> Todos => Set<Todo>();
}

// <snippet>
record TodoItemRequest(int Id, TodoDb Db);
// <snippet_1>
record CreateTodoItemRequest(TodoItemDTO Dto, TodoDb Db);
// </snippet_1>
record EditTodoItemRequest(int Id, TodoItemDTO Dto, TodoDb Db);
// </snippet>
