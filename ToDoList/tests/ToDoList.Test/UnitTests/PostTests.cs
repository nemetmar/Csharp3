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
    public void Post_Item_CreatesItem()
    {
        // Arrange
        var repositoryMock = Substitute.For<IRepository<ToDoItem>>();
        var controller = new ToDoItemsController(repositoryMock);
        var item = new ToDoItemCreateRequestDto("Test name", "Test description", false);


        //Act
        var result = controller.Create(item);
        var resultResult = result as CreatedAtActionResult;
        var value = resultResult.Value as ToDoItemGetResponseDto;

        //Assert
        Assert.True(result is CreatedAtActionResult);
        Assert.IsType<CreatedAtActionResult>(result);

        Assert.NotNull(value);

        Assert.Equal(item.Description, value.Description);
        Assert.Equal(item.IsCompleted, value.IsCompleted);
        Assert.Equal(item.Name, value.Name);
        //Assert.Equal(ToDoItemsController.items.Max(i => i.ToDoItemId), value.ToDoItemId);
    }


    [Fact]
    public void Post_UnhandledException_Returns500()
    {
        // Arrange
        var repositoryMock = Substitute.For<IRepository<ToDoItem>>();
        var controller = new ToDoItemsController(repositoryMock);
        var item = new ToDoItemCreateRequestDto("Test name", "Test description", false);

        repositoryMock.When(r => r.Create(Arg.Any<ToDoItem>())).Do(r => throw new Exception());

        //Act
        var result = controller.Create(item);

        //Assert
        Assert.True(result is ObjectResult);
        Assert.IsType<ObjectResult>(result);
        Assert.Equivalent(new StatusCodeResult(StatusCodes.Status500InternalServerError), result);
    }

}
