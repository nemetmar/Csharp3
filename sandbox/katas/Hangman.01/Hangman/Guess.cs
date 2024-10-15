using System;

namespace Hangman;

public class Guess
{
    public string Letter {get; private set;}
    public bool IsCorrectGuess {get; private set;}

    public Guess (string letter, bool isCorrectGuess)
    {
        Letter = letter;
        IsCorrectGuess = isCorrectGuess;
    }
}
