using System.Text.RegularExpressions;

namespace AdventOfCode.Day05;

public static partial class Part1
{
    public static long Solve(string input)
    {
        var ruleList = new List<(long, long)>();
        var orders = new List<List<long>>();
        var lines = input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
        foreach (var line in lines)
        {
            var ruleParts = RuleRegex().Match(line);
            if (ruleParts.Success)
            {
                ruleList.Add((long.Parse(ruleParts.Groups[1].Value), long.Parse(ruleParts.Groups[2].Value)));
                continue;
            }

            var order = line.Split(',').Select(long.Parse).ToList();
            orders.Add(order);
        }

        var ruleLookup = ruleList.ToLookup(rule => rule.Item1, rule => rule.Item2);
        var result = GetResult(ruleLookup, orders);

        return result;
    }

    private static long GetResult(ILookup<long, long> rules, List<List<long>> orders)
    {
        var result = 0L;
        foreach (var order in orders)
        {
            if (IsValid(rules, order))
            {
                var middleIndex = (int)Math.Floor(order.Count / 2.0);
                result += order[middleIndex];
            }
        }

        return result;
    }

    private static bool IsValid(ILookup<long, long> rules, List<long> order)
    {
        var visited = new HashSet<long>();
        foreach (var orderNumber in order)
        {
            foreach (var r in rules[orderNumber])
            {
                if (visited.Contains(r))
                {
                    return false;
                }
            }

            visited.Add(orderNumber);
        }

        return true;
    }

    [GeneratedRegex(@"^(\d+)\|(\d+)")]
    private static partial Regex RuleRegex();
}