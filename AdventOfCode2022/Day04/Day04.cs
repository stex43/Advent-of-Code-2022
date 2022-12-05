namespace AdventOfCode2022.Day04;

public sealed class Day04 : Solver
{
    public Day04()
        : base(nameof(Day04).Substring(3, 2))
    {
    }

    protected override object Solve1Internal(StreamReader input)
    {
        
        var count = 0;

        while (!input.EndOfStream)
        {
            var line = input.ReadLine()!;
            var splitLine = line.Split(',');

            var firstElfRange = GetAssignmentRange(splitLine[0]);
            var secondElfRange = GetAssignmentRange(splitLine[1]);

            if (IsFullyContained(firstElfRange, secondElfRange)
                || IsFullyContained(secondElfRange, firstElfRange))
            {
                count++;
            }
        }

        return count;
    }

    protected override object Solve2Internal(StreamReader input)
    {
        
        var count = 0;

        while (!input.EndOfStream)
        {
            var line = input.ReadLine()!;
            var splitLine = line.Split(',');

            var firstElfRange = GetAssignmentRange(splitLine[0]);
            var secondElfRange = GetAssignmentRange(splitLine[1]);

            if (IsLeftBoundContained(firstElfRange, secondElfRange)
                || IsLeftBoundContained(secondElfRange, firstElfRange))
            {
                count++;
            }
        }

        return count;
    }

    private static AssignmentRange GetAssignmentRange(string assignment)
    {
        var assignmentSplit = assignment.Split('-');

        return new AssignmentRange(int.Parse(assignmentSplit[0]), int.Parse(assignmentSplit[1]));
    }

    private static bool IsFullyContained(AssignmentRange content, AssignmentRange container)
    {
        return content.Left >= container.Left && content.Right <= container.Right;
    }

    private static bool IsLeftBoundContained(AssignmentRange content, AssignmentRange container)
    {
        return content.Left >= container.Left && content.Left <= container.Right;
    }

    private class AssignmentRange
    {
        public int Left { get; }

        public int Right { get; }

        public AssignmentRange(int left, int right)
        {
            this.Left = left;
            this.Right = right;
        }
    }
}