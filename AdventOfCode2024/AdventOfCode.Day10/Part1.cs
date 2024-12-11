namespace AdventOfCode.Day10;

public static class Part1
{
    private static readonly List<(int offsetX, int offsetY)> Directions =
    [
        (1, 0),
        (0, 1),
        (-1, 0),
        (0, -1),
    ];

    public static long Solve(string input)
    {
        var matrix = input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
            .Select(x => x.ToCharArray().ToList())
            .ToList();

        var paths = GetPaths(matrix);

        var result = new HashSet<(int, int, int, int)>(paths.Select(x =>
        {
            var first = x.First();
            var last = x.Last();

            return (first.x, first.y, last.x, last.y);
        }));

        return result.Count;
    }


    private static List<HashSet<(int x, int y)>> GetPaths(List<List<char>> matrix)
    {
        var search = new Stack<HashSet<(int x, int y)>>();
        InitSearch(matrix, search);
        var correct = new List<HashSet<(int x, int y)>>();

        do
        {
            var current = search.Pop();
            var (x, y) = current.Last();
            if (matrix[y][x] == '9')
            {
                correct.Add(current);
                continue;
            }

            var newPaths = GetPaths(matrix, current);
            foreach (var path in newPaths)
            {
                var clone = new HashSet<(int x, int y)>(current) { path };
                search.Push(clone);
            }
        } while (search.Count != 0);

        return correct;
    }

    private static void InitSearch(List<List<char>> matrix, Stack<HashSet<(int x, int y)>> stack)
    {
        for (var i = 0; i < matrix.Count; i++)
        {
            for (var j = 0; j < matrix[i].Count; j++)
            {
                if (matrix[i][j] == '0')
                {
                    var point = (j, i);
                    stack.Push([point]);
                }
            }
        }
    }

    private static List<(int x, int y)> GetPaths(List<List<char>> matrix, HashSet<(int x, int y)> path)
    {
        var (x, y) = path.Last();
        var currentValue = matrix[y][x];
        var newPaths = new List<(int x, int y)>();

        foreach (var (offsetX, offsetY) in Directions)
        {
            var newPath = (x + offsetX, y + offsetY);
            if (OutOfRange(matrix, newPath))
            {
                continue;
            }

            var newValue = matrix[newPath.Item2][newPath.Item1];
            if (currentValue + 1 == newValue)
            {
                newPaths.Add(newPath);
            }
        }

        return newPaths;
    }

    private static bool OutOfRange(List<List<char>> matrix, (int x, int y) point)
    {
        var (x, y) = point;

        return x < 0 || y > matrix.Count - 1 || y < 0 || x > matrix[y].Count - 1;
    }
}