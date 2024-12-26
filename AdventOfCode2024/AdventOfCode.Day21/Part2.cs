namespace AdventOfCode.Day21;

public static class Part2
{
    private static readonly Dictionary<(char from, char to), List<string>> MoveCache = new();
    private static readonly Dictionary<(char from, char to, int robotIndex), long> LengthCache = new();

    private static readonly Dictionary<char, List<(char newChar, char move)>> NumericMap = new()
    {
        { '7', [('8', '>'), ('4', 'v')] },
        { '8', [('7', '<'), ('9', '>'), ('5', 'v')] },
        { '9', [('8', '<'), ('6', 'v')] },
        { '4', [('7', '^'), ('5', '>'), ('1', 'v')] },
        { '5', [('4', '<'), ('8', '^'), ('6', '>'), ('2', 'v')] },
        { '6', [('5', '<'), ('9', '^'), ('3', 'v')] },
        { '1', [('4', '^'), ('2', '>')] },
        { '2', [('1', '<'), ('5', '^'), ('3', '>'), ('0', 'v')] },
        { '3', [('2', '<'), ('6', '^'), ('A', 'v')] },
        { '0', [('2', '^'), ('A', '>')] },
        { 'A', [('0', '<'), ('3', '^')] },
    };

    private static readonly Dictionary<char, List<(char newChar, char move)>> DirectionMap = new()
    {
        { '^', [('A', '>'), ('v', 'v')] },
        { 'A', [('^', '<'), ('>', 'v')] },
        { '<', [('v', '>')] },
        { 'v', [('<', '<'), ('>', '>'), ('^', '^')] },
        { '>', [('v', '<'), ('A', '^')] },
    };

    private static readonly List<Dictionary<char, List<(char newChar, char move)>>> Robots =
    [
        NumericMap,
        DirectionMap, DirectionMap, DirectionMap, DirectionMap, DirectionMap,
        DirectionMap, DirectionMap, DirectionMap, DirectionMap, DirectionMap,
        DirectionMap, DirectionMap, DirectionMap, DirectionMap, DirectionMap,
        DirectionMap, DirectionMap, DirectionMap, DirectionMap, DirectionMap,
        DirectionMap, DirectionMap, DirectionMap, DirectionMap, DirectionMap,
    ];

    public static long Solve(string input)
    {
        var codes = input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
            .ToList();

        var result = 0L;
        foreach (var code in codes)
        {
            var num1 = long.Parse(code.Replace("A", string.Empty));
            var moves = FindMoves(code);
            Console.WriteLine($"{code} - {moves}");
            result += num1 * moves;
        }


        return result;
    }

    private static long FindMoves(string code)
    {
        var result = 0L;
        var from = 'A';
        foreach (var to in code)
        {
            var moves = FindMove(from, to, 0);
            from = to;
            result += moves;
        }


        return result;
    }

    private static long FindMove(char from, char to, int robotIndex)
    {
        if (robotIndex == Robots.Count)
        {
            return 1;
        }

        var key = (from, to, robotIndex);
        if (LengthCache.TryGetValue(key, out var value))
        {
            return value;
        }
        

        var moves = FindMove(from, to, Robots[robotIndex]);
        var min = long.MaxValue;
        foreach (var move in moves)
        {
            var current = 0L;
            var currentFrom = 'A';
            foreach (var c in move)
            {
                current += FindMove(currentFrom, c, robotIndex + 1);
                currentFrom = c;
            }
            
            min = Math.Min(min, current);
        }

        LengthCache[key] = min;

        return min;
    }

    private static List<string> FindMove(char from, char to, Dictionary<char, List<(char newChar, char move)>> map)
    {
        var key = (from, to);
        if (MoveCache.TryGetValue(key, out var move))
        {
            return move;
        }

        var found = new List<string>();
        var min = int.MaxValue;
        var search = new PriorityQueue<(char current, string path), int>();
        search.Enqueue((from, string.Empty), 0);

        do
        {
            var (current, path) = search.Dequeue();
            if (path.Length > min)
            {
                continue;
            }

            if (current == to)
            {
                if (path.Length < min)
                {
                    min = path.Length;
                    found = [path];
                }
                else if (path.Length == min)
                {
                    found.Add(path);
                }

                continue;
            }

            foreach (var (newChar, m) in map[current])
            {
                var newPath = path + m;
                search.Enqueue((newChar, newPath), newPath.Length);
            }
        } while (search.Count > 0);

        found = found.Select(path => path + 'A').ToList();

        MoveCache[key] = found;

        return found;
    }
}