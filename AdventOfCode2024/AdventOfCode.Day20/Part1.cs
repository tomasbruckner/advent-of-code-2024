namespace AdventOfCode.Day20;

public static class Part1
{
    private static readonly List<(int offsetX, int offsetY)> ValidSteps =
    [
        (0, 1), (0, -1), (1, 0), (-1, 0),
    ];

    private const char Blocked = '#';

    public static long Solve(string input, int threshold)
    {
        var matrix = input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
            .Select(x => x.ToCharArray().ToList())
            .ToList();
        var (start, end) = FindStartEnd(matrix);
        var toRemove = FindToRemove(matrix);
        var validSteps = CalculateSteps(matrix, start, end);
        var result = 0L;
        foreach (var (x, y) in toRemove)
        {
            matrix[y][x] = '.';
            var steps = CalculateSteps(matrix, start, end);
            matrix[y][x] = Blocked;
            var diff = validSteps - steps;
            if (diff >= threshold)
            {
                result += 1;
            }
        }


        return result;
    }

    private static List<(int x, int y)> FindToRemove(List<List<char>> matrix)
    {
        var toRemove = new List<(int x, int y)>();

        for (var y = 1; y < matrix.Count - 1; y++)
        {
            for (var x = 1; x < matrix[y].Count - 1; x++)
            {
                if (matrix[y][x] != Blocked)
                {
                    continue;
                }

                var horizontally = matrix[y][x - 1] != Blocked && matrix[y][x + 1] != Blocked;
                var vertically = matrix[y - 1][x] != Blocked && matrix[y + 1][x] != Blocked;
                if (horizontally || vertically)
                {
                    toRemove.Add((x, y));
                }
            }
        }

        return toRemove;
    }

    private static long CalculateSteps(List<List<char>> matrix, (int x, int y) start, (int x, int y) end)
    {
        var toSearch = new PriorityQueue<((int x, int y), long score), long>();
        toSearch.Enqueue((start, 0), 0);
        var visited = new Dictionary<(int x, int y), long>
        {
            [start] = 0,
        };

        do
        {
            var (current, score) = toSearch.Dequeue();
            if (current == end)
            {
                return score;
            }

            foreach (var direction in ValidSteps)
            {
                var newX = current.x + direction.offsetX;
                var newY = current.y + direction.offsetY;
                if (matrix[newY][newX] == Blocked)
                {
                    continue;
                }

                var newScore = score + 1;
                if (visited.TryGetValue((newX, newY), out var value) && value <= newScore)
                {
                    continue;
                }

                visited[(newX, newY)] = newScore;
                var newPriority = newScore + matrix.Count - newX + matrix.Count - newY;
                toSearch.Enqueue(((newX, newY), newScore), newPriority);
            }
        } while (toSearch.Count > 0);

        return -1;
    }

    private static ((int x, int y) start, (int x, int y) end) FindStartEnd(List<List<char>> matrix)
    {
        var start = (0, 0);
        var end = (0, 0);
        for (var y = 0; y < matrix.Count; y++)
        {
            for (var x = 0; x < matrix[y].Count; x++)
            {
                if (matrix[y][x] == 'S')
                {
                    start = (x, y);
                }
                else if (matrix[y][x] == 'E')
                {
                    end = (x, y);
                }
            }
        }

        return (start, end);
    }
}