using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

public class GreedGame
{
    private List<int> throws;
    private int[] counts;
    private readonly int[] tripletPoints = new int[] {1000, 200, 300, 400, 500, 600};
    private readonly int[] singlePoints = new int[] {100, 0, 0, 0, 50, 0};

    public void ThrowDice(int numberOfDice)
    {
        counts = new int[6];
        throws = new List<int>(numberOfDice);
        for(int i = 0; i < numberOfDice; i++)
        {
            Random rnd = new Random();
            int value = rnd.Next(1, 7);
            throws.Add(value);
            counts[value-1]++;
        }

    }

    public void Evaluate()
    {
        int score = 0;
        for(int i = 0; i < counts.Length; i++)
        {
            if(counts[i] >= 3)
            {
                score = score + tripletPoints[i];
                counts[i] = counts[i] - 3;
            }
            while(counts[i] > 0 && singlePoints[i] > 0)
            {
                score = score + singlePoints[i];
                counts[i]--;
            }
        }
        Console.WriteLine("Výsledek: " + score);
    }

    public void Show()
    {
        foreach(int item in throws)
        {
            Console.Write("|" + item);
        }
        Console.WriteLine("|");

        for(int i = 0; i < counts.Length; i++)
        {
            Console.WriteLine((i + 1) + ": " + counts[i]);
        }
    }
}
