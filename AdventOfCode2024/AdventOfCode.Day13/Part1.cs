using System.Text.RegularExpressions;

namespace AdventOfCode.Day13;

public static partial class Part1
{
    public static long Solve(string input)
    {
        var lines = input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
            .ToList();
        var games = GetGames(lines);
        var result = CalculateResult(games);

        return result;
    }

    private static List<((int x, int y), (int x, int y), (int x, int y))> GetGames(List<string> lines)
    {
        var games = new List<((int x, int y), (int x, int y), (int x, int y))>();
        for (var i = 0; i < lines.Count; i += 3)
        {
            var buttonPartsA = ButtonRegex().Match(lines[i]);
            var buttonPartsB = ButtonRegex().Match(lines[i + 1]);
            var prizeParts = PrizeRegex().Match(lines[i + 2]);
            var buttonInfoA = (int.Parse(buttonPartsA.Groups[1].Value), int.Parse(buttonPartsA.Groups[2].Value));
            var buttonInfoB = (int.Parse(buttonPartsB.Groups[1].Value), int.Parse(buttonPartsB.Groups[2].Value));
            var prizeInfo = (int.Parse(prizeParts.Groups[1].Value), int.Parse(prizeParts.Groups[2].Value));

            games.Add((buttonInfoA, buttonInfoB, prizeInfo));
        }

        return games;
    }

    private static long CalculateResult(List<((int x, int y), (int x, int y), (int x, int y))> games)
    {
        var result = 0L;
        foreach (var game in games)
        {
            var (pressesA, pressesB) = CalculateGameResult(game);
            result += pressesA * 3 + pressesB;
        }

        return result;
    }

    private static (int pressesA, int pressesB) CalculateGameResult(
        ((int x, int y), (int x, int y), (int x, int y)) game)
    {
        
        var ((xOffsetA, yOffsetA), (xOffsetB, yOffsetB), (x, y)) = game;

        for (var aPressed = 0; aPressed <= 100; aPressed++)
        {
            for (var bPressed = 0; bPressed <= 100; bPressed++)
            {
                var currentX = aPressed * xOffsetA + bPressed * xOffsetB;
                var currentY = aPressed * yOffsetA + bPressed * yOffsetB;
                
                if (currentX > x || currentY > y)
                {
                    break;
                }
                
                if (currentX == x && currentY == y)
                {
                    return (aPressed, bPressed);
                }
            }
        }

        return (0, 0);
    }

    [GeneratedRegex(@"Button \w: X\+(\d+), Y\+(\d+)")]
    private static partial Regex ButtonRegex();

    [GeneratedRegex(@"Prize: X=(\d+), Y=(\d+)")]
    private static partial Regex PrizeRegex();
}