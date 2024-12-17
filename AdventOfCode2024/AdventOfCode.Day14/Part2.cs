using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode.Day14;

public static partial class Part2
{
    public static long Solve(string input, (int x, int y) maxIndex)
    {
        var robots = GetRobots(input);
        var (positions, seconds) = GetPositions(robots, maxIndex);
        Print(positions, maxIndex);

        return seconds;
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

    private static (List<(long x, long y)> positions, long seconds) GetPositions(
        List<(long x, long y, long vx, long vy)> robots,
        (int x, int y) maxIndex
    )
    {
        var (maxX, maxY) = maxIndex;
        var seconds = 1L;

        do
        {
            var positions = new HashSet<(long x, long y)>();
            foreach (var (x, y, vx, vy) in robots)
            {
                var newX = (x + seconds * vx) % maxX;
                var newY = (y + seconds * vy) % maxY;
                positions.Add((
                    newX >= 0 ? newX : newX + maxX,
                    newY >= 0 ? newY : newY + maxY
                ));
            }

            if (positions.Count == robots.Count)
            {
                return (positions.ToList(), seconds);
            }

            seconds += 1;
        } while (true);
    }

    private static void Print(List<(long x, long y)> positions, (int x, int y) maxIndex)
    {
        var map = positions.ToDictionary(x => x, _ => true);
        var stringBuilder = new StringBuilder();
        for (var y = 0; y < maxIndex.y; y++)
        {
            for (var x = 0; x < maxIndex.x; x++)
            {
                var character = map.ContainsKey((x, y)) ? 'X' : '.';
                stringBuilder.Append(character);
            }

            stringBuilder.AppendLine();
        }

        Console.WriteLine(stringBuilder.ToString());
    }


    [GeneratedRegex(@"^p=(\d+),(\d+) v=(-?\d+),(-?\d+)")]
    private static partial Regex RobotsInfo();
}