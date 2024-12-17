using System.Text.RegularExpressions;

namespace AdventOfCode.Day14;

public static partial class Part1
{
    private const int Seconds = 100;

    public static long Solve(string input, (int x, int y) maxIndex)
    {
        var robots = GetRobots(input);
        var positions = GetPositions(robots, maxIndex);
        var result = CalculateResult(positions, maxIndex);

        return result;
    }

    private static List<(long x, long y, long vx, long vy)> GetRobots(string input)
    {
        var robots = new List<(long x, long y, long vx, long vy)>();
        var lines = input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
            .ToList();

        foreach (var line in lines)
        {
            var parts = RobotsInfo().Match(line);
            robots.Add((
                long.Parse(parts.Groups[1].Value),
                long.Parse(parts.Groups[2].Value),
                long.Parse(parts.Groups[3].Value),
                long.Parse(parts.Groups[4].Value)
            ));
        }

        return robots;
    }

    private static List<(long x, long y)> GetPositions(List<(long x, long y, long vx, long vy)> robots,
        (int x, int y) maxIndex)
    {
        var positions = new List<(long x, long y)>();
        var (maxX, maxY) = maxIndex;

        foreach (var (x, y, vx, vy) in robots)
        {
            var newX = (x + Seconds * vx) % maxX;
            var newY = (y + Seconds * vy) % maxY;
            positions.Add((
                newX >= 0 ? newX : newX + maxX,
                newY >= 0 ? newY : newY + maxY
            ));
        }

        return positions;
    }

    private static long CalculateResult(List<(long x, long y)> positions, (int x, int y) maxIndex)
    {
        var q1 = 0;
        var q2 = 0;
        var q3 = 0;
        var q4 = 0;
        var middleX = maxIndex.x / 2;
        var middleY = maxIndex.y / 2;

        foreach (var (x, y) in positions)
        {
            if (x == middleX || y == middleY)
            {
                continue;
            }

            if (x < middleX && y < middleY)
            {
                q1++;
            }
            else if (x < middleX && y > middleY)
            {
                q2++;
            }
            else if (x > middleX && y < middleY)
            {
                q3++;
            }
            else if (x > middleX && y > middleY)
            {
                q4++;
            }
        }


        return q1 * q2 * q3 * q4;
    }


    [GeneratedRegex(@"^p=(\d+),(\d+) v=(-?\d+),(-?\d+)")]
    private static partial Regex RobotsInfo();
}