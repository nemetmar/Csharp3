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

public class DeleteTests_Unit
{
    [Fact]
    public async void Delete_DeleteByIdValidItemId_ReturnsNoContent()
    {
        //Arrange
        var repositoryMock = Substitute.For<IRepositoryAsync<ToDoItem>>();
        var controller = new ToDoItemsController(repositoryMock);
        repositoryMock.ReadByIdAsync(Arg.Any<int>()).Returns(new ToDoItem { Name = "Test name", Description = "Test description", IsCompleted = false, Category = "Homework" });
        var someId = 1;

        //Act
        var result = await controller.DeleteByIdAsync(someId);

        //Assert
        await repositoryMock.Received(1).ReadByIdAsync(someId);
        await repositoryMock.Received(1).DeleteByIdAsync(someId);
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async void Delete_DeleteByIdInvalidItemId_ReturnsNotFound()
    {
        //Arrange
        var repositoryMock = Substitute.For<IRepositoryAsync<ToDoItem>>();
        var controller = new ToDoItemsController(repositoryMock);
        repositoryMock.ReadByIdAsync(Arg.Any<int>()).ReturnsNull();
        var someId = 1;

        //Act
        var result = await controller.DeleteByIdAsync(someId);

        //Assert
        await repositoryMock.Received(1).ReadByIdAsync(someId);
        await repositoryMock.Received(0).DeleteByIdAsync(someId);
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async void Delete_DeleteByIdUnhandledException_ReturnsInternalServerError()
    {
        //Arrange
        var repositoryMock = Substitute.For<IRepositoryAsync<ToDoItem>>();
        var controller = new ToDoItemsController(repositoryMock);
        repositoryMock.ReadByIdAsync(Arg.Any<int>()).Throws(new Exception());
        var someId = 1;

        //Act
        var result = await controller.DeleteByIdAsync(someId);

        //Assert
        await repositoryMock.Received(1).ReadByIdAsync(someId);
        await repositoryMock.Received(0).DeleteByIdAsync(someId);
        Assert.IsType<ObjectResult>(result);
        Assert.Equivalent(new StatusCodeResult(StatusCodes.Status500InternalServerError), result);
    }

    [Fact]
    public async void Delete_DeleteByIdThrowsException_ReturnsInternalServerError()
    {
        // Arrange
        var repositoryMock = Substitute.For<IRepositoryAsync<ToDoItem>>();
        var controller = new ToDoItemsController(repositoryMock);
        repositoryMock.ReadByIdAsync(Arg.Any<int>()).Returns(new ToDoItem { Name = "Test name", Description = "Test description", IsCompleted = false, Category = "Homework" });
        //Tady mi musela poradit AI, sama bych to asi nesestavila, zasekla jsem se na tom, že nemohu použít .Throws na void metodě a nenapadlo mě nic lepšího, než upravit metody, což je mi jasné, že není cesta:
        //Teď koukám, že to samé máme v Post testu na Create, která je taky void, tam mě něnapadlo se podívat, ale jsem asi ráda, že mi to vysvětlila ještě jedna "entita" :D
        repositoryMock.When(repo => repo.DeleteByIdAsync(Arg.Any<int>())).Do(call => { throw new Exception(); });

        var someId = 1;

        // Act
        var result = await controller.DeleteByIdAsync(someId);
        // Assert

        await repositoryMock.Received(1).ReadByIdAsync(someId);
        await repositoryMock.Received(1).DeleteByIdAsync(someId);
        Assert.IsType<ObjectResult>(result);
        Assert.Equivalent(new StatusCodeResult(StatusCodes.Status500InternalServerError), result);
    }

}
