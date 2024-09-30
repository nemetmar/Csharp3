using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


public static class Input
{

    public static int ReadPositiveInteger(string message)
    {
        bool isNumber = false;
        int number = 0;
        while (!isNumber || number <= 0)
        {
            Console.WriteLine(message);
            string vstup = Console.ReadLine();
            isNumber = int.TryParse(vstup, out number);
        }


        return number;
    }

}

