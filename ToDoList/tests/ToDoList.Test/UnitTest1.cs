namespace ToDoList.Test;

using Microsoft.VisualStudio.TestPlatform.ObjectModel;

public class UnitTest1
{
    [Fact]
    public void Divide_WithioutReminder_Succeeds()
    {
        // Arrange
        var calculator = new Calculator();

        //Act
        var result = calculator.Divide(10, 5);

        //Assert
        Assert.Equal(2, result);

    }

    [Fact]
    public void DivideFloat_ByZero_ReturnsInfinity()
    {
        // Arrange
        var calculator = new Calculator();

        //Act
        var result = calculator.DivideFloat(10, 0);

        //Assert
        Assert.Equal(float.PositiveInfinity, result);
    }


    [Fact]
    public void Test3()
    {
        // Arrange
        var calculator = new Calculator();

        //Act
        var result = calculator.Divide(10, 5) == 3;

        //Assert
        Assert.False(result);
    }

    [Theory]
    [InlineData(10, 5)]
    [InlineData(30, 15)]
    public void Divide_WithioutReminder_Succeeds_Theory(int value1, int value2)
    {
        // Arrange
        var calculator = new Calculator();

        //Act
        var result = calculator.Divide(value1, value2);

        //Assert
        Assert.Equal(2, result);
    }

}



public class Calculator
{
    public int Divide(int dividend, int divisor)
    {
        return (dividend / divisor);
    }

    public float DivideFloat(float dividend, float divisor)
    {
        return (dividend / divisor);
    }

}
