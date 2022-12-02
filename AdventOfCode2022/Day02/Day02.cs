namespace AdventOfCode2022.Day02;

public sealed class Day02 : Solver
{
    public Day02(string dayNumber)
        : base(dayNumber)
    {
    }

    private readonly int[,] shapeScoringMap =
    {
        { 3, 0, 6 },
        { 6, 3, 0 },
        { 0, 6, 3 }
    };

    private readonly Dictionary<char, int> shapeScores = new()
    {
        { 'A', 1 },
        { 'B', 2 },
        { 'C', 3 },
        { 'X', 1 },
        { 'Y', 2 },
        { 'Z', 3 }
    };

    private readonly Dictionary<char, int> matchResultScores = new()
    {
        { 'X', 0 },
        { 'Y', 3 },
        { 'Z', 6 }
    };

    public override object Solve1()
    {
        var score = 0;
        while (!InputStream.EndOfStream)
        {
            var line = InputStream.ReadLine();

            var elfShapeScore = shapeScores[line[0]];
            var mineShapeScore = shapeScores[line[2]];

            score += mineShapeScore;

            var elfShapeNumber = elfShapeScore - 1;
            var mineShapeNumber = mineShapeScore - 1;

            score += shapeScoringMap[mineShapeNumber, elfShapeNumber];
        }

        return score;
    }

    public override object Solve2()
    {
        var score = 0;
        
        while (!InputStream.EndOfStream)
        {
            var line = InputStream.ReadLine();

            var elfShapeScore = shapeScores[line[0]];
            var matchResult = matchResultScores[line[2]];

            score += matchResult;

            var elfShapeNumber = elfShapeScore - 1;

            for (var i = 0; i < 3; i++)
            {
                if (shapeScoringMap[i, elfShapeNumber] != matchResult)
                {
                    continue;
                }

                var mineShapeScore = i + 1;
                score += mineShapeScore;
                break;
            }
        }

        return score;
    }
}