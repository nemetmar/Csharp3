namespace ToDoList.Test;

using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.Models;
using ToDoList.WebApi;
using ToDoList.Domain.DTOs;

[Collection("Tests")]
public class PostTests
{
    [Fact]
    public void Post_Item_CreatesItem()
    {
        // Arrange
        var controller = new ToDoItemsController();
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
        Assert.Equal(ToDoItemsController.items.Max(i => i.ToDoItemId), value.ToDoItemId);
    }
}
