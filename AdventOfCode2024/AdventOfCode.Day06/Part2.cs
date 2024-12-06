﻿namespace AdventOfCode.Day06;

public static class Part2
{
    private enum DirectionEnum
    {
        Up,
        Right,
        Left,
        Down,
        Finished,
    }

    private static readonly Dictionary<DirectionEnum, (int x, int y)> Directions = new()
    {
        { DirectionEnum.Up, (0, -1) },
        { DirectionEnum.Right, (1, 0) },
        { DirectionEnum.Left, (-1, 0) },
        { DirectionEnum.Down, (0, 1) },
    };

    private static readonly Dictionary<char, DirectionEnum> DirectionMap = new()
    {
        { '^', DirectionEnum.Up },
        { '>', DirectionEnum.Right },
        { '<', DirectionEnum.Left },
        { 'v', DirectionEnum.Down },
    };

    private static readonly Dictionary<DirectionEnum, DirectionEnum> DirectionRotationMap = new()
    {
        { DirectionEnum.Left, DirectionEnum.Up },
        { DirectionEnum.Up, DirectionEnum.Right },
        { DirectionEnum.Right, DirectionEnum.Down },
        { DirectionEnum.Down, DirectionEnum.Left },
    };

    public static long Solve(string input)
    {
        var matrix = input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
            .Select(q => q.ToCharArray().ToList())
            .ToList();
        var (x, y, direction) = FindStart(matrix);
        matrix[y][x] = '.';
        
        var loopCount = 0;
        for (var i = 0; i < matrix.Count; i++)
        {
            for (var j = 0; j < matrix[i].Count; j++)
            {
                if (matrix[i][j] == '#' || y == i && x == j)
                {
                    continue;
                }
        
                matrix[i][j] = '#';
                if (IsLoop(matrix, x, y, direction))
                {
                    loopCount += 1;
                }
                matrix[i][j] = '.';
            }
        }
        
        return loopCount;
    }

    private static (int x, int y, DirectionEnum direction) FindStart(List<List<char>> matrix)
    {
        for (var y = 0; y < matrix.Count; y++)
        {
            for (var x = 0; x < matrix[y].Count; x++)
            {
                if (DirectionMap.ContainsKey(matrix[y][x]))
                {
                    return (x, y, DirectionMap[matrix[y][x]]);
                }
            }
        }

        throw new Exception("Start not found");
    }

    private static bool IsLoop(List<List<char>> matrix, int x, int y, DirectionEnum direction)
    {
        var visited = new HashSet<(int, int, DirectionEnum)> { (x, y, direction) };

        do
        {
            var (xOffset, yOffset) = Directions[direction];
            var newDirection = GetNewDirection(matrix, x + xOffset, y + yOffset, direction);
            if (newDirection == DirectionEnum.Finished)
            {
                return false;
            }

            if (newDirection != direction)
            {
                direction = newDirection;
                continue;
            }
            
            x += Directions[direction].x;
            y += Directions[direction].y;

            if (!visited.Add((x, y, direction)))
            {
                return true;
            }
        } while (true);
    }

    private static DirectionEnum GetNewDirection(List<List<char>> matrix, int x, int y, DirectionEnum direction)
    {
        if (OutOfRange(matrix, x, y))
        {
            return DirectionEnum.Finished;
        }

        return matrix[y][x] == '.' ? direction : DirectionRotationMap[direction];
    }

    private static bool OutOfRange(List<List<char>> matrix, int x, int y)
    {
        return y > matrix.Count - 1 || y < 0 || x < 0 || x > matrix[y].Count - 1;
    }
}