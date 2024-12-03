using System.Text.RegularExpressions;

namespace AdventOfCode.Day03;

public static partial class Part1
{
    public static long Solve(string input)
    {
        var result = GetResult(input.ReplaceLineEndings(" "));

        return result;
    }

    private static long GetResult(string line)
    {
        var result = 0L;
        var parts = MyRegex().Matches(line);
        foreach (Match part in parts)
        {
            result += long.Parse(part.Groups[1].Value) * long.Parse(part.Groups[2].Value);
        }

        return result;
    }
    
    [GeneratedRegex(@"mul\((\d\d?\d?),(\d\d?\d?)\)")]
    private static partial Regex MyRegex();
}
