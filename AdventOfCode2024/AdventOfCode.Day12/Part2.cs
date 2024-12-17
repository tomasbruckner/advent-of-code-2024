namespace AdventOfCode.Day12;

public static class Part2
{
    private static readonly List<(int offsetX, int offsetY)> Directions =
    [
        (1, 0),
        (0, 1),
        (-1, 0),
        (0, -1),
    ];

    private enum CornerEnum
    {
        Empty,
        Full,
    }

    private static readonly List<List<(int offsetX, int offsetY, CornerEnum cornerType)>> CornersInfo =
    [
        // .#
        // ##
        [(0, -1, CornerEnum.Full), (-1, 0, CornerEnum.Full), (-1, -1, CornerEnum.Empty)],

        // ?.
        // .#
        [(0, -1, CornerEnum.Empty), (-1, 0, CornerEnum.Empty)],

        // #.
        // ##
        [(0, -1, CornerEnum.Full), (1, 0, CornerEnum.Full), (1, -1, CornerEnum.Empty)],

        // .?
        // #.
        [(0, -1, CornerEnum.Empty), (1, 0, CornerEnum.Empty)],

        // ##
        // .#
        [(0, 1, CornerEnum.Full), (-1, 0, CornerEnum.Full), (-1, 1, CornerEnum.Empty)],

        // .#
        // ?.
        [(0, 1, CornerEnum.Empty), (-1, 0, CornerEnum.Empty)],

        // ##
        // #.
        [(0, 1, CornerEnum.Full), (1, 0, CornerEnum.Full), (1, 1, CornerEnum.Empty)],

        // #.
        // .?
        [(0, 1, CornerEnum.Empty), (1, 0, CornerEnum.Empty)],
    ];

    public static long Solve(string input)
    {
        var matrix = input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
            .Select(x => x.ToCharArray().ToList())
            .ToList();
        var areas = GetAreas(matrix);
        var result = CalculateResult(areas);

        return result;
    }

    private static List<List<(int x, int y)>> GetAreas(List<List<char>> matrix)
    {
        var areas = new List<List<(int x, int y)>>();
        var visited = new Dictionary<(int x, int y), bool>();

        for (var y = 0; y < matrix.Count; y++)
        {
            for (var x = 0; x < matrix[y].Count; x++)
            {
                if (visited.ContainsKey((x, y)))
                {
                    continue;
                }

                var area = GetArea(matrix, visited, (x, y));
                areas.Add(area);
            }
        }

        return areas;
    }

    private static List<(int x, int y)> GetArea(
        List<List<char>> matrix,
        Dictionary<(int x, int y), bool> visited,
        (int x, int y) start
    )
    {
        var symbol = matrix[start.y][start.x];
        visited[start] = true;
        var search = new Stack<(int x, int y)>();
        search.Push((start.x, start.y));
        var area = new List<(int x, int y)> { (start.x, start.y) };

        do
        {
            var current = search.Pop();

            foreach (var direction in Directions)
            {
                var newNode = (current.x + direction.offsetX, current.y + direction.offsetY);
                if (!IsValidAreaNode(matrix, visited, newNode, symbol))
                {
                    continue;
                }

                visited[newNode] = true;
                search.Push(newNode);
                area.Add(newNode);
            }
        } while (search.Count > 0);

        return area;
    }

    private static bool IsValidAreaNode(
        List<List<char>> matrix,
        Dictionary<(int x, int y), bool> visited,
        (int, int) newNode,
        char symbol
    )
    {
        if (OutOfRange(matrix, newNode))
        {
            return false;
        }

        if (matrix[newNode.Item2][newNode.Item1] != symbol)
        {
            return false;
        }

        return !visited.ContainsKey(newNode);
    }

    private static long CalculateResult(List<List<(int x, int y)>> areas)
    {
        var result = 0L;

        foreach (var area in areas)
        {
            result += area.Count * GetAreaBoundary(area);
        }

        return result;
    }

    private static long GetAreaBoundary(List<(int x, int y)> area)
    {
        var result = 0L;
        var pointMap = area.ToDictionary(x => x, _ => true);

        foreach (var point in area)
        {
            foreach (var cornerInfo in CornersInfo)
            {
                if (CheckCorner(cornerInfo, point, pointMap))
                {
                    result += 1;
                }
            }
        }


        return result;
    }

    private static bool CheckCorner(
        List<(int offsetX, int offsetY, CornerEnum cornerType)> cornerInfo,
        (int x, int y) point,
        Dictionary<(int x, int y), bool> pointMap
    )
    {
        foreach (var (offsetX, offsetY, cornerType) in cornerInfo)
        {
            var newPoint = (point.x + offsetX, point.y + offsetY);
            switch (cornerType)
            {
                case CornerEnum.Full when !pointMap.ContainsKey(newPoint):
                case CornerEnum.Empty when pointMap.ContainsKey(newPoint):
                    return false;
            }
        }

        return true;
    }

    private static bool OutOfRange(List<List<char>> matrix, (int x, int y) point)
    {
        var (x, y) = point;

        return x < 0 || y > matrix.Count - 1 || y < 0 || x > matrix[y].Count - 1;
    }
}