namespace AdventOfCode.Day18;

public static class Part2
{
    private const char Blocked = '#';

    private static readonly List<(int offsetX, int offsetY)> Directions =
    [
        (-1, 0),
        (1, 0),
        (0, -1),
        (0, 1),
    ];

    public static void Solve(string input, int size)
    {
        var blockedList = GetBlocked(input);
        var start = 0;
        var end = blockedList.Count;
        while (true)
        {
            var take = (end - start) / 2 + start;
            var blockedMap = blockedList
                .Take(take)
                .ToDictionary(x => x, _ => true);
            var matrix = CreateMatrix(size, blockedMap);
            var result = FindShortest(matrix);
            if (result == -1)
            {
                end = take;
            }
            else
            {
                start = take;
            }

            if (start == end - 1)
            {
                Console.WriteLine($"{blockedList[start].Item1},{blockedList[start].Item2} - index {start}");
                return;
            }
        }
    }

    private static List<(long, long)> GetBlocked(string input)
    {
        return input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
            .Select(o =>
            {
                var parts = o.Split(",");
                return (long.Parse(parts[0]), long.Parse(parts[1]));
            }).ToList();
    }

    private static List<List<char>> CreateMatrix(int size, Dictionary<(long, long), bool> blocked)
    {
        var matrix = new List<List<char>>();
        for (var y = 0; y < size; y++)
        {
            var row = new List<char>();
            for (var x = 0; x < size; x++)
            {
                row.Add(blocked.ContainsKey((x, y)) ? Blocked : '.');
            }

            matrix.Add(row);
        }

        return matrix;
    }

    private static long FindShortest(List<List<char>> matrix)
    {
        var end = (matrix.Count - 1, matrix.Count - 1);
        var toSearch = new PriorityQueue<((int x, int y), int score), int>();
        toSearch.Enqueue(((0, 0), 0), 0);
        var visited = new Dictionary<(int x, int y), int>
        {
            [(0, 0)] = 0,
        };

        do
        {
            var (current, score) = toSearch.Dequeue();
            if (current == end)
            {
                return score;
            }

            foreach (var direction in Directions)
            {
                var newX = current.x + direction.offsetX;
                var newY = current.y + direction.offsetY;
                if (OutOfRange(matrix, (newX, newY)) || matrix[newY][newX] == Blocked)
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

    private static bool OutOfRange(List<List<char>> matrix, (int x, int y) point)
    {
        var (x, y) = point;

        return x < 0 || y > matrix.Count - 1 || y < 0 || x > matrix[y].Count - 1;
    }
}