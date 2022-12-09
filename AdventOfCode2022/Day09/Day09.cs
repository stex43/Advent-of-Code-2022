namespace AdventOfCode2022.Day09;

public sealed class Day09 : Solver
{
    public Day09()
        : base(nameof(Day09).Substring(3, 2))
    {
    }

    protected override object Solve1Internal(StreamReader input)
    {
        var knotsCount = 2;
        var originOffset = 500;

        return Solve(knotsCount, originOffset, input);
    }

    protected override object Solve2Internal(StreamReader input)
    {
        var knotsCount = 10;
        var originOffset = 500;

        return Solve(knotsCount, originOffset, input);
    }

    private static int Solve(int knotsCount, int originOffset, StreamReader input)
    {
        var knots = new List<Point>(knotsCount);
        for (var i = 0; i < knotsCount; i++)
        {
            knots.Add(Point.Empty);
        }
        
        var tailVisitedMap = new int[originOffset * 2, originOffset * 2];
        tailVisitedMap[originOffset, originOffset] = 1;

        while (!input.EndOfStream)
        {
            var line = input.ReadLine()!;
            var split = line.Split(' ');
            
            var direction = split[0];
            var steps = int.Parse(split[1]);

            switch (direction)
            {
                case "D":
                    MoveHead(steps, knots, point => point.Y--, tailVisitedMap, originOffset);
                    break;
                
                case "U":
                    MoveHead(steps, knots, point => point.Y++, tailVisitedMap, originOffset);
                    break;
                
                case "R":
                    MoveHead(steps, knots, point => point.X++, tailVisitedMap, originOffset);
                    break;
                
                case "L":
                    MoveHead(steps, knots, point => point.X--, tailVisitedMap, originOffset);
                    break;
            }
        }

        return tailVisitedMap.Cast<int>().Sum();
    }

    private static void MoveHead(
        int steps,
        List<Point> knots,
        Action<Point> headMove,
        int[,] tailVisitedMap, 
        int originOffset)
    {
        for (var i = 0; i < steps; i++)
        {
            var knotsCount = knots.Count;
            headMove(knots[0]);
                    
            for (var knot = 1; knot < knotsCount; knot++)
            {
                if (IsNeighbours(knots[knot - 1], knots[knot]))
                {
                    break;
                }

                var head = knots[knot - 1];
                var tail = knots[knot];

                MoveToHead(head, tail);

                if (knot == knotsCount - 1)
                {
                    tailVisitedMap[tail.X + originOffset, tail.Y + originOffset] = 1;
                }
            }
        }
    }

    private static bool IsNeighbours(Point head, Point tail)
    {
        return Math.Max(Math.Abs(head.X - tail.X), Math.Abs(head.Y - tail.Y)) <= 1;
    }

    private static void MoveToHead(Point head, Point tail)
    {
        if (head.X == tail.X)
        {
            MoveToHeadOnY(head, tail);
        }
        else if (head.Y == tail.Y)
        {
            MoveToHeadOnX(head, tail);
        }
        else
        {
            MoveToHeadOnX(head, tail);
            MoveToHeadOnY(head, tail);
        }
    }

    private static void MoveToHeadOnY(Point head, Point tail)
    {
        if (head.Y > tail.Y)
        {
            tail.Y++;
        }
        else
        {
            tail.Y--;
        }
    }    
    
    private static void MoveToHeadOnX(Point head, Point tail)
    {
        if (head.X > tail.X)
        {
            tail.X++;
        }
        else if (head.X < tail.X)
        {
            tail.X--;
        }
    }
    
    private class Point
    {
        public int X { get; set; }
        
        public int Y { get; set; }

        public Point(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public static Point Empty => new(0, 0);

        public override string ToString()
        {
            return $"{this.X} {this.Y}";
        }
    }
}