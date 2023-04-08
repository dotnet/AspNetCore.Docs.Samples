using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using WebMinRouteGroup;
using WebMinRouteGroup.Data;
using WebMinRouteGroup.Services;

namespace UnitTests;

public class TodoMoqTests
{
    [Fact]
    public async Task GetTodoReturnsNotFoundIfNotExists()
    {
        // Arrange
        var mock = new Mock<ITodoService>();

        mock.Setup(m => m.Find(It.Is<int>(id => id == 1)))
            .ReturnsAsync((Todo?)null);

        // Act
        var result = await TodoEndpointsV2.GetTodo(1, mock.Object);

        //Assert
        Assert.IsType<Results<Ok<Todo>, NotFound>>(result);

        var notFoundResult = (NotFound) result.Result;

        Assert.NotNull(notFoundResult);
    }

    [Fact]
    public async Task GetAllReturnsTodosFromDatabase()
    {
        // Arrange
        var mock = new Mock<ITodoService>();

        mock.Setup(m => m.GetAll())
            .ReturnsAsync(new List<Todo> {
                new Todo
                {
                    Id = 1,
                    Title = "Test title 1",
                    IsDone = false
                },
                new Todo
                {
                    Id = 2,
                    Title = "Test title 2",
                    IsDone = true
                }
            });

        // Act
        var result = await TodoEndpointsV2.GetAllTodos(mock.Object);

        //Assert
        Assert.IsType<Ok<List<Todo>>>(result);

        var okResult = (Ok<List<Todo>>) result;

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

    [Fact]
    public async Task GetAllIncompletedReturnsIncompletedTodosFromDatabase()
    {
        // Arrange
        var mock = new Mock<ITodoService>();

        mock.Setup(m => m.GetIncompleteTodos())
            .ReturnsAsync(new List<Todo> {
                new Todo
                {
                    Id = 1,
                    Title = "Test title 1",
                    IsDone = false
                },
                new Todo
                {
                    Id = 2,
                    Title = "Test title 2",
                    IsDone = false
                }
            });

        // Act
        var result = await TodoEndpointsV2.GetAllIncompletedTodos(mock.Object);

        //Assert
        Assert.IsType<Ok<List<Todo>>>(result);

        var okResult = (Ok<List<Todo>>) result;

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
            Assert.False(todo2.IsDone);
        });
    }

    [Fact]
    public async Task GetTodoReturnsTodoFromDatabase()
    {
        // Arrange
        var mock = new Mock<ITodoService>();

        mock.Setup(m => m.Find(It.Is<int>(id => id == 1)))
            .ReturnsAsync(new Todo
            {
                Id = 1,
                Title = "Test title",
                IsDone = false
            });

        // Act
        var result = await TodoEndpointsV2.GetTodo(1, mock.Object);

        //Assert
        Assert.IsType<Results<Ok<Todo>, NotFound>>(result);

        var okResult = (Ok<Todo>) result.Result;

        Assert.NotNull(okResult.Value);
        Assert.Equal(1, okResult.Value.Id);
    }

    [Fact]
    public async Task CreateTodoCreatesTodoInDatabase()
    {
        //Arrange
        var todos = new List<Todo>();

        var newTodo = new TodoDto
        {
            Title = "Test title",
            Description = "Test description",
            IsDone = false
        };

        var mock = new Mock<ITodoService>();

        mock.Setup(m => m.Add(It.Is<Todo>(t => t.Title == newTodo.Title && t.Description == newTodo.Description && t.IsDone == newTodo.IsDone)))
            .Callback<Todo>(todo => todos.Add(todo))
            .Returns(Task.CompletedTask);

        //Act
        var result = await TodoEndpointsV2.CreateTodo(newTodo, mock.Object);

        //Assert
        Assert.IsType<Created<Todo>>(result);

        var createdResult = (Created<Todo>) result;

        Assert.NotNull(createdResult);
        Assert.NotNull(createdResult.Location);

        Assert.NotEmpty(todos);
        Assert.Collection(todos, todo =>
        {
            Assert.Equal("Test title", todo.Title);
            Assert.Equal("Test description", todo.Description);
            Assert.False(todo.IsDone);
        });
    }

    [Fact]
    public async Task UpdateTodoUpdatesTodoInDatabase()
    {
        //Arrange
        var existingTodo = new Todo
        {
            Id = 1,
            Title = "Exiting test title",
            IsDone = false
        };

        var updatedTodo = new Todo
        {
            Id = 1,
            Title = "Updated test title",
            IsDone = true
        };

        var mock = new Mock<ITodoService>();

        mock.Setup(m => m.Find(It.Is<int>(id => id == 1)))
            .ReturnsAsync(existingTodo);

        mock.Setup(m => m.Update(It.Is<Todo>(t => t.Id == updatedTodo.Id && t.Description == updatedTodo.Description && t.IsDone == updatedTodo.IsDone)))
            .Callback<Todo>(todo => existingTodo = todo)
            .Returns(Task.CompletedTask);

        //Act
        var result = await TodoEndpointsV2.UpdateTodo(updatedTodo, mock.Object);

        //Assert
        Assert.IsType<Results<Created<Todo>, NotFound>>(result);

        var createdResult = (Created<Todo>) result.Result;

        Assert.NotNull(createdResult);
        Assert.NotNull(createdResult.Location);

        Assert.Equal("Updated test title", existingTodo.Title);
        Assert.True(existingTodo.IsDone);
    }

    [Fact]
    public async Task DeleteTodoDeletesTodoInDatabase()
    {
        //Arrange
        var existingTodo = new Todo
        {
            Id = 1,
            Title = "Test title 1",
            IsDone = false
        };

        var todos = new List<Todo> { existingTodo };

        var mock = new Mock<ITodoService>();

        mock.Setup(m => m.Find(It.Is<int>(id => id == existingTodo.Id)))
            .ReturnsAsync(existingTodo);

        mock.Setup(m => m.Remove(It.Is<Todo>(t => t.Id == 1)))
            .Callback<Todo>(t => todos.Remove(t))
            .Returns(Task.CompletedTask);

        //Act
        var result = await TodoEndpointsV2.DeleteTodo(existingTodo.Id, mock.Object);

        //Assert
        Assert.IsType<Results<NoContent, NotFound>>(result);
        
        var noContentResult = (NoContent) result.Result;

        Assert.NotNull(noContentResult);
        Assert.Empty(todos);
    }
}
