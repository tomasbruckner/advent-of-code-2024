namespace AdventOfCode.Day02;

public static class Part1
{
    public static long Solve(string input)
    {
        var lines = input.Split(Environment.NewLine);

        var result = lines.LongCount(IsValid);

        return result;
    }

    private static bool IsValid(string line)
    {
        long? previous = null;
        Direction? direction = null;

        foreach (var num in line.Split(' '))
        {
            var current = long.Parse(num);
            if (!previous.HasValue)
            {
                previous = current;
                continue;
            }

            direction ??= previous > current ? Direction.Decreasing : Direction.Increasing;

            var diff = direction == Direction.Decreasing ? previous.Value - current : current - previous.Value;
            if (diff is < 1 or > 3)
            {
                return false;
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
