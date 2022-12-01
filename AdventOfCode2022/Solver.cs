namespace AdventOfCode2022;

public abstract class Solver
{
    protected readonly StreamReader InputStream;

    protected Solver(string dayNumber)
    {
        this.InputStream = new StreamReader($"Day{dayNumber}/input.txt");
    }

    public abstract object Solve1();
    
    public abstract object Solve2();
}