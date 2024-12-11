using System.Text.RegularExpressions;

namespace AdventOfCode.Day07;

public static partial class Part1
{
    public static long Solve(string input)
    {
        var lines = input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
        var result = 0L;
        foreach (var line in lines)
        {
            var parts = EquationRegex().Match(line);
            if (!parts.Success)
            {
                continue;
            }

            var sum = long.Parse(parts.Groups[1].Value);
            var nums = parts.Groups[2].Value.Split(" ").Select(long.Parse).ToList();
            if (IsValid(sum, nums, 0))
            {
                result += sum;
            }
        }


        return result;
    }

    private static bool IsValid(long finalSum, List<long> nums, long sum)
    {
        var clone = new List<long>(nums);
        if (nums.Count == 0)
        {
            return finalSum == sum;
        }

        var currentNum = clone[0];
        clone.RemoveAt(0);

        return IsValid(finalSum, clone, sum + currentNum) ||
               IsValid(finalSum, clone, sum == 0 ? currentNum : sum * currentNum);
    }


    [GeneratedRegex(@"^(\d+): +(.+)")]
    private static partial Regex EquationRegex();
}