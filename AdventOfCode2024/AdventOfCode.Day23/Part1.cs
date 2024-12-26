namespace AdventOfCode.Day23;

public static class Part1
{
    public static long Solve(string input)
    {
        var parsed = input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
            .Select(x => x.Split('-').ToList())
            .ToList();
        var connections = GetConnections(parsed);
        var result = CalculateResult(connections);

        return result;
    }

    private static long CalculateResult(Dictionary<string, List<string>> map)
    {
        var triples = new List<(string, string, string)>();
        var visited = new Dictionary<string, bool>();
        foreach (var connection in map)
        {
            visited[connection.Key] = true;
            for (var i = 0; i < connection.Value.Count - 1; i++)
            {
                var item1 = connection.Value[i];
                for (var j = i + 1; j < connection.Value.Count; j++)
                {
                    var item2 = connection.Value[j];
                    if (visited.ContainsKey(item2) || visited.ContainsKey(item1))
                    {
                        continue;
                    }

                    if (map[item1].Contains(item2))
                    {
                        triples.Add((connection.Key, item1, item2));
                    }
                }
            }
        }

        return triples.Count(x => x.Item1.StartsWith('t') || x.Item2.StartsWith('t') || x.Item3.StartsWith('t'));
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