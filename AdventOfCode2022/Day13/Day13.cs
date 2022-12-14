namespace AdventOfCode2022.Day13;

public sealed class Day13 : Solver
{
    public Day13()
        : base(nameof(Day13).Substring(3, 2))
    {
    }

    protected override object Solve1Internal(StreamReader input)
    {
        while (!input.EndOfStream)
        {
        }

        return 0;
    }

    protected override object Solve2Internal(StreamReader input)
    {
        while (!input.EndOfStream)
        {
        }

        return 0;
    }

    private class Signal
    {
        public int Value { get; set; }

        public List<Signal> List { get; set; } = new();
    }
}