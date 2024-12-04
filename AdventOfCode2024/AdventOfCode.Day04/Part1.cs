namespace AdventOfCode.Day04;

public static class Part1
{
    private static readonly List<(int x, int y)> Directions =
    [
        (1, 0),
        (-1, 0),
        (0, 1),
        (0, -1),
        (1, 1),
        (-1, 1),
        (1, -1),
        (-1, -1),
    ];

    public static long Solve(string input)
    {
        var matrix = input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
            .Select(x => x.ToCharArray().ToList())
            .ToList();

        var result = GetResult(matrix);

        return result;
    }

    private static long GetResult(List<List<char>> matrix)
    {
        var result = 0L;
        for (var i = 0; i < matrix.Count; i++)
        {
            for (var j = 0; j < matrix[i].Count; j++)
            {
                if (matrix[i][j] == 'X')
                {
                    result += GetXmasCount(matrix, i, j);
                }
            }
        }

        return result;
    }

    private static int GetXmasCount(List<List<char>> matrix, int y, int x)
    {
        var result = 0;
        foreach (var (xOffset, yOffset) in Directions)
        {
            var valid = IsValid(matrix, x + xOffset, y + yOffset, 'M') &&
                        IsValid(matrix, x + 2 * xOffset, y + 2 * yOffset, 'A') &&
                        IsValid(matrix, x + 3 * xOffset, y + 3 * yOffset, 'S');
            if (valid)
            {
                result += 1;
            }
        }

        return result;
    }

    private static bool IsValid(List<List<char>> matrix, int x, int y, char c)
    {
        if (y > matrix.Count - 1 || y < 0 || x < 0 || x > matrix[y].Count - 1)
        {
            return false;
        }
        
        return matrix[y][x] == c;
    }
}