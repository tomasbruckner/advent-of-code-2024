using System.Text;

namespace AdventOfCode.Day15;

public static class Part1
{
    private static readonly Dictionary<char, (int offsetX, int offsetY)> Directions = new()
    {
        { 'v', (0, 1) },
        { '<', (-1, 0) },
        { '>', (1, 0) },
        { '^', (0, -1) },
    };

    public static long Solve(string input)
    {
        var lines = input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
            .ToList();
        var (matrix, moves) = GetPuzzle(lines);
        UpdatePositions(matrix, moves);
        var result = CalculateResult(matrix);

        return result;
    }

    private static (List<List<char>> matrix, string moves) GetPuzzle(List<string> lines)
    {
        var matrix = new List<List<char>>();
        var stringBuilder = new StringBuilder();
        foreach (var line in lines)
        {
            if (line.StartsWith('#'))
            {
                matrix.Add(line.ToCharArray().ToList());
            }
            else
            {
                stringBuilder.Append(line);
            }
        }

        return (matrix, stringBuilder.ToString());
    }

    private static void UpdatePositions(List<List<char>> matrix, string moves)
    {
        var (x, y) = FindStart(matrix);

        foreach (var move in moves)
        {
            var (offsetX, offsetY) = Directions[move];
            var newX = x + offsetX;
            var newY = y + offsetY;
            var isEmpty = matrix[newY][newX] == '.';
            var isMoveableObject = matrix[newY][newX] == 'O' && TryToPush(matrix, Directions[move], (newX, newY));
            if (isEmpty || isMoveableObject)
            {
                matrix[y][x] = '.';
                matrix[newY][newX] = '@';
                x = newX;
                y = newY;
            }
        }
    }

    private static bool TryToPush(List<List<char>> matrix, (int offsetX, int offsetY) move, (int x, int y) point)
    {
        var (offsetX, offsetY) = move;
        var (x, y) = point;
        var newX = x + offsetX;
        var newY = y + offsetY;

        while (true)
        {
            switch (matrix[newY][newX])
            {
                case '.':
                    matrix[y][x] = '.';
                    matrix[newY][newX] = 'O';
                    return true;
                case '#':
                    return false;
                default:
                    newX += offsetX;
                    newY += offsetY;
                    break;
            }
        }
    }

    private static (int x, int y) FindStart(List<List<char>> matrix)
    {
        for (var y = 0; y < matrix.Count; y++)
        {
            for (var x = 0; x < matrix[y].Count; x++)
            {
                if (matrix[y][x] == '@')
                {
                    return (x, y);
                }
            }
        }

        throw new Exception("Missing start");
    }

    private static long CalculateResult(List<List<char>> matrix)
    {
        var result = 0L;
        for (var y = 0; y < matrix.Count; y++)
        {
            for (var x = 0; x < matrix[y].Count; x++)
            {
                if (matrix[y][x] == 'O')
                {
                    result += x + 100 * y;
                }
            }
        }

        return result;
    }
}