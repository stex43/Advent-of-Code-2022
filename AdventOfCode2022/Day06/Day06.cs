namespace AdventOfCode2022.Day06;

public sealed class Day06 : Solver
{
    public Day06() 
        : base(nameof(Day06).Substring(3, 2))
    {
    }

    protected override object Solve1Internal(StreamReader input)
    {
        var line = input.ReadLine()!;

        return FindMarkerStart(line, 4);
    }

    protected override object Solve2Internal(StreamReader input)
    {
        var line = input.ReadLine()!;

        return FindMarkerStart(line, 14);
    }

    private static int FindMarkerStart(string line, int markerSize)
    {
        var markerStart = 0;

        for (var i = 0; i < line.Length - markerSize; i++)
        {
            var window = line.Substring(i, markerSize);
            var set = new HashSet<char>(window);
            if (set.Count != markerSize)
            {
                continue;
            }
            
            markerStart = i + markerSize;
            break;
        }

        return markerStart;
    }
}