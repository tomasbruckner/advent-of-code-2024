namespace AdventOfCode.Day09;

public static class Part1
{
    private const int Dot = -1;

    public static long Solve(string input)
    {
        var expanded = GetExpanded(input);
        UpdateToFormatted(expanded);
        var result = CalculateResult(expanded);

        return result;
    }

    private static List<(int id, int count)> GetExpanded(string input)
    {
        var expanded = new List<(int id, int count)>();
        var id = 0;

        for (var i = 0; i < input.Length; i++)
        {
            var current = int.Parse(input[i].ToString());
            if (i % 2 == 0)
            {
                expanded.Add((id, current));
                id += 1;
            }
            else
            {
                expanded.Add((Dot, current));
            }
        }

        return expanded;
    }

    private static void UpdateToFormatted(List<(int id, int count)> expanded)
    {
        var startIndex = 0;
        var endIndex = expanded.Count - 1;

        while (true)
        {
            while (expanded[startIndex].id != Dot)
            {
                startIndex += 1;
            }

            while (expanded[endIndex].id == Dot)
            {
                endIndex -= 1;
            }

            if (startIndex > endIndex)
            {
                break;
            }

            var dotSpace = expanded[startIndex];
            var numSpace = expanded[endIndex];
            if (dotSpace.count == numSpace.count)
            {
                expanded[startIndex] = (numSpace.id, numSpace.count);
                endIndex -= 1;
            }
            else if (dotSpace.count > numSpace.count)
            {
                expanded[startIndex] = (numSpace.id, numSpace.count);
                expanded.Insert(startIndex + 1, (Dot, dotSpace.count - numSpace.count));
            }
            else
            {
                expanded[startIndex] = (numSpace.id, dotSpace.count);
                expanded[endIndex] = (numSpace.id, numSpace.count - dotSpace.count);
            }
        }
    }

    private static long CalculateResult(List<(int id, int count)> formatted)
    {
        var result = 0L;
        var multiplier = 0L;

        for (var i = 0;; i++)
        {
            var (id, count) = formatted[i];
            if (id == Dot)
            {
                break;
            }

            for (var j = 0; j < count; j++)
            {
                result += multiplier * id;
                multiplier += 1;
            }
        }

        return result;
    }
}