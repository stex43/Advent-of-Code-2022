using System.Text;

namespace AdventOfCode2022.Day05;

public sealed class Day05 : Solver
{
    public Day05()
        : base(nameof(Day05).Substring(3, 2))
    {
    }

    protected override object Solve1Internal(StreamReader input)
    {
        var piles = CreatePiles(input);

        while (!input.EndOfStream)
        {
            var line = input.ReadLine()!;
            var craneAssignment = CreateCraneAssignment(line);

            for (var i = 0; i < craneAssignment.CrateAmount; i++)
            {
                var crate = piles[craneAssignment.FromPile].Pop();
                piles[craneAssignment.ToPile].Push(crate);
            }
        }

        return GetUpperCrates(piles);
    }

    protected override object Solve2Internal(StreamReader input)
    {
        var piles = CreatePiles(input);

        while (!input.EndOfStream)
        {
            var line = input.ReadLine()!;
            var craneAssignment = CreateCraneAssignment(line);

            var t = new Stack<char>(craneAssignment.CrateAmount);
            for (var i = 0; i < craneAssignment.CrateAmount; i++)
            {
                var crate = piles[craneAssignment.FromPile].Pop();
                t.Push(crate);
            }

            while (t.TryPop(out var crate))
            {
                piles[craneAssignment.ToPile].Push(crate);
            }
        }
        
        return GetUpperCrates(piles);
    }

    private static List<Stack<char>> CreatePiles(StreamReader input)
    {
        var piles = new List<Stack<char>>(9);
        var inputPiles = new List<Stack<char>>(9);

        for (var i = 0; i < 9; i++)
        {
            piles.Add(new Stack<char>());
            inputPiles.Add(new Stack<char>());
        }

        while (true)
        {
            var line = input.ReadLine()!;

            // line of crate numbers
            if (line.Contains('1'))
            {
                break;
            }

            var crateNumber = 0;
            for (var i = 0; i < line.Length; i++)
            {
                // skip square parentheses
                i++;
                var crate = line[i];

                if (char.IsLetter(crate))
                {
                    inputPiles[crateNumber].Push(crate);
                }

                crateNumber++;

                // skip square parentheses
                i += 2;
            }
        }

        // skip empty line
        input.ReadLine();

        for (var i = 0; i < 9; i++)
        {
            while (inputPiles[i].TryPop(out var crate))
            {
                piles[i].Push(crate);
            }
        }

        return piles;
    }

    private static CraneAssignment CreateCraneAssignment(string line)
    {
        var split = line.Split(new[] { "move ", "from ", "to " }, StringSplitOptions.TrimEntries);

        var amount = int.Parse(split[1]);
        var from = int.Parse(split[2]);
        var to = int.Parse(split[3]);

        return new CraneAssignment(amount, from - 1, to - 1);
    }

    private static string GetUpperCrates(List<Stack<char>> piles)
    {
        var stringBuilder = new StringBuilder(9);
        for (var i = 0; i < 9; i++)
        {
            var crate = piles[i].Peek();
            stringBuilder.Append(crate);
        }

        return stringBuilder.ToString();
    }

    private class CraneAssignment
    {
        public int CrateAmount { get; }

        public int FromPile { get; }

        public int ToPile { get; }

        public CraneAssignment(int crateAmount, int fromPile, int toPile)
        {
            this.CrateAmount = crateAmount;
            this.FromPile = fromPile;
            this.ToPile = toPile;
        }
    }
}