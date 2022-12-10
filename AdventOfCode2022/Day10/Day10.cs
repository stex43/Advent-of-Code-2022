namespace AdventOfCode2022.Day10;

public sealed class Day10 : Solver
{
    public Day10()
        : base(nameof(Day10).Substring(3, 2))
    {
    }

    protected override object Solve1Internal(StreamReader input)
    {
        var cycle = 0;
        var commandQueue = new Queue<Command>();
        var x = 1;
        var signalStrengthSum = 0;

        while (!input.EndOfStream || commandQueue.Count != 0)
        {
            cycle++;

            if ((cycle - 20) % 40 == 0)
            {
                var signalStrength = cycle * x;
                signalStrengthSum += signalStrength;
            }

            ReadAndEnqueueCommand(input, commandQueue); 
            x = ExecuteCommand(commandQueue, x);
        }

        return signalStrengthSum;
    }

    protected override object Solve2Internal(StreamReader input)
    {
        var cycle = 0;
        var commandQueue = new Queue<Command>();
        var x = 1;

        while (!input.EndOfStream || commandQueue.Count != 0)
        {
            cycle++;

            ReadAndEnqueueCommand(input, commandQueue);
            x = ExecuteCommand(commandQueue, x);
            
            Console.Write(IsCurrentPixelContainedInSprite(cycle, x) ? '#' : '.');

            if (cycle % 40 == 0)
            {
                Console.WriteLine();
            }
        }

        return "Done!";
    }

    private static void ReadAndEnqueueCommand(StreamReader input, Queue<Command> commandQueue)
    {
        var line = input.ReadLine();
        if (!string.IsNullOrWhiteSpace(line))
        {
            commandQueue.Enqueue(CreateCommand(line));
        }
    }

    private static int ExecuteCommand(Queue<Command> commandQueue, int x)
    {
        if (!commandQueue.TryPeek(out var executingCommand))
        {
            return x;
        }

        executingCommand.Cycle++;
        if (executingCommand.Cycle == 2)
        {
            commandQueue.Dequeue();
            return x + executingCommand.Value;
        }

        return x;
    }

    private static Command CreateCommand(string line)
    {
        var split = line.Split();
        
        if (split[0] == "addx")
        {
            return new Command(int.Parse(split[1])) { Cycle = 0 };
        }
        else
        {
            return new Command(0) { Cycle = 1 };
        }
    }

    private static bool IsCurrentPixelContainedInSprite(int cycle, int x)
    {
        return Math.Abs(cycle % 40 - x) <= 1;
    }

    private class Command
    {
        public int Cycle { get; set; }

        public int Value { get; }

        public Command(int value)
        {
            this.Value = value;
        }
    }
}