namespace AdventOfCode.Day20;

public static class Part2
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
        var validSteps = CalculateSteps(matrix, start, end);
        var result = CalculateResult(validSteps, threshold);

        return result;
    }

    private static List<(int x, int y)> CalculateSteps(List<List<char>> matrix, (int x, int y) start,
        (int x, int y) end)
    {
        var steps = new List<(int x, int y)>();
        var current = start;
        while (true)
        {
            steps.Add(current);
            if (current == end)
            {
                break;
            }

            foreach (var direction in ValidSteps)
            {
                var newX = current.x + direction.offsetX;
                var newY = current.y + direction.offsetY;
                var newPoint = (newX, newY);
                var previous = steps.Count > 1 && steps[^2] == newPoint;
                if (matrix[newY][newX] == Blocked || previous)
                {
                    continue;
                }

                current = newPoint;
                break;
            }
        }

        return steps;
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

    private static long CalculateResult(List<(int x, int y)> validSteps, int threshold)
    {
        var mapPath = CreateMapPath(validSteps);
        var result = 0L;
        foreach (var (x, y) in validSteps)
        {
            for (var newY = y - 20; newY <= y + 20; newY++)
            {
                for (var newX = x - 20; newX <= x + 20; newX++)
                {
                    var coordDiff = Math.Abs(newX - x) + Math.Abs(newY - y);
                    if (coordDiff > 20 || !mapPath.ContainsKey((newX, newY)))
                    {
                        continue;
                    }
                    
                    var valueDiff = mapPath[(newX, newY)] - mapPath[(x, y)] - coordDiff;
                    if (valueDiff >= threshold)
                    {
                        result += 1;
                    } 
                }
            }
        }
        
        return result;
    }
    
    private static Dictionary<(int x, int y), long> CreateMapPath(List<(int x, int y)> validSteps)
    {
        var mapPath = new Dictionary<(int x, int y), long>();

        for (var i = 0; i < validSteps.Count; i++)
        {
            mapPath[validSteps[i]] = validSteps.Count - i;
        }

        return mapPath;
    }
}