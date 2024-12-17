﻿using static System.Int64;

namespace AdventOfCode.Day16;

public static class Part2
{
    private enum DirectionEnum
    {
        Top,
        Right,
        Bottom,
        Left,
    }

    private static readonly
        Dictionary<DirectionEnum, List<(int offsetX, int offsetY, int score, DirectionEnum direction)>> ValidMoves =
            new()
            {
                {
                    DirectionEnum.Bottom,
                    [
                        (0, 1, 1, DirectionEnum.Bottom),
                        (-1, 0, 1001, DirectionEnum.Left),
                        (1, 0, 1001, DirectionEnum.Right),
                    ]
                },
                {
                    DirectionEnum.Top,
                    [
                        (0, -1, 1, DirectionEnum.Top),
                        (-1, 0, 1001, DirectionEnum.Left),
                        (1, 0, 1001, DirectionEnum.Right),
                    ]
                },
                {
                    DirectionEnum.Left,
                    [
                        (-1, 0, 1, DirectionEnum.Left),
                        (0, -1, 1001, DirectionEnum.Top),
                        (0, 1, 1001, DirectionEnum.Bottom),
                    ]
                },
                {
                    DirectionEnum.Right,
                    [
                        (1, 0, 1, DirectionEnum.Right),
                        (0, -1, 1001, DirectionEnum.Top),
                        (0, 1, 1001, DirectionEnum.Bottom),
                    ]
                },
            };

    public static long Solve(string input)
    {
        var matrix = input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
            .Select(x => x.ToCharArray().ToList())
            .ToList();
        var paths = FindShortestPath(matrix);
        var result = paths.SelectMany(x => x).ToHashSet().Count;

        return result;
    }

    private static (int startX, int startY) FindStartAndEnd(List<List<char>> matrix)
    {
        for (var y = 0; y < matrix.Count; y++)
        {
            for (var x = 0; x < matrix[y].Count; x++)
            {
                if (matrix[y][x] == 'S')
                {
                    return (x, y);
                }
            }
        }

        throw new Exception("Start not found");
    }


    private static List<List<(int x, int y)>> FindShortestPath(
        List<List<char>> matrix)
    {
        var minFinishedScore = MaxValue;
        var shortestPaths = new List<List<(int x, int y)>>();
        var (startX, startY) = FindStartAndEnd(matrix);

        var visited = new Dictionary<((int x, int y), DirectionEnum direction), int>();
        var search =
            new PriorityQueue<(List<(int x, int y)>, DirectionEnum direction, int score), int>();
        var start = (new List<(int x, int y)> { (startX, startY) }, DirectionEnum.Right, 0);
        search.Enqueue(start, 0);

        do
        {
            var (path, direction, score) = search.Dequeue();
            var (x, y) = path.Last();
            if (matrix[y][x] == 'E')
            {
                if (score == minFinishedScore)
                {
                    shortestPaths.Add(path);
                }
                else if (score < minFinishedScore)
                {
                    shortestPaths = [path];
                    minFinishedScore = score;
                }

                continue;
            }

            if (score > minFinishedScore)
            {
                continue;
            }

            var key = ((x, y), direction);
            if (visited.TryGetValue(key, out var value) && value < score)
            {
                continue;
            }

            visited[key] = score;

            foreach (var (offsetX, offsetY, offsetScore, newDirection) in ValidMoves[direction])
            {
                var newX = x + offsetX;
                var newY = y + offsetY;
                if (matrix[newY][newX] == '#')
                {
                    continue;
                }

                search.Enqueue(
                    ([..path, (newX, newY)], newDirection, score + offsetScore), score + offsetScore
                );
            }
        } while (search.Count > 0);

        return shortestPaths;
    }
}