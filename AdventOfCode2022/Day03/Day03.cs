namespace AdventOfCode2022.Day03;

public sealed class Day03 : Solver
{
    public Day03()
        : base(nameof(Day03).Substring(3, 2))
    {
    }

    protected override object Solve1Internal(StreamReader input)
    {
        var sum = 0;
        
        while (!input.EndOfStream)
        {
            var line = input.ReadLine();

            var length = line!.Length;
            var compartment1 = line.Substring(0, length / 2);
            var compartment2 = line.Substring(length / 2, length / 2);

            var set1 = new HashSet<char>(compartment1);
            var set2 = new HashSet<char>(compartment2);
            var itemIntersection = set1.Intersect(set2);

            foreach (var item in itemIntersection)
            {
                sum += CountPriority(item);
            }
        }
        
        return sum;
    }

    protected override object Solve2Internal(StreamReader input)
    {
        var sum = 0;
        
        while (!input.EndOfStream)
        {
            var line1 = input.ReadLine();
            var line2 = input.ReadLine();
            var line3 = input.ReadLine();

            var set1 = new HashSet<char>(line1!);
            var set2 = new HashSet<char>(line2!);
            var set3 = new HashSet<char>(line3!);

            var itemIntersection = set1.Intersect(set2).Intersect(set3);

            foreach (var item in itemIntersection)
            {
                sum += CountPriority(item);
            }
        }
        
        return sum;
    }

    private static int CountPriority(char item)
    {
        if (char.IsLower(item))
        {
            return item - 'a' + 1;
        }

        return item - 'A' + 27;
    }
}