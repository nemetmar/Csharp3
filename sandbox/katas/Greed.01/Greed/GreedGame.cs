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
    // S .NET 8 prisla collection initialization a int[], ale napriklad aj List<int> je mozne napisat aj sposobom nizsie.
    // V projekte v praci sme nedavno na .NET 8 presli a osobne to rada pouzivam
    private readonly int[] tripletPoints = [1000, 200, 300, 400, 500, 600]; // pekne pouzitie readonly, chvalim :)
    private readonly int[] singlePoints = new int[] { 100, 0, 0, 0, 50, 0 };

    public void ThrowDice(int numberOfDice)
    {
        counts = new int[6];
        throws = new List<int>(numberOfDice);
        for (int i = 0; i < numberOfDice; i++)
        {
            Random rnd = new Random();
            /*
            Nevieme, ci ste to uz mali na nejakych hodinach, ale premenne sa casto inicializuju pomocou "var". Kompilator si uz z pravej casti odvodi, co to je za typ
            Priklad dolu: kompilator vie, ze rnd.Next() vrati int, takze je mozne napisat var value.
            Nejde to napisat, ked napr. priradzujes null. Vtedy kompilator nevie, co to je za null, takze clovek musi napisat presny typ, teda napr. int? value = null;
            Budeme sa tomu na hodinach venovat, pripadne si to mozeme dovysvetlit mimo ak to nebude jasne :)
            */
            var value = rnd.Next(1, 7);
            throws.Add(value);
            /*
            Syntakticky analyzator mi navrhoval opravit formatovanie, dat medzery. Odporucam ho pouzivat, skusat, co dokaze opravit, prispieva to k uhladnosti kodu. :)
            Kliknut na podciarknute miesto a dat Ctrl + . alebo po kliknuti by sa nalavo mala zobrazit ziarovka, kliknut na nu -> objavi sa okienko Quick fix
            */
            counts[value - 1]++;
        }

    }

    public void Evaluate()
    {
        int score = 0;
        for (int i = 0; i < counts.Length; i++)
        {
            if (counts[i] >= 3)
            {
                // Ak do nejakej hodnoty priradzujes tu istu hodnotu navysenu o nejake cisle, vyuziva sa skor tento zapis:
                score += tripletPoints[i];
                // Vola sa to compound assignment: https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/operators/assignment-operator#compound-assignment
                counts[i] = counts[i] - 3;
            }
            while (counts[i] > 0 && singlePoints[i] > 0)
            {
                score = score + singlePoints[i];
                counts[i]--;
            }
        }
        Console.WriteLine("Výsledek: " + score);
    }

    public void Show()
    {
        /*
        Pri tomto som bola chvilu zmatena, co mi to vlastne zobrazuje, na prvu mi nedoslo, ze prvy riadok zobrazuje hod a druhy pocetnost nejakej hodnoty.
        Navrhla by som preto pridavat v takychto situaciach nejaky kratky popis, priklad:
        Hod kockou: |5|5|3|3|5|
        Pocet jednotlivych hodnot v hode:
        1: 0
        2: 0 ... atd.
        */
        foreach (int item in throws)
        {
            Console.Write("|" + item);
        }
        Console.WriteLine("|");

        for (int i = 0; i < counts.Length; i++)
        {
            Console.WriteLine(i + 1 + ": " + counts[i]); // syntakticky analyzator mi navrhoval, ze nie su nutne zatvorky
        }
    }
}
