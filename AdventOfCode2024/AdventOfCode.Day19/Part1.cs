using System.Text.RegularExpressions;

namespace AdventOfCode.Day19;

public static class Part1
{
    public static long Solve(string input)
    {
        var (uniqueChars, otherTowels, puzzles) = GetPuzzle(input);

        var result = 0L;
        foreach (var puzzle in puzzles)
        {
            if (IsValid(puzzle, otherTowels, uniqueChars))
            {
                result += 1;
            }
        }

        return result;
    }

    private static bool IsValid(
        string originalPuzzle,
        List<string> towels,
        List<char> uniqueChars)
    {
        var uniqueString = string.Join("", uniqueChars);
        var search = new Stack<string>();
        search.Push(originalPuzzle);
        var reducedTowels = towels.Where(originalPuzzle.Contains).ToList();
        var regexes = reducedTowels.Select(x => new Regex($"^[^{uniqueString}]*{x}")).ToList();
        var visited = new Dictionary<string, bool>();

        do
        {
            var puzzle = search.Pop();
            if (!uniqueChars.Any(x => puzzle.Contains(x)))
            {
                return true;
            }

            foreach (var regex in regexes)
            {
                if (!regex.IsMatch(puzzle))
                {
                    continue;
                }

                var newText = regex.Replace(puzzle, "", 1);
                if (!visited.TryAdd(newText, true))
                {
                    continue;
                }

                search.Push(newText);
            }
        } while (search.Count > 0);

        return false;
    }

    private static (List<char> uniqueChars, List<string> otherTowels, List<string> puzzles)
        GetPuzzle(string input)
    {
        var lines = input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
        var puzzles = lines.Skip(1).ToList();
        var towels = lines[0].Split(",").Select(x => x.Trim()).OrderBy(x => x.Length).ToList();
        var singleLetterTowels = towels.Where(x => x.Length == 1).ToList();
        var otherTowels = towels.Where(x =>
        {
            foreach (var o in singleLetterTowels)
            {
                x = x.Replace(o, "");
            }

            return x.Length > 0;
        }).ToList();
        var uniqueChars = string.Join("", otherTowels)
            .ToCharArray()
            .Distinct()
            .Where(x => !singleLetterTowels.Any(q => q.Contains(x)))
            .ToList();

        otherTowels = ReduceTowels(otherTowels, uniqueChars);

        return (uniqueChars, otherTowels, puzzles);
    }

    private static List<string> ReduceTowels(List<string> otherTowels, List<char> uniqueChars)
    {
        var reduced = otherTowels.Where(x => x.Length == 2).ToList();
        var maxLength = otherTowels.Max(x => x.Length);

        for (var i = 3; i <= maxLength; i++)
        {
            var candidates = otherTowels.Where(x => x.Length == i).ToList();
            var newReduced = new List<string>();
            foreach (var current in candidates)
            {
                if (!IsValid(current, reduced, uniqueChars))
                {
                    newReduced.Add(current);
                }
            }

            reduced.AddRange(newReduced);
        }

        return reduced;
    }
}