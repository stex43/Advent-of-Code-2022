namespace AdventOfCode2022.Day05;

public sealed class Day05 : Solver
{
    public Day05()
        : base(nameof(Day05).Substring(3, 2))
    {
    }

    protected override object Solve1Internal(StreamReader input)
    {
        var piles = new List<Stack<char>>(9);
        var q = new List<Stack<char>>(9);

        for (var i = 0; i < 9; i++)
        {
            piles.Add(new Stack<char>());
            q.Add(new Stack<char>());
        }
        
        while (!input.EndOfStream)
        {
            var line = input.ReadLine();

            if (!string.IsNullOrWhiteSpace(line))
            {
                var crateNumber = 0;
                for (var i = 0; i < line.Length; i++)
                {
                    i++;
                    var crate = line[i];

                    if (char.IsDigit(crate))
                    {
                        break;
                    }

                    if (char.IsLetter(crate))
                    {
                        //piles[crateNumber].Push(crate);
                        q[crateNumber].Push(crate);
                    }

                    i++;
                    i++;
                    crateNumber++;
                }
            }
            else
            {
                break;
            }
        }
        
        for (var i = 0; i < 9; i++)
        {
            while (q[i].TryPop(out var crate))
            {
                piles[i].Push(crate);
            }
        }

        while (!input.EndOfStream)
        {
            var line = input.ReadLine();
            var split = line.Split(new[] { "move ", "from ", "to " }, StringSplitOptions.TrimEntries);

            var count = int.Parse(split[1]);
            var from = int.Parse(split[2]);
            var to = int.Parse(split[3]);

            for (var i = 0; i < count; i++)
            {
                var crate = piles[from - 1].Pop();
                piles[to - 1].Push(crate);
            }
        }

        var s = string.Empty;
        for (var i = 0; i < 9; i++)
        {
            var crate = piles[i].Peek();
            s += crate;
        }

        return s;
    }

    protected override object Solve2Internal(StreamReader input)
    {
        var piles = new List<Stack<char>>(9);
        var q = new List<Stack<char>>(9);

        for (var i = 0; i < 9; i++)
        {
            piles.Add(new Stack<char>());
            q.Add(new Stack<char>());
        }
        
        while (!input.EndOfStream)
        {
            var line = input.ReadLine();

            if (!string.IsNullOrWhiteSpace(line))
            {
                var crateNumber = 0;
                for (var i = 0; i < line.Length; i++)
                {
                    i++;
                    var crate = line[i];

                    if (char.IsDigit(crate))
                    {
                        break;
                    }

                    if (char.IsLetter(crate))
                    {
                        //piles[crateNumber].Push(crate);
                        q[crateNumber].Push(crate);
                    }

                    i++;
                    i++;
                    crateNumber++;
                }
            }
            else
            {
                break;
            }
        }
        
        for (var i = 0; i < 9; i++)
        {
            while (q[i].TryPop(out var crate))
            {
                piles[i].Push(crate);
            }
        }

        while (!input.EndOfStream)
        {
            var line = input.ReadLine();
            var split = line.Split(new[] { "move ", "from ", "to " }, StringSplitOptions.TrimEntries);

            var count = int.Parse(split[1]);
            var from = int.Parse(split[2]);
            var to = int.Parse(split[3]);

            var t = new Stack<char>();
            for (var i = 0; i < count; i++)
            {
                var crate = piles[from - 1].Pop();
                t.Push(crate);
            }

            while (t.TryPop(out var crate))
            {
                piles[to -1].Push(crate);
            }
        }

        var s = string.Empty;
        for (var i = 0; i < 9; i++)
        {
            piles[i].TryPeek(out var crate);
            s += crate;
        }

        return s;
    }
}