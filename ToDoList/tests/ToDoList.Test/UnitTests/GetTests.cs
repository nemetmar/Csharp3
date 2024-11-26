namespace ToDoList.Test;

using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.Models;
using ToDoList.WebApi;
using NSubstitute;
using ToDoList.Persistence.Repositories;
using Microsoft.AspNetCore.Http;
using NSubstitute.ReturnsExtensions;
using NSubstitute.ExceptionExtensions;

public class GetTests_Unit
{
    [Fact]
    public async void Get_ReadWhenSomeItemAvailable_ReturnsOk()
    {
        //Arrange
        var repositoryMock = Substitute.For<IRepositoryAsync<ToDoItem>>();
        var controller = new ToDoItemsController(repositoryMock);

        //repositoryMock.When().Do();           //genericke kdyz-tak
        //repositoryMock.Read().Returns();      // nastavujeme return value
        //repositoryMock.Read().Throws();       //vyhazujeme výjimku
        //repositoryMock.Received().Read();     //kontrolujeme zavolání metody
        //repositoryMock.Read().ReturnsNull();   //nastavuje return null

        repositoryMock.ReadAsync().Returns([new ToDoItem { Name = "Test name", Description = "Test description", IsCompleted = false, Category = "Homework" }]);

        //Act
        var result = await controller.ReadAsync();
        var resultResult = result.Result;

        //Assert
        Assert.IsType<OkObjectResult>(resultResult);
        await repositoryMock.Received(1).ReadAsync();  //otestuje, že ta metoda byla skutečně zavolaná (právě jednou)
    }


    [Fact]
    public async void Get_ReadWhenNoItemAvailable_ReturnsNotFound()
    {
        //Arrange
        var repositoryMock = Substitute.For<IRepositoryAsync<ToDoItem>>();
        var controller = new ToDoItemsController(repositoryMock);
        repositoryMock.ReadAsync().ReturnsNull();


        //Act
        var result = await controller.ReadAsync();
        var resultResult = result.Result;

        //Assert
        Assert.IsType<NotFoundResult>(resultResult);
        await repositoryMock.Received(1).ReadAsync();
    }



    [Fact]
    public async void Get_ReadUnhandledException_ReturnsInternalServerError()
    {
        //Arrange
        var repositoryMock = Substitute.For<IRepositoryAsync<ToDoItem>>();
        var controller = new ToDoItemsController(repositoryMock);
        repositoryMock.ReadAsync().Throws(new Exception());

        //Act
        var result = await controller.ReadAsync();
        var resultResult = result.Result;

        //Assert
        Assert.IsType<ObjectResult>(resultResult);
        await repositoryMock.Received(1).ReadAsync();
        Assert.Equivalent(new StatusCodeResult(StatusCodes.Status500InternalServerError), resultResult);
    }


}
