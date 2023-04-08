using Microsoft.AspNetCore.Http.HttpResults;
using UnitTests.Helpers;
using WebMinRouteGroup;
using WebMinRouteGroup.Data;

namespace UnitTests;

public class TodoInMemoryTests
{
    // <snippet_>
    [Fact]
    public async Task GetTodoReturnsNotFoundIfNotExists()
    {
        // Arrange
        await using var context = new MockDb().CreateDbContext();

        // Act
        var result = await TodoEndpointsV1.GetTodo(1, context);

        //Assert
        Assert.IsType<Results<Ok<Todo>, NotFound>>(result);

        var notFoundResult = (NotFound) result.Result;

        Assert.NotNull(notFoundResult);
    }
    // </snippet_>

    [Fact]
    public async Task GetAllReturnsTodosFromDatabase()
    {
        // Arrange
        await using var context = new MockDb().CreateDbContext();

        context.Todos.Add(new Todo
        {
            Id = 1,
            Title = "Test title 1",
            Description = "Test description 1",
            IsDone = false
        });

        context.Todos.Add(new Todo
        {
            Id = 2,
            Title = "Test title 2",
            Description = "Test description 2",
            IsDone = true
        });

        await context.SaveChangesAsync();

        // Act
        var result = await TodoEndpointsV1.GetAllTodos(context);

        //Assert
        Assert.IsType<Ok<List<Todo>>>(result);

        var okResult = (Ok<List<Todo>>)result;

        Assert.NotNull(okResult.Value);
        Assert.NotEmpty(okResult.Value);
        Assert.Collection(okResult.Value, todo1 =>
        {
            Assert.Equal(1, todo1.Id);
            Assert.Equal("Test title 1", todo1.Title);
            Assert.False(todo1.IsDone);
        }, todo2 =>
        {
            Assert.Equal(2, todo2.Id);
            Assert.Equal("Test title 2", todo2.Title);
            Assert.True(todo2.IsDone);
        });
    }

    // <snippet_1>
    [Fact]
    public async Task GetTodoReturnsTodoFromDatabase()
    {
        // Arrange
        await using var context = new MockDb().CreateDbContext();

        context.Todos.Add(new Todo
        {
            Id = 1,
            Title = "Test title",
            Description = "Test description",
            IsDone = false
        });

        await context.SaveChangesAsync();

        // Act
        var result = await TodoEndpointsV1.GetTodo(1, context);

        //Assert
        Assert.IsType<Results<Ok<Todo>, NotFound>>(result);

        var okResult = (Ok<Todo>)result.Result;

        Assert.NotNull(okResult.Value);
        Assert.Equal(1, okResult.Value.Id);
    }
    // </snippet_1>

    // <snippet_3>
    [Fact]
    public async Task CreateTodoCreatesTodoInDatabase()
    {
        //Arrange
        await using var context = new MockDb().CreateDbContext();

        var newTodo = new TodoDto
        {
            Title = "Test title",
            Description = "Test description",
            IsDone = false
        };

        //Act
        var result = await TodoEndpointsV1.CreateTodo(newTodo, context);

        //Assert
        Assert.IsType<Created<Todo>>(result);

        var createdResult = (Created<Todo>) result;

        Assert.NotNull(createdResult);
        Assert.NotNull(createdResult.Location);

        Assert.NotEmpty(context.Todos);
        Assert.Collection(context.Todos, todo =>
        {
            Assert.Equal("Test title", todo.Title);
            Assert.Equal("Test description", todo.Description);
            Assert.False(todo.IsDone);
        });
    }
    // </snippet_3>

    [Fact]
    public async Task UpdateTodoUpdatesTodoInDatabase()
    {
        //Arrange
        await using var context = new MockDb().CreateDbContext();

        context.Todos.Add(new Todo
        {
            Id = 1,
            Title = "Exiting test title",
            IsDone = false
        });

        await context.SaveChangesAsync();

        var updatedTodo = new Todo
        {
            Id = 1,
            Title = "Updated test title",
            IsDone = true
        };

        //Act
        var result = await TodoEndpointsV1.UpdateTodo(updatedTodo, context);

        //Assert
        Assert.IsType<Results<Created<Todo>, NotFound>>(result);

        var createdResult = (Created<Todo>)result.Result;

        Assert.NotNull(createdResult);
        Assert.NotNull(createdResult.Location);

        var todoInDb = await context.Todos.FindAsync(1);

        Assert.NotNull(todoInDb);
        Assert.Equal("Updated test title", todoInDb!.Title);
        Assert.True(todoInDb.IsDone);
    }

    [Fact]
    public async Task DeleteTodoDeletesTodoInDatabase()
    {
        //Arrange
        await using var context = new MockDb().CreateDbContext();

        var existingTodo = new Todo
        {
            Id = 1,
            Title = "Exiting test title",
            IsDone = false
        };

        context.Todos.Add(existingTodo);

        await context.SaveChangesAsync();

        //Act
        var result = await TodoEndpointsV1.DeleteTodo(existingTodo.Id, context);

        //Assert
        Assert.IsType<Results<NoContent, NotFound>>(result);

        var noContentResult = (NoContent)result.Result;

        Assert.NotNull(noContentResult);
        Assert.Empty(context.Todos);
    }
}
