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
    public void Get_ReadWhenSomeItemAvailable_ReturnsOk()
    {
        //Arrange
        var repositoryMock = Substitute.For<IRepository<ToDoItem>>();
        var controller = new ToDoItemsController(repositoryMock);

        //repositoryMock.When().Do();           //genericke kdyz-tak
        //repositoryMock.Read().Returns();      // nastavujeme return value
        //repositoryMock.Read().Throws();       //vyhazujeme výjimku
        //repositoryMock.Received().Read();     //kontrolujeme zavolání metody
        //repositoryMock.Read().ReturnsNull();   //nastavuje return null

        repositoryMock.Read().Returns([new ToDoItem { Name = "Test name", Description = "Test description", IsCompleted = false, Category = "Homework" }]);

        //Act
        var result = controller.Read();
        var resultResult = result.Result;

        //Assert
        Assert.IsType<OkObjectResult>(resultResult);
        repositoryMock.Received(1).Read();  //otestuje, že ta metoda byla skutečně zavolaná (právě jednou)
    }


    [Fact]
    public void Get_ReadWhenNoItemAvailable_ReturnsNotFound()
    {
        //Arrange
        var repositoryMock = Substitute.For<IRepository<ToDoItem>>();
        var controller = new ToDoItemsController(repositoryMock);
        repositoryMock.Read().ReturnsNull();


        //Act
        var result = controller.Read();
        var resultResult = result.Result;

        //Assert
        Assert.IsType<NotFoundResult>(resultResult);
        repositoryMock.Received(1).Read();
    }



    [Fact]
    public void Get_ReadUnhandledException_ReturnsInternalServerError()
    {
        //Arrange
        var repositoryMock = Substitute.For<IRepository<ToDoItem>>();
        var controller = new ToDoItemsController(repositoryMock);
        repositoryMock.Read().Throws(new Exception());

        //Act
        var result = controller.Read();
        var resultResult = result.Result;

        //Assert
        Assert.IsType<ObjectResult>(resultResult);
        repositoryMock.Received(1).Read();
        Assert.Equivalent(new StatusCodeResult(StatusCodes.Status500InternalServerError), resultResult);
    }


}
