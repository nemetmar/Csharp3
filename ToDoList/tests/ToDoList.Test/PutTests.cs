namespace ToDoList.Test;

using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.Models;
using ToDoList.WebApi;
using ToDoList.Domain.DTOs;

[Collection("Tests")]
public class PutTests
{
    [Fact]
    // tu si sa asi prepsala a mas tu dvakrat item v nazvu :)
    public void Put_ItemItemById_UpdatesItemById()
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
        var item = new ToDoItemUpdateRequestDto("Updated name", "Updated description", true);


        //Act
        var result = controller.UpdateById(toDoItem.ToDoItemId, item);
        /*
        Pozor, PUT potom nebude vracat CreatedAtAction (ked testy opravime a pridame tam mocky), takze ti pravdepodobne tento test bude padat
        Nezabudni si to opravit na to, ze testujes NoContent + zaroven uz nebudes mat response, takze cast, kde value porovnavas s item uz tiez nebude validna
        */
        var resultResult = result as CreatedAtActionResult;
        var value = resultResult.Value as ToDoItemGetResponseDto;

        //Assert
        Assert.True(result is CreatedAtActionResult);
        Assert.IsType<CreatedAtActionResult>(result);

        Assert.NotNull(value);

        Assert.Equal(item.Description, value.Description);
        Assert.Equal(item.IsCompleted, value.IsCompleted);
        Assert.Equal(item.Name, value.Name);
        Assert.Equal(toDoItem.ToDoItemId, value.ToDoItemId);
    }

    [Fact]
    public void Put_ItemById_Returns404OnIdNotFound()
    {
        // Arrange
        var controller = new ToDoItemsController();
        var item = new ToDoItemUpdateRequestDto("Updated name", "Updated description", true);

        //Act
        var result = controller.UpdateById(100, item);

        //Assert
        Assert.IsType<NotFoundResult>(result);

    }
}
