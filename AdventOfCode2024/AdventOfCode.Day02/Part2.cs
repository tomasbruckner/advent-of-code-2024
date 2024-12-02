namespace AdventOfCode.Day02;

public static class Part2
{
    public static long Solve(string input)
    {
        var lines = input.Split(Environment.NewLine);
        var result = lines.LongCount(x =>
        {
            var nums = x.Split(" ")
                .Select(long.Parse)
                .ToList();
            return IsValid(nums);
        });

        return result;
    }

    private static bool IsValid(List<long> nums, bool allowErrors = true)
    {
        long? previous = null;
        Direction? direction = null;

        foreach (var current in nums)
        {
            if (!previous.HasValue)
            {
                previous = current;
                continue;
            }

            direction ??= previous > current ? Direction.Decreasing : Direction.Increasing;

            var diff = direction == Direction.Decreasing ? previous.Value - current : current - previous.Value;
            if (diff is < 1 or > 3)
            {
                return CheckPartialValid(nums, allowErrors);
            }

            previous = current;
        }

        return true;
    }

    private static bool CheckPartialValid(List<long> nums, bool allowErrors)
    {
        if (!allowErrors)
        {
            return false;
        }

        for (var i = 0; i < nums.Count; i++)
        {
            var x = nums.Where((_, i1) => i1 != i).ToList();
            if (IsValid(x, false))
            {
                return true;
            }
        }

        return false;
    }

    private enum Direction
    {
        Increasing,
        Decreasing,
    }
}