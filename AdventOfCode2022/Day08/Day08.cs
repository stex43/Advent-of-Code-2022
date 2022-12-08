namespace AdventOfCode2022.Day08;

public sealed class Day08 : Solver
{
    public Day08() 
        : base(nameof(Day08).Substring(3, 2))
    {
    }

    protected override object Solve1Internal(StreamReader input)
    {
        var trees = GetSquareTreesHeightMap(input);
        var mapSize = trees.GetLength(0);

        var visibilityTreeMap = new int[mapSize, mapSize];

        for (var i = 0; i < mapSize; i++)
        {
            MarkBorderTrees(i, mapSize, visibilityTreeMap);
            
            // from the left
            var maxHeightFromSide = trees[i, 0];
            for (var j = 1; j < mapSize - 1; j++)
            {
                if (trees[i, j] > maxHeightFromSide)
                {
                    visibilityTreeMap[i, j] = 1;
                }

                maxHeightFromSide = Math.Max(maxHeightFromSide, trees[i, j]);
            }
            
            // from the right
            maxHeightFromSide = trees[i, mapSize - 1];
            for (var j = mapSize - 2; j >= 0; j--)
            {
                if (trees[i, j] > maxHeightFromSide)
                {
                    visibilityTreeMap[i, j] = 1;
                }

                maxHeightFromSide = Math.Max(maxHeightFromSide, trees[i, j]);
            }
            
            // from the up
            maxHeightFromSide = trees[0, i];
            for (var j = 1; j < mapSize - 1; j++)
            {
                if (trees[j, i] > maxHeightFromSide)
                {
                    visibilityTreeMap[j, i] = 1;
                }

                maxHeightFromSide = Math.Max(maxHeightFromSide, trees[j, i]);
            }
            
            // from the down
            maxHeightFromSide = trees[mapSize - 1, i];
            for (var j = mapSize - 2; j >= 0; j--)
            {
                if (trees[j, i] > maxHeightFromSide)
                {
                    visibilityTreeMap[j, i] = 1;
                }

                maxHeightFromSide = Math.Max(maxHeightFromSide, trees[j, i]);
            }
        }

        return visibilityTreeMap.Cast<int>().Count(x => x == 1);
    }

    protected override object Solve2Internal(StreamReader input)
    {
        var trees = GetSquareTreesHeightMap(input);
        var mapSize = trees.GetLength(0);

        var scenicScoreTreeMap = new int[mapSize, mapSize];

        for (var i = 1; i < mapSize - 1; i++)
        {
            for (var j = 1; j < mapSize - 1; j++)
            {
                var visibleFromUp = 1;
                var visibleFromDown = 1;
                var visibleFromRight = 1;
                var visibleFromLeft = 1;
                
                var currentTreeHeight = trees[i, j];
                
                // from the left
                for (var k = j - 1; k >= 0; k--)
                {
                    visibleFromLeft = j - k;
                    
                    var iteratedTreeHeight = trees[i, k];
                    if (iteratedTreeHeight >= currentTreeHeight)
                    {
                        break;
                    }
                }

                // from the right
                for (var k = j + 1; k < mapSize; k++)
                {
                    visibleFromRight = k - j;
                    
                    var iteratedTreeHeight = trees[i, k];
                    if (iteratedTreeHeight >= currentTreeHeight)
                    {
                        break;
                    }
                }
                
                // from the up
                for (var k = i - 1; k >= 0; k--)
                {
                    visibleFromUp = i - k;
                    
                    var iteratedTreeHeight = trees[k, j];
                    if (iteratedTreeHeight >= currentTreeHeight)
                    {
                        break;
                    }
                }

                // from the down
                for (var k = i + 1; k < mapSize; k++)
                {
                    visibleFromDown = k - i;
                    
                    var iteratedTreeHeight = trees[k, j];
                    if (iteratedTreeHeight >= currentTreeHeight)
                    {
                        break;
                    }
                }
                
                scenicScoreTreeMap[i, j] = visibleFromRight * visibleFromLeft * visibleFromUp * visibleFromDown;
            }
        }
        
        return scenicScoreTreeMap.Cast<int>().Max();
    }

    private static int[,] GetSquareTreesHeightMap(StreamReader input)
    {
        int[,]? treesMap = null;

        var i = 0;
        while (!input.EndOfStream)
        {
            var line = input.ReadLine()!;

            if (treesMap == null)
            {
                var count = line.Length;
                treesMap = new int[count, count];
            }
            
            var j = 0;
            foreach (var tree in line)
            {
                var height = tree - '0';
                treesMap[i, j] = height;
                
                j++;
            }

            i++;
        }

        return treesMap!;
    }

    private static void MarkBorderTrees(int i, int mapSize, int[,] visibilityMap)
    {
        visibilityMap[0, i] = 1;
        visibilityMap[i, 0] = 1;
        visibilityMap[i, mapSize - 1] = 1;
        visibilityMap[mapSize - 1, i] = 1;
    }
}