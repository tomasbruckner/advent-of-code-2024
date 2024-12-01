using System.Text.RegularExpressions;

namespace AdventOfCode.Day01;

public static partial class Part1
{
    public static long Solve(string input)
    {
        var result = 0L;
        var lines = input.Split(Environment.NewLine);
        var arr1 = new List<long>();
        var arr2 = new List<long>();

        foreach (var line in lines)
        {
            var parts = MyRegex().Match(line);
            if (!parts.Success)
            {
                continue;
            }
            
            arr1.Add(long.Parse(parts.Groups[1].Value));
            arr2.Add(long.Parse(parts.Groups[2].Value));
        }
        
        arr1 = arr1.OrderBy(x => x).ToList();
        arr2 = arr2.OrderBy(x => x).ToList();

        for (var i = 0; i < arr1.Count; i++)
        {
            result += Math.Abs(arr1[i] - arr2[i]);
        }

        return result;
    }

    [GeneratedRegex(@"^(\d+) +(\d+)")]
    private static partial Regex MyRegex();
}
