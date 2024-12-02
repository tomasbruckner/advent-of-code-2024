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

    private static bool IsValid(List<long> nums)
    {
        long? previous = null;
        Direction? direction = null;
        var allowErrors = true;

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
                if (!allowErrors)
                {
                    return false;
                }

                allowErrors = false;
            }

            previous = current;
        }

        return true;
    }

    private enum Direction
    {
        Increasing,
        Decreasing,
    }
}