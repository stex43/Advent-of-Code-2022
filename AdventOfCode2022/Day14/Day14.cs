using AdventOfCode2022.Common;

namespace AdventOfCode2022.Day14;

public sealed class Day14 : Solver
{
    public Day14() 
        : base(nameof(Day14).Substring(3, 2))
    {
    }

    protected override object Solve1Internal(StreamReader input)
    {
        var map = CreateMap(input);

        var minimalLevel = GetMinimalLevel(map);

        var sandAmount = 0;
        while (!Fall(new Point(500, 0), map, minimalLevel, new Point(500, 0)))
        {
            sandAmount++;
        }

        return sandAmount;
    }

    protected override object Solve2Internal(StreamReader input)
    {
        var map = CreateMap(input);
        
        var minimalLevel = GetMinimalLevel(map) + 2;
        for (var j = 0; j < 1000; j++)
        {
            map[minimalLevel, j] = 1;
        }

        var sandAmount = 0;
        while (!Fall(new Point(500, 0), map, minimalLevel, new Point(500, 0)))
        {
            sandAmount++;
        }

        return sandAmount;
    }

    private static int[,] CreateMap(StreamReader input)
    {
        var map = new int[1000,1000];

        while (!input.EndOfStream)
        {
            var line = input.ReadLine()!;
            var split = line.Split(" -> ");

            for (var i = 0; i < split.Length - 1; i++)
            {
                var first = split[i].Split(",");
                var second = split[i + 1].Split(",");

                var firstX = int.Parse(first[0]);
                var firstY = int.Parse(first[1]);
                var secondX = int.Parse(second[0]);
                var secondY = int.Parse(second[1]);

                if (firstX == secondX)
                {
                    if (firstY > secondY)
                    {
                        for (var j = secondY; j <= firstY; j++)
                        {
                            map[j, firstX] = 1;
                        }
                    }
                    else
                    {
                        
                        for (var j = firstY; j <= secondY; j++)
                        {
                            map[j, firstX] = 1;
                        }
                    }
                }
                else
                {
                    if (firstX > secondX)
                    {
                        for (var j = secondX; j <= firstX; j++)
                        {
                            map[firstY, j] = 1;
                        }
                    }
                    else
                    {
                        for (var j = firstX; j <= secondX; j++)
                        {
                            map[firstY, j] = 1;
                        }
                    }
                }
            }
        }

        return map;
    }

    private static int GetMinimalLevel(int[,] map)
    {
        for (var i = map.GetLength(0) - 1; i >= 0; i--)
        {
            for (var j = 0; j < map.GetLength(1); j++)
            {
                if (map[i, j] == 1)
                {
                    return i;
                }
            }
        }

        return 0;
    }
    
    private static bool Fall(Point sandPosition, int[,] map, int minimalLevel, Point sandOrigin)
    {
        if (FallStrictlyDown(sandPosition, map, minimalLevel))
        {
            return true;
        }

        // fall to the left
        if (map[sandPosition.Y + 1, sandPosition.X - 1] == 0)
        {
            sandPosition.X--;
            sandPosition.Y++;

            if (sandPosition.Y >= minimalLevel)
            {
                return true;
            }
            
            Fall(sandPosition, map, minimalLevel, sandOrigin);
        }
        // fall to the right
        else if (map[sandPosition.Y + 1, sandPosition.X + 1] == 0)
        {
            sandPosition.X++;
            sandPosition.Y++;

            if (sandPosition.Y >= minimalLevel)
            {
                return true;
            }
            
            Fall(sandPosition, map, minimalLevel, sandOrigin);
        }

        if (sandPosition.Y >= minimalLevel || sandPosition == sandOrigin)
        {
            return true;
        }
        
        map[sandPosition.Y, sandPosition.X] = 2;

        return false;
    }

    private static bool FallStrictlyDown(Point sand, int[,] map, int min)
    {
        while (map[sand.Y + 1, sand.X] == 0)
        {      
            if (sand.Y >= min)
            {
                return true;
            }
            
            sand.Y++;
        }

        return false;
    }
}