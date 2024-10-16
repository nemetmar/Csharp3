using System;

namespace Hangman;

public static class Output
{
    public static void DisplayWelcome()
    {
        Console.WriteLine("========= Vítej ve hře OBĚŠENEC! =========");
    }
    public static void DisplayCurrentGuessState(HangmanGame game)
    {
        Console.WriteLine("Aktuální stav tajenky: " + game.ReturnGuessWord());
        Console.WriteLine("Můžeš se splést celkem " + (game.MaxNumberOfBadGuesses - game.NumberOfBadGuesses) + "x.");
    }

    public static void DisplayResult(HangmanGame game)
    {
        if (game.Guesses[game.Guesses.Count - 1].IsCorrectGuess)
        {
            Console.WriteLine("Gratuluji! Písmeno \"" + game.Guesses[game.Guesses.Count - 1].Letter + "\" se nachází v tajence.");
        }
        else
        {
            Console.WriteLine("Bohužel. Písmeno \"" + game.Guesses[game.Guesses.Count - 1].Letter + "\" není v tajence.");
        }
    }

    public static void DisplayWonStatus(HangmanGame game)
    {
        Console.WriteLine("Gratuluji k výhře!");
        Console.WriteLine("Správná odpověď: " + game.SecretWord);
    }

    public static void DisplayLostStatus(HangmanGame game)
    {
        Console.WriteLine("Hra skončila, pokusy vyčerpány.");
        Console.WriteLine("Správná odpověď: " + game.SecretWord);
    }

    public static void DisplayUsedLetters(List<Guess> guesses)
    {
        Console.Write("Již byla použita tato písmena: ");
        foreach(Guess item in guesses)
        {
            Console.Write("[" + item.Letter + "]");
        }
        Console.WriteLine();
    }

}
