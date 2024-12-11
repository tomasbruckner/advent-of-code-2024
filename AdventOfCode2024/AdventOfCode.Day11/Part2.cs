namespace AdventOfCode.Day11;

public static class Part2
{
    private const int Iteration = 75;
    private static readonly Dictionary<(long num, int iteration), long> Cache = new();

    public static long Solve(string input)
    {
        var nums = input.Split(" ", StringSplitOptions.RemoveEmptyEntries)
            .Select(long.Parse)
            .ToList();
        var result = ExpandedCount(nums, Iteration);

        return result;
    }

    private static long ExpandedCount(List<long> nums, int iteration)
    {
        if (iteration == 0)
        {
            return nums.Count;
        }

        var result = 0L;
        foreach (var num in nums)
        {
            if (Cache.ContainsKey((num, iteration)))
            {
                result += Cache[(num, iteration)];
                continue;
            }

            var processed = Process(num);
            var currentResult = ExpandedCount(processed, iteration - 1);
            result += currentResult;
            Cache[(num, iteration)] = currentResult;
        }


        return result;
    }

    private static List<long> Process(long num)
    {
        if (num == 0)
        {
            return [1L];
        }

        if (num.ToString().Length % 2 == 0)
        {
            var (first, second) = SplitNumber(num);
            return [first, second];
        }

        return [num * 2024];
    }

    private static (long, long) SplitNumber(long number)
    {
        var str = number.ToString();
        var middleIndex = str.Length / 2;
        var firstStr = str[..middleIndex];
        var secondStr = str[middleIndex..];

        var first = long.Parse(firstStr);
        var second = long.Parse(secondStr);

        return (first, second);
    }
}