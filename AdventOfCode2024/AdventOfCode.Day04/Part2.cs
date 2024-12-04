namespace AdventOfCode.Day04;

public static class Part2
{
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
                if (matrix[i][j] is 'M' or 'S' && GetXmas(matrix, i, j))
                {
                    result += 1;
                }
            }
        }

        return result;
    }

    private static bool GetXmas(List<List<char>> matrix, int y, int x)
    {
        var leftTopM = matrix[y][x] == 'M';
        var leftTopS = matrix[y][x] == 'S';
        var rightTopM = IsValid(matrix, x + 2, y, 'M');
        var rightTopS = IsValid(matrix, x + 2, y, 'S');
        var foundA = IsValid(matrix, x + 1, y + 1, 'A');
        var leftBottomM = IsValid(matrix, x, y + 2, 'M');
        var leftBottomS = IsValid(matrix, x, y + 2, 'S');
        var rightBottomM = IsValid(matrix, x + 2, y + 2, 'M');
        var rightBottomS = IsValid(matrix, x + 2, y + 2, 'S');
        var validFirstDiagonal = (leftTopM && foundA && rightBottomS) || (leftTopS && foundA && rightBottomM);
        var validSecondDiagonal = (rightTopM && foundA && leftBottomS) || (rightTopS && foundA && leftBottomM);

        return validFirstDiagonal && validSecondDiagonal;
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