namespace ToDoList.Test;

using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.Models;
using ToDoList.WebApi;
using ToDoList.Domain.DTOs;
using NSubstitute;
using ToDoList.Persistence.Repositories;
using Microsoft.AspNetCore.Http;

public class PostTests_Unit
{
    [Fact]
    public void Post_CreateValidRequest_ReturnsCreatedAtAction()
    {
        // Arrange
        var repositoryMock = Substitute.For<IRepository<ToDoItem>>();
        var controller = new ToDoItemsController(repositoryMock);
        var item = new ToDoItemCreateRequestDto("Test name", "Test description", false, "Homework");

        //Act
        var result = controller.Create(item);
        var resultResult = result as CreatedAtActionResult;
        var value = resultResult.Value as ToDoItemGetResponseDto;

        //Assert
        Assert.True(result is CreatedAtActionResult);
        Assert.IsType<CreatedAtActionResult>(result);
        repositoryMock.Received(1).Create(Arg.Any<ToDoItem>());
        Assert.NotNull(value);

        Assert.Equal(item.Description, value.Description);
        Assert.Equal(item.IsCompleted, value.IsCompleted);
        Assert.Equal(item.Name, value.Name);
    }


    [Fact]
    public void Post_CreateUnhandledException_ReturnsInternalServerError()
    {
        // Arrange
        var repositoryMock = Substitute.For<IRepository<ToDoItem>>();
        var controller = new ToDoItemsController(repositoryMock);
        var item = new ToDoItemCreateRequestDto("Test name", "Test description", false, "Homework");

        repositoryMock.When(r => r.Create(Arg.Any<ToDoItem>())).Do(r => throw new Exception());

        //Act
        var result = controller.Create(item);

        //Assert
        Assert.True(result is ObjectResult);
        Assert.IsType<ObjectResult>(result);
        repositoryMock.Received(1).Create(Arg.Any<ToDoItem>());
        Assert.Equivalent(new StatusCodeResult(StatusCodes.Status500InternalServerError), result);
    }

}
