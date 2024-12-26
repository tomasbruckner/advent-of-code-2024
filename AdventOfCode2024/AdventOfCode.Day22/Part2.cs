namespace AdventOfCode.Day22;

public static class Part2
{
    public static long Solve(string input)
    {
        var codes = input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
            .Select(long.Parse)
            .ToList();

        var mapList = new List<Dictionary<(int, int, int, int), int>>();
        foreach (var code in codes)
        {
            var map = GenerateMap(code);
            mapList.Add(map);
        }

        var result = CalculateResult(mapList);

        return result;
    }

    private static long CalculateResult(List<Dictionary<(int, int, int, int), int>> mapList)
    {
        var visited = new Dictionary<(int, int, int, int), int>();
        var max = int.MinValue;
        foreach (var map in mapList)
        {
            foreach (var kvp in map)
            {
                if (visited.ContainsKey(kvp.Key))
                {
                    continue;
                }
                
                var sum = mapList.Select(x => x.ContainsKey(kvp.Key) ? x[kvp.Key] : 0).Sum();
                if (sum > max)
                {
                    max = sum;
                }

                visited[kvp.Key] = sum;                
            }
        }

        return max;
    }

    private static Dictionary<(int, int, int, int), int> GenerateMap(long code)
    {
        var dictionary = new Dictionary<(int, int, int, int), int>();
        var seq = new[] { 10, 10, 10, 10 };
        var previous = code;
        for (var i = 0; i < 2000; i++)
        {
            var newSecret = ((64 * previous) ^ previous) % 16777216;
            newSecret = ((newSecret / 32) ^ newSecret) % 16777216;
            newSecret = ((2048 * newSecret) ^ newSecret) % 16777216;
            seq[0] = seq[1];
            seq[1] = seq[2];
            seq[2] = seq[3];
            var price = (int) (newSecret % 10);
            seq[3] = (int)(price - previous % 10);
            previous = newSecret;

            if (i >= 3)
            {
                dictionary.TryAdd((seq[0], seq[1], seq[2], seq[3]), price);
            }
        }

        return dictionary;
    }
}