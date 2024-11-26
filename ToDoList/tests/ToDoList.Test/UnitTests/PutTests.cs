namespace ToDoList.Test;

using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.Models;
using ToDoList.WebApi;
using ToDoList.Domain.DTOs;
using NSubstitute;
using ToDoList.Persistence.Repositories;
using NSubstitute.ReturnsExtensions;
using NSubstitute.ExceptionExtensions;
using Microsoft.AspNetCore.Http;

public class PutTests_Unit
{
    [Fact]
    public async void Put_UpdateByIdWhenItemUpdated_ReturnsNoContent()
    {
        //Arrange
        var repositoryMock = Substitute.For<IRepositoryAsync<ToDoItem>>();
        var controller = new ToDoItemsController(repositoryMock);
        repositoryMock.ReadByIdAsync(Arg.Any<int>()).Returns(new ToDoItem { Name = "Test name", Description = "Test description", IsCompleted = false, Category = "Homework" });
        var someId = 1;
        var item = new ToDoItemUpdateRequestDto("Updated name", "Updated description", false, "Homework");

        //Act
        var result = await controller.UpdateByIdAsync(someId, item);

        //Assert
        await repositoryMock.Received(1).ReadByIdAsync(someId);
        //Tady jsem měla problém, že mi test nebral item.ToDomain() v argumentu, řešení mi opět poradila AI, tak snad je korektní
        //přišlo mi, že Arg.Any<ToDoItem>(), které máme v PostTests netestuje úplně to, že se jedná o stejné hodnoty
        await repositoryMock.Received(1).UpdateByIdAsync(someId, Arg.Is<ToDoItem>(x => x.Name == item.Name && x.Description == item.Description && x.IsCompleted == item.IsCompleted && x.Category == item.Category));
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async void Put_UpdateByIdWhenIdNotFound_ReturnsNotFound()
    {
        //Arrange
        var repositoryMock = Substitute.For<IRepositoryAsync<ToDoItem>>();
        var controller = new ToDoItemsController(repositoryMock);
        repositoryMock.ReadByIdAsync(Arg.Any<int>()).ReturnsNull();
        var someId = 1;
        var item = new ToDoItemUpdateRequestDto("Updated name", "Updated description", false, "Homework");

        //Act
        var result = await controller.UpdateByIdAsync(someId, item);

        //Assert
        await repositoryMock.Received(1).ReadByIdAsync(someId);
        await repositoryMock.Received(0).UpdateByIdAsync(someId, item.ToDomain());
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async void Put_UpdateByIdUnhandledException_ReturnsInternalServerError()
    {
        //Arrange
        var repositoryMock = Substitute.For<IRepositoryAsync<ToDoItem>>();
        var controller = new ToDoItemsController(repositoryMock);
        repositoryMock.ReadByIdAsync(Arg.Any<int>()).Throws(new Exception());
        var someId = 1;
        var item = new ToDoItemUpdateRequestDto("Updated name", "Updated description", false, "Homework");

        //Act
        var result = await controller.UpdateByIdAsync(someId, item);

        //Assert
        await repositoryMock.Received(1).ReadByIdAsync(someId);
        await repositoryMock.Received(0).UpdateByIdAsync(someId, item.ToDomain());
        Assert.IsType<ObjectResult>(result);
        Assert.Equivalent(new StatusCodeResult(StatusCodes.Status500InternalServerError), result);
    }

    [Fact]
    public async void Put_UpdateByIdThrowsException_ReturnsInternalServerError()
    {
        // Arrange
        var repositoryMock = Substitute.For<IRepositoryAsync<ToDoItem>>();
        var controller = new ToDoItemsController(repositoryMock);
        repositoryMock.ReadByIdAsync(Arg.Any<int>()).Returns(new ToDoItem { Name = "Test name", Description = "Test description", IsCompleted = false, Category = "Homework" });
        repositoryMock.When(repo => repo.UpdateByIdAsync(Arg.Any<int>(), Arg.Any<ToDoItem>())).Do(call => { throw new Exception(); });
        var someId = 1;
        var item = new ToDoItemUpdateRequestDto("Updated name", "Updated description", false, "Homework");

        // Act
        var result = await controller.UpdateByIdAsync(someId, item);
        // Assert

        await repositoryMock.Received(1).ReadByIdAsync(someId);
        await repositoryMock.Received(1).UpdateByIdAsync(someId, Arg.Is<ToDoItem>(x => x.Name == item.Name && x.Description == item.Description && x.IsCompleted == item.IsCompleted && x.Category == item.Category));
        Assert.IsType<ObjectResult>(result);
        Assert.Equivalent(new StatusCodeResult(StatusCodes.Status500InternalServerError), result);
    }

}
