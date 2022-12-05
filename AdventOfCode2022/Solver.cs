namespace AdventOfCode2022;

public abstract class Solver
{
    private readonly string DayNumber;
    
    protected Solver(string dayNumber)
    {
        this.DayNumber = dayNumber;
    }

    public object Solve1()
    {
        using var streamReader = new StreamReader($"Day{this.DayNumber}/input.txt");

        return this.Solve1Internal(streamReader);
    }
    
    public object Solve2()
    {
        using var streamReader = new StreamReader($"Day{this.DayNumber}/input.txt");

        return this.Solve2Internal(streamReader);
    }

    protected abstract object Solve1Internal(StreamReader input);
    
    protected abstract object Solve2Internal(StreamReader input);
}