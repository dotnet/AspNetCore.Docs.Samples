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
record TodoItemRequest(int Id, TodoDb Db);
// </snippet>
record CreateTodoItemRequest(TodoItemDTO Dto, TodoDb Db);
record EditTodoItemRequest(int Id, TodoItemDTO Dto, TodoDb Db);
// </snippet_1>
