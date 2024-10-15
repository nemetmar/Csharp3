using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangman;




public static class Input
{

    public static string ReadAToZLetter(string message)
    {
        string letter = "";
        bool isLetter = false;
        while (!isLetter)
        {
            Console.WriteLine(message);
            letter = Console.ReadLine();
            if (letter.Length > 1)
            {
                Console.WriteLine("V jednom kole lze zadat pouze jedno písmeno.");
            }
            else if(string.IsNullOrEmpty(letter))
            {
                Console.WriteLine("Musíš zadat jedno písmeno.");
            }
            else if(!char.IsLetter(letter[0]))
            {
                Console.WriteLine("Zadej prosím jedno písmeno, jiné znaky v tajném slově nejsou.");
            }
            else
            {
                isLetter = true;
            }
        }


        return letter.ToUpper();
    }

    public static string ReadUniqueAToZLetter(List<Guess> guesses, string message)
    {
        string letter = "";
        bool duplicateEntry = true;
        while (duplicateEntry)
        {
            letter = ReadAToZLetter(message);
            duplicateEntry = guesses.FindIndex(i => i.Letter == letter) != -1;
            if(duplicateEntry)
            {
                Console.WriteLine("Toto písmeno už bylo zadáno, zkus jiné.");
                Output.DisplayUsedLetters(guesses);
            }
        }
        return letter;
    }

}

