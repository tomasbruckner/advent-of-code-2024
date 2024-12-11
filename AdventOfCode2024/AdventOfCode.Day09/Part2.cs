namespace AdventOfCode.Day09;

public static class Part2
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
        for (var numIndex = expanded.Count - 1; numIndex >= 0; numIndex--)
        {
            if (expanded[numIndex].id == Dot)
            {
                continue;
            }
            
            var dotIndex = expanded.FindIndex(x => x.id == Dot && x.count >= expanded[numIndex].count);
            if (dotIndex == -1 || dotIndex > numIndex)
            {
                continue;
            }
            
            var dotSpace = expanded[dotIndex];
            var numSpace = expanded[numIndex];

            expanded[numIndex] = (Dot, numSpace.count);
            if (numSpace.count == dotSpace.count)
            {
                expanded[dotIndex] = (numSpace.id, dotSpace.count);
            }
            else
            {
                expanded[dotIndex] = (numSpace.id, expanded[numIndex].count);
                var newDotCount = dotSpace.count - numSpace.count;
                if (expanded[dotIndex + 1].id == Dot)
                {
                    expanded[dotIndex + 1] = (Dot, expanded[dotIndex + 1].count + newDotCount);
                }
                else
                {
                    expanded.Insert(dotIndex + 1, (Dot, newDotCount));
                }
            }

        }
    }

    private static long CalculateResult(List<(int id, int count)> formatted)
    {
        var result = 0L;
        var multiplier = 0L;

        foreach (var (id, count) in formatted)
        {
            if (id == Dot)
            {
                multiplier += count;
                continue;
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