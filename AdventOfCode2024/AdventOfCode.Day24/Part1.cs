using System.Text.RegularExpressions;

namespace AdventOfCode.Day24;

public static partial class Part1
{
    private enum OperationEnum
    {
        Or,
        And,
        Xor,
    }

    private static readonly Dictionary<string, OperationEnum> OpMap = new()
    {
        ["OR"] = OperationEnum.Or,
        ["AND"] = OperationEnum.And,
        ["XOR"] = OperationEnum.Xor,
    };

    public static long Solve(string input)
    {
        var (init, instructions) = GetInit(input);
        Execute(init, instructions);
        var result = CalculateResult(init);

        return result;
    }

    private static (Dictionary<string, int>, List<(string from, OperationEnum operation, string to, string dest)>)
        GetInit(string input)
    {
        var lines = input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
        var valueMap = new Dictionary<string, int>();
        var instructions = new List<(string from, OperationEnum operation, string to, string dest)>();
        foreach (var line in lines)
        {
            if (line.Contains(':'))
            {
                var initParts = line.Split(':');
                valueMap[initParts[0]] = int.Parse(initParts[1].Trim());
                continue;
            }

            var parts = InstructionsRegex().Match(line);
            var op = OpMap[parts.Groups[2].Value];
            instructions.Add((parts.Groups[1].Value, op, parts.Groups[3].Value, parts.Groups[4].Value));
        }

        return (valueMap, instructions);
    }

    private static void Execute(Dictionary<string, int> valueMap,
        List<(string from, OperationEnum operation, string to, string dest)> instructions)
    {
        var repeat = new List<(string from, OperationEnum operation, string to, string dest)>();
        foreach (var (from, op, to, dest) in instructions)
        {
            if (!valueMap.ContainsKey(from) || !valueMap.ContainsKey(to))
            {
                repeat.Add((from, op, to, dest));
                continue;
            }
            
            valueMap[dest] = op switch
            {
                OperationEnum.Or => valueMap[from] | valueMap[to],
                OperationEnum.And => valueMap[from] & valueMap[to],
                OperationEnum.Xor => valueMap[from] ^ valueMap[to],
                _ => valueMap[dest],
            };
        }

        if (repeat.Count != 0)
        {
            Execute(valueMap, repeat);
        }
    }

    private static long CalculateResult(Dictionary<string, int> valueMap)
    {
        var result = 0L;
        var pairs = valueMap.Where(x => x.Key.StartsWith('z'))
            .Select(x => (x.Key, x.Value))
            .OrderByDescending(x => x.Key);

        foreach (var (_, value) in pairs)
        {
            result <<= 1;
            result += value;
        }

        return result;
    }


    [GeneratedRegex(@"^([\w\d]+) +(\w+) +([\w\d]+) +-> +([\w\d]+)")]
    private static partial Regex InstructionsRegex();
}