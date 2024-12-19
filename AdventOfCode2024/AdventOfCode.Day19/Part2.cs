namespace AdventOfCode.Day19;

public static class Part2
{
    public static long Solve(string input)
    {
        var (towels, puzzles) = GetPuzzle(input);

        var result = 0L;
        foreach (var puzzle in puzzles)
        {
            var reducedTowels = towels.Where(puzzle.Contains).ToList();
            var visited = new Dictionary<string, long>();
            result += GetValidCombinations(puzzle, reducedTowels, visited);
        }

        return result;
    }

    private static long GetValidCombinations(
        string puzzle,
        List<string> towels,
        Dictionary<string, long> visited
        )
    {
        if (puzzle == string.Empty)
        {
            return 1;
        }

        if (visited.TryGetValue(puzzle, out var combinations))
        {
            return combinations;
        }
        
        var result = 0L;
        foreach (var towel in towels)
        {
            if (!puzzle.StartsWith(towel))
            {
                continue;
            }

            result += GetValidCombinations(puzzle[towel.Length..], towels, visited);
        }
        
        visited[puzzle] = result;

        return result;
    }


    private static (List<string> towels, List<string> puzzles) GetPuzzle(string input)
    {
        var lines = input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
        var puzzles = lines.Skip(1).ToList();
        var towels = lines[0].Split(",").Select(x => x.Trim()).OrderBy(x => x.Length).ToList();

        return (towels, puzzles);
    }
}