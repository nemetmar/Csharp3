namespace ToDoList.Test;

using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.Models;
using ToDoList.WebApi;
using NSubstitute;
using ToDoList.Persistence.Repositories;
using Microsoft.AspNetCore.Http;
using NSubstitute.ReturnsExtensions;
using NSubstitute.ExceptionExtensions;

public class GetByIdTests_Unit
{
    [Fact]
    public async void Get_ReadByIdWhenSomeItemAvailable_ReturnsOk()
    {
        //Arrange
        var repositoryMock = Substitute.For<IRepositoryAsync<ToDoItem>>();
        var controller = new ToDoItemsController(repositoryMock);
        repositoryMock.ReadByIdAsync(Arg.Any<int>()).Returns(new ToDoItem { Name = "Test name", Description = "Test description", IsCompleted = false, Category = "Homework" });
        var someId = 1;

        //Act
        var result = await controller.ReadByIdAsync(someId);

        //Assert
        Assert.IsType<OkObjectResult>(result);
        await repositoryMock.Received(1).ReadByIdAsync(someId);
    }


    [Fact]

    public async void Get_ReadByIdWhenItemIsNull_ReturnsNotFound()
    {
        //Arrange
        var repositoryMock = Substitute.For<IRepositoryAsync<ToDoItem>>();
        var controller = new ToDoItemsController(repositoryMock);
        repositoryMock.ReadByIdAsync(Arg.Any<int>()).ReturnsNull();
        var someId = 1;

        //Act
        var result = await controller.ReadByIdAsync(someId);

        //Assert
        Assert.IsType<NotFoundResult>(result);
        await repositoryMock.Received(1).ReadByIdAsync(someId);
    }



    [Fact]

    public async void Get_ReadByIdUnhandledException_ReturnsInternalServerError()
    {
        //Arrange
        var repositoryMock = Substitute.For<IRepositoryAsync<ToDoItem>>();
        var controller = new ToDoItemsController(repositoryMock);
        repositoryMock.ReadByIdAsync(Arg.Any<int>()).Throws(new Exception());
        var someId = 1;

        //Act
        var result = await controller.ReadByIdAsync(someId);

        //Assert
        Assert.IsType<ObjectResult>(result);
        await repositoryMock.Received(1).ReadByIdAsync(someId);
        Assert.Equivalent(new StatusCodeResult(StatusCodes.Status500InternalServerError), result);
    }


}
