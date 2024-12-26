namespace AdventOfCode.Day22;

public static class Part1
{
    public static long Solve(string input)
    {
        var codes = input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
            .Select(long.Parse)
            .ToList();

        var result = 0L;
        foreach (var code in codes)
        {
            var nextSecret = GenerateNextSecret(code);
            result += nextSecret;
        }

        return result;
    }

    private static long GenerateNextSecret(long code)
    {
        var previous = code;
        for (var i = 0; i < 2000; i++)
        {
            var newSecret = ((64 * previous) ^ previous) % 16777216;
            newSecret = ((newSecret / 32) ^ newSecret) % 16777216; 
            newSecret = ((2048 * newSecret) ^ newSecret) % 16777216;
            previous = newSecret;
        }

        return previous;
    }
}