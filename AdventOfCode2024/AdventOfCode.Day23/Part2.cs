namespace AdventOfCode.Day23;

public static class Part2
{
    public static string Solve(string input)
    {
        var parsed = input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
            .Select(x => x.Split('-').ToList())
            .ToList();
        var connections = GetConnections(parsed);
        var result = CalculateResult(connections);

        return result;
    }

    private static string CalculateResult(Dictionary<string, List<string>> map)
    {
        var largest = new List<string>();
        foreach (var connection in map)
        {
            foreach (var x in connection.Value)
            {
                var current = Search(map, [connection.Key, x]);
                if (current.Count > largest.Count)
                {
                    largest = current;
                }
            }
        }
        
        largest.Sort();

        return string.Join(",", largest);
    }

    private static List<string> Search(Dictionary<string, List<string>> map, List<string> connected)
    {
        var first = connected[0];
        var candidates = map[first].Where(x => !connected.Contains(x)).ToList();
        foreach (var candidate in candidates)
        {
            var allConnected = connected.All(x => map[x].Contains(candidate));
            if (allConnected)
            {
                var newConnected = new List<string>(connected) { candidate };
                return Search(map, newConnected);
            }
        }

        return connected;
    }
    
    private static Dictionary<string, List<string>> GetConnections(List<List<string>> parsed)
    {
        var map = new Dictionary<string, List<string>>();

        foreach (var connection in parsed)
        {
            var from = connection[0];
            var to = connection[1];

            if (map.ContainsKey(from))
            {
                map[from].Add(to);
            }
            else
            {
                map.Add(from, [to]);
            }

            if (map.ContainsKey(to))
            {
                map[to].Add(from);
            }
            else
            {
                map.Add(to, [from]);
            }
        }

        return map;
    }
}