using System.Text.RegularExpressions;

namespace AdventOfCode.Day03;

public static partial class Part2
{
    public static long Solve(string input)
    {
        var result = GetResult(input.ReplaceLineEndings(" "));

        return result;
    }

    private static long GetResult(string line)
    {
        var result = 0L;
        var enabled = true;
        var parts = MyRegex().Matches(line);
        foreach (Match part in parts)
        {
            if (part.Groups[0].Value == "do()")
            {
                enabled = true;
            }
            else if (part.Groups[0].Value == "don't()")
            {
                enabled = false;
            }
            else if (enabled)
            {
                result += long.Parse(part.Groups[2].Value) * long.Parse(part.Groups[3].Value);
            }
        }

        return result;
    }

    [GeneratedRegex(@"(mul\((\d\d?\d?),(\d\d?\d?)\))|(do\(\))|(don't\(\))")]
    private static partial Regex MyRegex();
}