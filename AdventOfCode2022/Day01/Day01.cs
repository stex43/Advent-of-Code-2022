namespace AdventOfCode2022.Day01;

public sealed class Day01 : Solver
{
    private const int SnackElvesAmount = 3;
    
    public Day01() 
        : base(nameof(Day01).Substring(3, 2))
    {
    }

    protected override object Solve1Internal(StreamReader input)
    {
        
        var max = 0;
        var currentCalories = 0;
        
        while (!input.EndOfStream)
        {
            var line = input.ReadLine();

            if (string.IsNullOrWhiteSpace(line))
            {
                currentCalories = 0;
                continue;
            }

            currentCalories += int.Parse(line);
            max = Math.Max(max, currentCalories);
        }

        return max;
    }

    protected override object Solve2Internal(StreamReader input)
    {
        var queue = new PriorityQueue<int, int>();
        var currentCalories = 0;
        
        while (!input.EndOfStream)
        {
            var line = input.ReadLine();

            if (string.IsNullOrWhiteSpace(line))
            {
                PutInMaxQueue(queue, currentCalories);
                
                currentCalories = 0;
                continue;
            }

            currentCalories += int.Parse(line);
        }
        
        PutInMaxQueue(queue, currentCalories);

        var sum = 0;
        while (queue.TryDequeue(out var value, out _))
        {
            sum += value;
        }

        return sum;
    }

    private static void PutInMaxQueue(PriorityQueue<int, int> queue, int amount)
    {
        queue.Enqueue(amount, amount);

        if (queue.Count > SnackElvesAmount)
        {
            queue.Dequeue();
        }
    }
}