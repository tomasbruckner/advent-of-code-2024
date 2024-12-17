using System.Text.RegularExpressions;

namespace AdventOfCode.Day13;

public static partial class Part2
{
    public static long Solve(string input)
    {
        var lines = input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
            .ToList();
        var games = GetGames(lines);
        var result = CalculateResult(games);

        return result;
    }

    private static List<((long x, long y), (long x, long y), (long x, long y))> GetGames(List<string> lines)
    {
        var games = new List<((long x, long y), (long x, long y), (long x, long y))>();
        for (var i = 0; i < lines.Count; i += 3)
        {
            var buttonPartsA = ButtonRegex().Match(lines[i]);
            var buttonPartsB = ButtonRegex().Match(lines[i + 1]);
            var prizeParts = PrizeRegex().Match(lines[i + 2]);
            var buttonInfoA = (long.Parse(buttonPartsA.Groups[1].Value), long.Parse(buttonPartsA.Groups[2].Value));
            var buttonInfoB = (long.Parse(buttonPartsB.Groups[1].Value), long.Parse(buttonPartsB.Groups[2].Value));
            var prizeInfo = (
                long.Parse(prizeParts.Groups[1].Value) + 10000000000000,
                long.Parse(prizeParts.Groups[2].Value) + 10000000000000
            );

            games.Add((buttonInfoA, buttonInfoB, prizeInfo));
        }

        return games;
    }

    private static long CalculateResult(List<((long x, long y), (long x, long y), (long x, long y))> games)
    {
        var result = 0L;
        foreach (var game in games)
        {
            var (pressesA, pressesB) = CalculateGameResult(game);
            result += pressesA * 3 + pressesB;
        }

        return result;
    }

    /**
        Button A: X+94, Y+34
        Button B: X+22, Y+67
        Prize: X=10000000008400, Y=10000000005400

        94 * q + 22*z = 10000000008400
        34 * q + 67*z = 10000000005400

        q = (10000000005400 - 67*z) / 34


        94 * (10000000005400 - 67*z) / 34 + 22*z = 10000000008400
        q1 * (q2 - q4*z) / q5 + q6*z = q3

        z = (94*10000000005400 -  10000000008400*34) / (67*94 - 22*34)
     */
    private static (long pressesA, long pressesB) CalculateGameResult(
        ((long x, long y), (long x, long y), (long x, long y)) game)
    {
        var ((xOffsetA, yOffsetA), (xOffsetB, yOffsetB), (x, y)) = game;

        var bPressed = (xOffsetA * y - x * yOffsetA) / (yOffsetB * xOffsetA - xOffsetB * yOffsetA);
        var aPressedX = (x - bPressed * xOffsetB) / xOffsetA;
        var aPressedY = (y - bPressed * yOffsetB) / yOffsetA;

        return aPressedX != aPressedY || aPressedX < 0 || bPressed < 0 ||
               (aPressedX * xOffsetA + bPressed * xOffsetB) != x ||
               (aPressedY * yOffsetA + bPressed * yOffsetB) != y
            ? (0, 0)
            : (aPressedX, bPressed);
    }

    [GeneratedRegex(@"Button \w: X\+(\d+), Y\+(\d+)")]
    private static partial Regex ButtonRegex();

    [GeneratedRegex(@"Prize: X=(\d+), Y=(\d+)")]
    private static partial Regex PrizeRegex();
}