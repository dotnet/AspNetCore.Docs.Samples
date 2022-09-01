using Microsoft.EntityFrameworkCore;

namespace TodoApi.Models;

class TodoDb : DbContext
{
    public TodoDb(DbContextOptions<TodoDb> options)
        : base(options) { }

    public DbSet<Todo> Todos => Set<Todo>();
}


record TodoItemRequest(int Id, TodoDb Db);

record CreateTodoItemRequest(TodoItemDTO Dto, TodoDb Db);

record EditTodoItemRequest(int Id, TodoItemDTO Dto, TodoDb Db);
