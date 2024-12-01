using System.Text.RegularExpressions;

namespace AdventOfCode.Day01;

public static partial class Part2
{
    public static long Solve(string input)
    {
        var result = 0L;
        var lines = input.Split(Environment.NewLine);
        var arr1 = new List<long>();
        var countMap = new Dictionary<long, long>();

        foreach (var line in lines)
        {
            var parts = MyRegex().Match(line);
            if (!parts.Success)
            {
                continue;
            }
            
            arr1.Add(long.Parse(parts.Groups[1].Value));
            var num2 = long.Parse(parts.Groups[2].Value);
            if (!countMap.TryAdd(num2, 1))
            {
                countMap[num2] += 1;
            }
        }
        
        arr1 = arr1.OrderBy(x => x).ToList();

        foreach (var t in arr1)
        {
            if (countMap.TryGetValue(t, out var value))
            {
                result += t * value;
            }
        }

        return result;
    }

    [GeneratedRegex(@"^(\d+) +(\d+)")]
    private static partial Regex MyRegex();
}
