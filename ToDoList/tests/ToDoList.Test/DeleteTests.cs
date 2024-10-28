namespace ToDoList.Test;

using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.Models;
using ToDoList.WebApi;
using ToDoList.Domain.DTOs;

[Collection("Tests")]
public class DeleteTests
{
    [Fact]
    // tu si sa asi tiez prepsala a mas dvakrat item v nazve :)
    public void Delete_ItemItemById_DeletesItemById()
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
        var result = controller.DeleteById(toDoItem.ToDoItemId);

        //Assert
        Assert.True(result is OkResult);
        Assert.IsType<OkResult>(result);

        Assert.Null(ToDoItemsController.items.Find(i => i.ToDoItemId == toDoItem.ToDoItemId));
    }

    [Fact]
    public void Delete_ItemById_Returns404OnIdNotFound()
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
        var countInitial = ToDoItemsController.items.Count;

        //Act
        var result = controller.DeleteById(222);

        //Assert
        Assert.IsType<NotFoundResult>(result);
        Assert.Equal(countInitial, ToDoItemsController.items.Count);

    }
}
