namespace AdventOfCode2022.Day12;

public sealed class Day12 : Solver
{
    public Day12()
        : base(nameof(Day12).Substring(3, 2))
    {
    }

    protected override object Solve1Internal(StreamReader input)
    {
        var labyrinth = CreateMap(input);

        var stepMap = CreateStepMap(labyrinth);

        return stepMap[labyrinth.End.Item1, labyrinth.End.Item2];
    }

    protected override object Solve2Internal(StreamReader input)
    {
        var labyrinth = CreateMap(input);
        var end = labyrinth.End;

        var minSteps = int.MaxValue;

        for (var i = 0; i < labyrinth.Map.GetLength(0); i++)
        {
            for (var j = 0; j < labyrinth.Map.GetLength(1); j++)
            {
                if (labyrinth.Map[i, j] != 0)
                {
                    continue;
                }

                labyrinth.Start = new Tuple<int, int>(i, j);
                var stepMap = CreateStepMap(labyrinth);

                labyrinth.Map[i,j] = int.MaxValue;
                if (stepMap[end.Item1, end.Item2] == 0)
                {
                    continue;
                }
                
                minSteps = Math.Min(minSteps, stepMap[end.Item1, end.Item2]);
            }
        }

        return minSteps;
    }

    private static Labyrinth CreateMap(StreamReader input)
    {
        var start = Tuple.Create(0, 0);
        var end = Tuple.Create(0, 0);
        var map = new List<List<int>>();

        var i = 0;
        while (!input.EndOfStream)
        {
            var line = input.ReadLine()!;

            map.Add(new List<int>());

            for (var j = 0; j < line.Length; j++)
            {
                var cell = line[j];

                if (cell == 'S')
                {
                    map[i].Add(0);
                    start = Tuple.Create(i, j);
                    continue;
                }

                if (cell == 'E')
                {
                    map[i].Add('z' - 'a');
                    end = Tuple.Create(i, j);
                    continue;
                }

                map[i].Add(cell - 'a');
            }

            i++;
        }

        var labyrinthMap = new int[map.Count, map.First().Count];

        for (i = 0; i < map.Count; i++)
        {
            for (var j = 0; j < map.First().Count; j++)
            {
                labyrinthMap[i, j] = map[i][j];
            }
        }

        return new Labyrinth(labyrinthMap, start, end);
    }

    private static int[,] CreateStepMap(Labyrinth labyrinth)
    {
        var current = labyrinth.Start;
        var visitQueue = new Queue<Tuple<int, int>>();
        visitQueue.Enqueue(labyrinth.Start);

        var height = labyrinth.Map.GetLength(0);
        var width = labyrinth.Map.GetLength(1);
        
        var stepMap = new int[height, width];
        var visitedMap = new int[height, width];

        while (!Equals(current, labyrinth.End))
        {
            if (!visitQueue.TryDequeue(out current))
            {
                break;
            }

            if (visitedMap[current.Item1, current.Item2] == 1)
            {
                continue;
            }

            visitedMap[current.Item1, current.Item2] = 1;
            if (Equals(current, labyrinth.End))
            {
                break;
            }

            var currentHeight = labyrinth.Map[current.Item1, current.Item2];
            var currentStep = stepMap[current.Item1, current.Item2];

            if (current.Item2 + 1 < width && labyrinth.Map[current.Item1, current.Item2 + 1] - currentHeight <= 1)
            {
                if (visitedMap[current.Item1, current.Item2 + 1] != 1)
                {
                    visitQueue.Enqueue(Tuple.Create(current.Item1, current.Item2 + 1));
                }

                var neighbourStepCount = stepMap[current.Item1, current.Item2 + 1];
                if (neighbourStepCount == 0 || neighbourStepCount < currentStep + 1)
                {
                    stepMap[current.Item1, current.Item2 + 1] = currentStep + 1;
                }
            }

            if (current.Item2 - 1 >= 0 && labyrinth.Map[current.Item1, current.Item2 - 1] - currentHeight <= 1)
            {
                if (visitedMap[current.Item1, current.Item2 - 1] != 1)
                {
                    visitQueue.Enqueue(Tuple.Create(current.Item1, current.Item2 - 1));
                }

                var neighbourStepCount = stepMap[current.Item1, current.Item2 - 1];
                if (neighbourStepCount == 0 || neighbourStepCount < currentStep + 1)
                {
                    stepMap[current.Item1, current.Item2 - 1] = currentStep + 1;
                }
            }

            if (current.Item1 + 1 < height && labyrinth.Map[current.Item1 + 1, current.Item2] - currentHeight <= 1)
            {
                if (visitedMap[current.Item1 + 1, current.Item2] != 1)
                {
                    visitQueue.Enqueue(Tuple.Create(current.Item1 + 1, current.Item2));
                }

                var neighbourStepCount = stepMap[current.Item1 + 1, current.Item2];
                if (neighbourStepCount == 0 || neighbourStepCount < currentStep + 1)
                {
                    stepMap[current.Item1 + 1, current.Item2] = currentStep + 1;
                }
            }

            if (current.Item1 - 1 >= 0 && labyrinth.Map[current.Item1 - 1, current.Item2] - currentHeight <= 1)
            {
                if (visitedMap[current.Item1 - 1, current.Item2] != 1)
                {
                    visitQueue.Enqueue(Tuple.Create(current.Item1 - 1, current.Item2));
                }

                var neighbourStepCount = stepMap[current.Item1 - 1, current.Item2];
                if (neighbourStepCount == 0 || neighbourStepCount < currentStep + 1)
                {
                    stepMap[current.Item1 - 1, current.Item2] = currentStep + 1;
                }
            }
        }

        return stepMap;
    }

    private class Labyrinth
    {
        public int[,] Map { get; }
        
        public Tuple<int, int> Start { get; set; }
        
        public Tuple<int, int> End { get; }

        public Labyrinth(int[,] map, Tuple<int, int> start, Tuple<int, int> end)
        {
            this.Map = map;
            this.Start = start;
            this.End = end;
        }
    }
}