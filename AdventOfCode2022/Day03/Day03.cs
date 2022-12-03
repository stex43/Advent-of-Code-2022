namespace AdventOfCode2022.Day03;

public sealed class Day03 : Solver
{
    public Day03(string dayNumber)
        : base(dayNumber)
    {
    }
    
    public override object Solve1()
    {
        var sum = 0;
        
        while (!InputStream.EndOfStream)
        {
            var line = InputStream.ReadLine();

            var length = line.Length;
            var compartment1 = line.Substring(0, length / 2);
            var compartment2 = line.Substring(length / 2, length / 2);

            var set1 = new HashSet<char>(compartment1);
            var set2 = new HashSet<char>(compartment2);
            var itemIntersection = set1.Intersect(set2);

            foreach (var item in itemIntersection)
            {
                if (char.IsLower(item))
                {
                    sum += item - 'a' + 1;
                }
                else
                {
                    sum += item - 'A' + 27;
                }
            }
        }
        
        return sum;
    }

    public override object Solve2()
    {
        var sum = 0;
        
        while (!InputStream.EndOfStream)
        {
            var line1 = InputStream.ReadLine();
            var line2 = InputStream.ReadLine();
            var line3 = InputStream.ReadLine();

            var set1 = new HashSet<char>(line1);
            var set2 = new HashSet<char>(line2);
            var set3 = new HashSet<char>(line3);

            var doubles = set1.Intersect(set2).Intersect(set3);

            foreach (var d in doubles)
            {
                if (char.IsLower(d))
                {
                    sum += d - 'a' + 1;
                }
                else
                {
                    sum += d - 'A' + 27;
                }
            }
        }
        
        return sum;
    }
}