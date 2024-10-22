using System;
using System.Runtime.CompilerServices;
using System.Text;

namespace Hangman;

public class HangmanGame
{
    public GameState CurrentState { get; private set; }
    public List<Guess> Guesses { get; private set; }
    private char[] guessedChars;
    public int MaxNumberOfBadGuesses { get; private set; }
    public int NumberOfBadGuesses { get; private set; }
    public string SecretWord { get; private set; }


    // premenovala som ti nazov parametru, aby bol v camelCase (v PascalCase byvaju iba nazvy properties). Tym padom nemusime pouzivat this lebo uz sa parameter nevola rovnako ako property
    public HangmanGame(int maxNumberOfGuesses, string secretWord)
    {
        CurrentState = GameState.Running;
        Guesses = new List<Guess>();
        // MaxNumberOfBadGuesses by mohla byt nejaka konstanta skor, a nie ako parameter
        MaxNumberOfBadGuesses = maxNumberOfGuesses;
        SecretWord = secretWord.ToUpper();
        guessedChars = new char[SecretWord.Length];
        for (int i = 0; i < SecretWord.Length; i++)
        {
            guessedChars[i] = '_';
        }
    }

    public void RunRound()
    {
        switch (CurrentState)
        {
            case GameState.Running:
                Output.DisplayCurrentGuessState(this);
                // zvazit moznost vracat rovno jeden char z ReadUniqueAToZLetter nez string, a potom v metodach nemusiet sa dotazovat na jedno pismeno cez letter[0]
                // EvaluateGuess a RefreshGuessWord by mohli brat ako parameter char namiesto string
                // aj trieda Guess by si mohla drzat iba char
                string letter = Input.ReadUniqueAToZLetter(Guesses, "Zadej písmeno: ");
                Guesses.Add(new Guess(letter, EvaluateGuess(letter)));
                Output.DisplayResult(this);
                UpdateCurrentState();
                break;

            case GameState.Won:
                Output.DisplayWonStatus(this);
                UpdateCurrentState();
                break;

            case GameState.Lost:
                Output.DisplayLostStatus(this);
                UpdateCurrentState();
                break;

            case GameState.Finished:
                break;
        }
    }


    private bool EvaluateGuess(string letter)
    {
        if (SecretWord.Contains(letter))
        {
            RefreshGuessWord(letter);
            return true;
        }
        NumberOfBadGuesses++;
        return false;
    }

    private void RefreshGuessWord(string letter)
    {
        bool lettersFound = true;
        int i = SecretWord.IndexOf(letter, StringComparison.InvariantCulture);
        while (lettersFound)
        {
            guessedChars[i] = letter[0];
            i = SecretWord.IndexOf(letter, i + 1, StringComparison.InvariantCulture);
            if (i == -1)
            {
                lettersFound = false;
            }
        }
    }


    private void UpdateCurrentState()
    {
        if (CurrentState == GameState.Won || CurrentState == GameState.Lost)
        {
            CurrentState = GameState.Finished;
        }
        // Nedalo by sa tu rovno pouzit guessedChars? Pristup k tomu mame a ReturnGuessWord uz nikde mimo triedu nepouzivas, takze nie je ho potreba
        else if (ReturnGuessWord().Equals(SecretWord))
        {
            CurrentState = GameState.Won;
        }
        else if (NumberOfBadGuesses == MaxNumberOfBadGuesses)
        {
            CurrentState = GameState.Lost;
        }
        else
        {
            CurrentState = GameState.Running;
        }
    }

    public string ReturnGuessWord()
    {
        StringBuilder sb = new StringBuilder();
        foreach (char item in guessedChars)
        {
            sb.Append(item);
        }
        return sb.ToString();
    }

}
