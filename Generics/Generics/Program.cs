// See https://aka.ms/new-console-template for more information

IEnumerable<string> strings = ["test2", "test3", "test4"];
IEnumerable<int> integers = [2, 3, 4];
var stringoveInt = integers.Select(i => i.ToString());

Console.WriteLine(Max(3, -3));

Console.WriteLine(Max("3", "-3"));

void WriteToConsole<T>(IEnumerable<T> list) where T: IComparable<T>
{
    foreach (var s in list)
    {
        Console.Write(s.ToString() + " ");
    }
}

T Max<T>(T a, T b) where T : IComparable<T>
{
    return a.CompareTo(b) > 0 ? a : b;
}


T MaxCollection<T>(IEnumerable<T> values) where T : IComparable<T>
{
    return values.Max();
}

bool IsDistinct<T>(IEnumerable<T> vlaues)  where T : IComparable<T>

{
    HashSet<T> seenValues = new HashSet<T>();
    foreach (var value in values)
    {
        if(!seenValues.Add(value))
        {
            return false;
        }
    }
    return true;
}

