using System.Text.RegularExpressions;

namespace AdventOfCode.Day24;

public static partial class Part2
{
    private record struct Instruction(string Op1, OperationEnum Op, string Op2, string Dest);

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

    public static string Solve(string input)
    {
        var instructions = GetInstructions(input);
        var faulty = FindFaulty(instructions);
        var str = faulty.Select(x => x.Dest).Order();

        return string.Join(',', str);
    }

    private static List<Instruction> GetInstructions(string input)
    {
        var lines = input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
        var instructions = new List<Instruction>();
        foreach (var line in lines)
        {
            if (line.Contains(':'))
            {
                continue;
            }

            var parts = InstructionsRegex().Match(line);
            var op = OpMap[parts.Groups[2].Value];
            instructions.Add(new Instruction(parts.Groups[1].Value, op, parts.Groups[3].Value, parts.Groups[4].Value));
        }

        return instructions;
    }

    private static List<Instruction> FindFaulty(List<Instruction> instructions)
    {
        var faulty = new List<Instruction>();
        var highestOutput = instructions.Where((x) => x.Dest.StartsWith('z'))
            .Select(x => x.Dest)
            .OrderByDescending(x => x)
            .First();

        foreach (var instruction in instructions)
        {
            if (!IsValid(instruction, instructions, highestOutput))
            {
                faulty.Add(instruction);
            }
        }

        return faulty;
    }

    private static bool IsValid(
        Instruction instruction,
        List<Instruction> instructions,
        string highestOutput
    )
    {
        var (_, operation, _, dest) = instruction;

        if (!dest.StartsWith('z'))
        {
            return IsValidIntermediate(instruction, instructions);
        }

        if (dest == highestOutput)
        {
            return operation == OperationEnum.Or;
        }

        return operation == OperationEnum.Xor;
    }

    private static bool IsValidIntermediate(Instruction instruction, List<Instruction> instructions)
    {
        var (op1, operation, op2, dest) = instruction;

        if (op1.StartsWith('x') || op1.StartsWith('y') || op2.StartsWith('x') || op2.StartsWith('y'))
        {
            var isXor = operation == OperationEnum.Xor && instructions
                .Where(x => x.Op1 == dest || x.Op2 == dest)
                .Count(x => x.Op == OperationEnum.Xor) == 1;
            var isAnd = operation == OperationEnum.And && instructions
                .Where(x => x.Op1 == dest || x.Op2 == dest)
                .Count(x => x.Op == OperationEnum.Or) == 1;
            var isFirstAnd = operation == OperationEnum.And && (op1 is "x00" or "y00") && instructions
                .Where(x => x.Op1 == dest || x.Op2 == dest)
                .Count(x => x.Op == OperationEnum.And) == 1;
            
            return isXor  || isAnd || isFirstAnd;
        }

        return operation is OperationEnum.And or OperationEnum.Or;
    }


    [GeneratedRegex(@"^([\w\d]+) +(\w+) +([\w\d]+) +-> +([\w\d]+)")]
    private static partial Regex InstructionsRegex();
}