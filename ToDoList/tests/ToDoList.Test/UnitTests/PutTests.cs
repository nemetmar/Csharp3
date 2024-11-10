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
    public void Put_UpdateByIdWhenItemUpdated_ReturnsNoContent()
    {
        //Arrange
        var repositoryMock = Substitute.For<IRepository<ToDoItem>>();
        var controller = new ToDoItemsController(repositoryMock);
        repositoryMock.ReadById(Arg.Any<int>()).Returns(new ToDoItem { Name = "Test name", Description = "Test description", IsCompleted = false });
        var someId = 1;
        var item = new ToDoItemUpdateRequestDto("Updated name", "Updated description", false);

        //Act
        var result = controller.UpdateById(someId, item);

        //Assert
        repositoryMock.Received(1).ReadById(someId);
        //Tady jsem měla problém, že mi test nebral item.ToDomain() v argumentu, řešení mi opět poradila AI, tak snad je korektní
        //přišlo mi, že Arg.Any<ToDoItem>(), které máme v PostTests netestuje úplně to, že se jedná o stejné hodnoty
        repositoryMock.Received(1).UpdateById(someId, Arg.Is<ToDoItem>(x => x.Name == item.Name && x.Description == item.Description && x.IsCompleted == item.IsCompleted));
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public void Put_UpdateByIdWhenIdNotFound_ReturnsNotFound()
    {
        //Arrange
        var repositoryMock = Substitute.For<IRepository<ToDoItem>>();
        var controller = new ToDoItemsController(repositoryMock);
        repositoryMock.ReadById(Arg.Any<int>()).ReturnsNull();
        var someId = 1;
        var item = new ToDoItemUpdateRequestDto("Updated name", "Updated description", false);

        //Act
        var result = controller.UpdateById(someId, item);

        //Assert
        repositoryMock.Received(1).ReadById(someId);
        repositoryMock.Received(0).UpdateById(someId, item.ToDomain());
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public void Put_UpdateByIdUnhandledException_ReturnsInternalServerError()
    {
        //Arrange
        var repositoryMock = Substitute.For<IRepository<ToDoItem>>();
        var controller = new ToDoItemsController(repositoryMock);
        repositoryMock.ReadById(Arg.Any<int>()).Throws(new Exception());
        var someId = 1;
        var item = new ToDoItemUpdateRequestDto("Updated name", "Updated description", false);

        //Act
        var result = controller.UpdateById(someId, item);

        //Assert
        repositoryMock.Received(1).ReadById(someId);
        repositoryMock.Received(0).UpdateById(someId, item.ToDomain());
        Assert.IsType<ObjectResult>(result);
        Assert.Equivalent(new StatusCodeResult(StatusCodes.Status500InternalServerError), result);
    }

    [Fact]
    public void Put_UpdateByIdThrowsException_ReturnsInternalServerError()
    {
        // Arrange
        var repositoryMock = Substitute.For<IRepository<ToDoItem>>();
        var controller = new ToDoItemsController(repositoryMock);
        repositoryMock.ReadById(Arg.Any<int>()).Returns(new ToDoItem { Name = "Test name", Description = "Test description", IsCompleted = false });
        repositoryMock.When(repo => repo.UpdateById(Arg.Any<int>(), Arg.Any<ToDoItem>())).Do(call => { throw new Exception(); });
        var someId = 1;
        var item = new ToDoItemUpdateRequestDto("Updated name", "Updated description", false);

        // Act
        var result = controller.UpdateById(someId, item);
        // Assert

        repositoryMock.Received(1).ReadById(someId);
        repositoryMock.Received(1).UpdateById(someId, Arg.Is<ToDoItem>(x => x.Name == item.Name && x.Description == item.Description && x.IsCompleted == item.IsCompleted));
        Assert.IsType<ObjectResult>(result);
        Assert.Equivalent(new StatusCodeResult(StatusCodes.Status500InternalServerError), result);
    }

}
