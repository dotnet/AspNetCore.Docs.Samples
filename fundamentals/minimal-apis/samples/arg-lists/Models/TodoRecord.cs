using TodoApi.Models;

namespace Record.Models;

// <snippet_1>
record TodoItemRequest(int Id, TodoDb Db);
record CreateTodoItemRequest(TodoItemDTO Dto, TodoDb Db);
record EditTodoItemRequest(int Id, TodoItemDTO Dto, TodoDb Db);
// </snippet_1>
