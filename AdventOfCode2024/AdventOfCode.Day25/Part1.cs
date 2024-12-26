namespace AdventOfCode.Day25;

public static class Part1
{
    public static long Solve(string input)
    {
        var (holes, keys) = GetHolesAndKeys(input);
        var result = CalculateResult(holes, keys);

        return result;
    }

    private static long CalculateResult(List<int[]> holes, List<int[]> keys)
    {
        var result = 0L;
        foreach (var hole in holes)
        {
            foreach (var key in keys)
            {
                if (KeyMatch(hole, key))
                {
                    result += 1;
                }
            }
        }

        return result;
    }

    private static bool KeyMatch(int[] hole, int[] key)
    {
        for (var i = 0; i < hole.Length; i++)
        {
            if (hole[i] >= key[i])
            {
                return false;
            }
        }

        return true;
    }

    private static (List<int[]> holes, List<int[]> keys) GetHolesAndKeys(string input)
    {
        var holes = new List<int[]>();
        var keys = new List<int[]>();
        var matrixes = GetMatrixes(input);

        foreach (var matrix in matrixes)
        {
            var isHole = matrix[0][0] == '#';
            if (isHole)
            {
                AddHole(holes, matrix);
            }
            else
            {
                AddKey(keys, matrix);
            }
        }

        return (holes, keys);
    }

    private static void AddHole(List<int[]> holes, List<List<char>> matrix)
    {
        var hole = new int[5];
        for (var x = 0; x < matrix[0].Count; x++)
        {
            for (var y = 0; y < matrix.Count; y++)
            {
                if (matrix[y][x] != '#')
                {
                    hole[x] = y - 1;
                    break;
                }
            }
        }

        holes.Add(hole);
    }

    private static void AddKey(List<int[]> keys, List<List<char>> matrix)
    {
        var key = new int[5];
        for (var x = 0; x < matrix[0].Count; x++)
        {
            for (var y = matrix.Count - 1; y >= 0; y--)
            {
                if (matrix[y][x] != '#')
                {
                    key[x] = y + 1;
                    break;
                }
            }
        }

        keys.Add(key);
    }

    private static List<List<List<char>>> GetMatrixes(string input)
    {
        var matrixes = new List<List<List<char>>>();
        var current = new List<List<char>>();
        foreach (var line in input.Split(Environment.NewLine))
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                matrixes.Add(current);
                current = new List<List<char>>();
                continue;
            }

            current.Add(line.ToCharArray().ToList());
        }

        matrixes.Add(current);

        return matrixes;
    }
}