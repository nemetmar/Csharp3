namespace ToDoList.Test;

using Microsoft.VisualStudio.TestPlatform.ObjectModel;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using ToDoList.Domain.Models;
using ToDoList.WebApi;
using ToDoList.Domain.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

public class GetTests
{

    [Fact]
    public void Get_AllItems_ReturnsAllItems()
    {
        // Arrange
        var controller = new ToDoItemsController();
        var toDoItem = new ToDoItem
        {
            ToDoItemId = 1,
            Name = "Test name",
            Description = "Test description",
            IsCompleted = false
        };
        ToDoItemsController.items.Add(toDoItem);

        //Act
        var result = controller.Read();
        var resultResult = result.Result as OkObjectResult;
        var value = resultResult.Value as IEnumerable<ToDoItemGetResponseDto>;

        //Assert
        Assert.True(resultResult is OkObjectResult);
        Assert.IsType<OkObjectResult>(resultResult);

        Assert.NotNull(value);
        Assert.Single(value);

        var firstItem = value.First();
        Assert.Equal(toDoItem.ToDoItemId, firstItem.ToDoItemId);
        Assert.Equal(toDoItem.Description, firstItem.Description);
        Assert.Equal(toDoItem.IsCompleted, firstItem.IsCompleted);
        Assert.Equal(toDoItem.Name, firstItem.Name);

    }

    [Fact]
    public void Get_ItemById_ReturnsItemById()
    {
        // Arrange
        var controller = new ToDoItemsController();
        var toDoItem = new ToDoItem
        {
            ToDoItemId = 3,
            Name = "Test name",
            Description = "Test description",
            IsCompleted = false
        };
        ToDoItemsController.items.Add(toDoItem);

        // Act
        var result = controller.ReadById(3);
        var resultResult = result as OkObjectResult;
        var value = resultResult.Value as ToDoItemGetResponseDto;

        //Assert
        Assert.True(result is OkObjectResult);
        Assert.IsType<OkObjectResult>(result);

        Assert.NotNull(value);

        Assert.Equal(toDoItem.ToDoItemId, value.ToDoItemId);
        Assert.Equal(toDoItem.Description, value.Description);
        Assert.Equal(toDoItem.IsCompleted, value.IsCompleted);
        Assert.Equal(toDoItem.Name, value.Name);


    }

    [Fact]
    public void Get_ItemById_Returns404OnIdNotFound()
    {
        // Arrange
        var controller = new ToDoItemsController();

        // Act
        var result = controller.ReadById(20);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
}
