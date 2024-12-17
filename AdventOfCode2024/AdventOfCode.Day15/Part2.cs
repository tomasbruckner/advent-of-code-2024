using System.Text;

namespace AdventOfCode.Day15;

public static class Part2
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
        ExpandMap(matrix);
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

    private static void ExpandMap(List<List<char>> matrix)
    {
        foreach (var line in matrix)
        {
            for (var x = 0; x < line.Count; x++)
            {
                switch (line[x])
                {
                    case '#':
                        line.Insert(x + 1, '#');
                        break;
                    case '@':
                    case '.':
                        line.Insert(x + 1, '.');
                        break;
                    case 'O':
                        line[x] = '[';
                        line.Insert(x + 1, ']');
                        break;
                }

                x++;
            }
        }
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
            var isObject = matrix[newY][newX] == '[' || matrix[newY][newX] == ']';
            var isMoveableObject = isObject && TryToPush(matrix, move, (newX, newY));
            if (isEmpty || isMoveableObject)
            {
                matrix[y][x] = '.';
                matrix[newY][newX] = '@';
                x = newX;
                y = newY;
            }

        }
    }

    private static bool TryToPush(List<List<char>> matrix, char move, (int x, int y) point)
    {
        return move is '<' or '>' ? TryPushHorizontal(matrix, move, point) : TryPushVertical(matrix, move, point);
    }

    private static bool TryPushHorizontal(List<List<char>> matrix, char move, (int x, int y) point)
    {
        var (offsetX, _) = Directions[move];
        var (x, y) = point;
        var newX = x;

        do
        {
            if (OutOfRange(matrix, (newX, y)))
            {
                return false;
            }

            switch (matrix[y][newX])
            {
                case '.':
                {
                    var previous = matrix[y][x];
                    matrix[y][x] = '.';
                    var i = x;
                    do
                    {
                        i += offsetX;
                        (matrix[y][i], previous) = (previous, matrix[y][i]);
                        if (i == newX)
                        {
                            return true;
                        }
                    } while (true);
                }
                case '#':
                {
                    return false;
                }
                default:
                {
                    newX += offsetX;
                    break;
                }
            }
        } while (true);
    }

    private static bool TryPushVertical(List<List<char>> matrix, char move, (int x, int y) point)
    {
        var (_, offsetY) = Directions[move];
        var (x, y) = point;
        var isLeftBracket = matrix[y][x] == '[';
        var layer = new HashSet<(int x, int y)>
        {
            (x, y),
            isLeftBracket ? (x + 1, y) : (x - 1, y),
        };
        var toMove = new List<HashSet<(int x, int y)>> { layer };

        var newY = y + offsetY;
        do
        {
            if (OutOfRange(matrix, (x, newY)))
            {
                return false;
            }

            var newLayer = new HashSet<(int x, int y)>();
            foreach (var (indexX, _) in layer)
            {
                switch (matrix[newY][indexX])
                {
                    case '#':
                        return false;
                    case '[':
                        newLayer.Add((indexX, newY));
                        newLayer.Add((indexX + 1, newY));
                        break;
                    case ']':
                        newLayer.Add((indexX - 1, newY));
                        newLayer.Add((indexX, newY));
                        break;
                }
            }

            if (newLayer.Count == 0)
            {
                UpdateVertical(matrix, move, point, toMove);
                return true;
            }

            var (firstX, firstY) = newLayer.First();
            if (matrix[firstY][firstX] == ']')
            {
                newLayer.Add((firstX - 1, firstY));
            }

            var (lastX, lastY) = newLayer.Last();
            if (matrix[lastY][lastX] == '[')
            {
                newLayer.Add((lastX + 1, lastY));
            }

            layer = newLayer.OrderBy(o => o.x).ToHashSet();
            toMove.Add(layer);
            newY += offsetY;
        } while (true);
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

    private static void UpdateVertical(List<List<char>> matrix, char move, (int x, int y) point,
        List<HashSet<(int x, int y)>> layer)
    {
        var (_, offsetY) = Directions[move];

        for (var i = layer.Count - 1; i >= 0; i--)
        {
            foreach (var (indexX, indexY) in layer[i])
            {
                var newY = indexY + offsetY;
                matrix[newY][indexX] = matrix[indexY][indexX];
                matrix[indexY][indexX] = '.';
            }
        }

        if (matrix[point.y][point.x] == '[')
        {
            matrix[point.y][point.x + 1] = '.';
        }
        else if (matrix[point.y][point.x] == ']')
        {
            matrix[point.y][point.x - 1] = '.';
        }

        matrix[point.y][point.x] = '.';
    }

    private static long CalculateResult(List<List<char>> matrix)
    {
        var result = 0L;
        for (var y = 0; y < matrix.Count; y++)
        {
            for (var x = 0; x < matrix[y].Count; x++)
            {
                if (matrix[y][x] == '[')
                {
                    result += x + 100 * y;
                }
            }
        }

        return result;
    }


    private static bool OutOfRange(List<List<char>> matrix, (int x, int y) point)
    {
        var (x, y) = point;

        return x < 0 || y > matrix.Count - 1 || y < 0 || x > matrix[y].Count - 1;
    }
}