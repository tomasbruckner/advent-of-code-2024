namespace AdventOfCode.Day08;

public static class Part1
{
    public static long Solve(string input)
    {
        var matrix = input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
            .Select(x => x.ToCharArray().ToList())
            .ToList();
        var symbols = GetSymbols(matrix);
        var antinodes = CalculateAntinodes(matrix, symbols);

        return antinodes.Count;
    }

    private static ILookup<char, (int x, int y)> GetSymbols(List<List<char>> matrix)
    {
        var symbolList = new List<(char c, int x, int y)>();
        for (var y = 0; y < matrix.Count; y++)
        {
            for (var x = 0; x < matrix[y].Count; x++)
            {
                if (matrix[y][x] != '.')
                {
                    symbolList.Add((matrix[y][x], x, y));
                }
            }
        }

        return symbolList.ToLookup(o => o.c, o => (o.x, o.y));
    }

    private static HashSet<(int x, int y)> CalculateAntinodes(
        List<List<char>> matrix,
        ILookup<char, (int x, int y)> symbols
    )
    {
        var antinodes = new List<(int x, int y)>();

        foreach (var group in symbols)
        {
            var newNodes = GenerateAntinodes(matrix, group.ToList());
            antinodes.AddRange(newNodes);
        }

        return antinodes.ToHashSet();
    }

    private static IEnumerable<(int x, int y)> GenerateAntinodes(
        List<List<char>> matrix,
        List<(int x, int y)> coords
    )
    {
        IEnumerable<(int x, int y)> antinodes = new HashSet<(int x, int y)>();

        for (var i = 0; i < coords.Count - 1; i++)
        {
            for (var j = i + 1; j < coords.Count; j++)
            {
                var newNodes = GenerateAntinodes(matrix, coords[i], coords[j]);
                antinodes = antinodes.Concat(newNodes);
            }
        }

        return antinodes;
    }

    private static IEnumerable<(int x, int y)> GenerateAntinodes(
        List<List<char>> matrix,
        (int x, int y) point1,
        (int x, int y) point2
    )
    {
        var newNodes = new List<(int x, int y)>();
        var (x1, y1) = point1;
        var (x2, y2) = point2;
        var xDiff = Math.Abs(x1 - x2);
        var yDiff = Math.Abs(y1 - y2);

        if (x1 == x2)
        {
            var newPoint1 = (x1, Math.Min(y1, y2) - yDiff);
            AddIfValid(newNodes, matrix, newPoint1);
            var newPoint2 = (x1, Math.Max(y1, y2) + yDiff);
            AddIfValid(newNodes, matrix, newPoint2);
        }
        else if (y1 == y2)
        {
            var newPoint1 = (Math.Min(x1, x2) - xDiff, y1);
            AddIfValid(newNodes, matrix, newPoint1);
            var newPoint2 = (Math.Max(x1, x2) + xDiff, y1);
            AddIfValid(newNodes, matrix, newPoint2);
        }
        else
        {
            var xCoef = x1 > x2 ? 1 : -1;
            var yCoef = y1 > y2 ? 1 : -1;
            var newPoint1 = (x1 + xCoef * xDiff, y1 + yCoef * yDiff);
            AddIfValid(newNodes, matrix, newPoint1);
            var newPoint2 = (x2 - xCoef * xDiff, y2 - yCoef * yDiff);
            AddIfValid(newNodes, matrix, newPoint2);
        }

        return newNodes;
    }

    private static void AddIfValid(List<(int x, int y)> coords, List<List<char>> matrix, (int x, int y) coord)
    {
        if (!OutOfRange(matrix, coord))
        {
            coords.Add(coord);
        }
    }

    private static bool OutOfRange(List<List<char>> matrix, (int x, int y) point)
    {
        var (x, y) = point;

        return x < 0 || y > matrix.Count - 1 || y < 0 || x > matrix[y].Count - 1;
    }
}