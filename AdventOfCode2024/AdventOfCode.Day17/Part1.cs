namespace AdventOfCode.Day17;

public static class Part1
{
    private enum InstructionEnum
    {
        Adv = 0,
        Bxl = 1,
        Bst = 2,
        Jnz = 3,
        Bxc = 4,
        Out = 5,
        Bdv = 6,
        Cdv = 7,
    }

    public static void Solve(string input)
    {
        var program = GetProgram(input);
        Execute(program);
    }

    private static void Execute(((int a, int b, int c) registers, List<int> instructions) program)
    {
        var outputs = new List<int>();
        var (a, b, c) = program.registers;
        var instructions = program.instructions;

        for (var i = 0; i < instructions.Count; i++)
        {
            switch ((InstructionEnum)instructions[i])
            {
                case InstructionEnum.Adv:
                {
                    var val = GetValue((a, b, c), instructions[i + 1]);
                    a /= Convert.ToInt32(Math.Pow(2, val));
                    i++;
                    break;
                }
                case InstructionEnum.Bxl:
                {
                    b ^= instructions[i + 1];
                    i++;
                    break;
                }
                case InstructionEnum.Bst:
                {
                    var val = GetValue((a, b, c), instructions[i + 1]);
                    b = val % 8;
                    i++;
                    break;
                }
                case InstructionEnum.Jnz:
                {
                    if (a != 0)
                    {
                        i = instructions[i + 1] - 1;
                    }
                    else
                    {
                        i++;
                    }

                    break;
                }
                case InstructionEnum.Bxc:
                {
                    b ^= c;
                    i++;
                    break;
                }
                case InstructionEnum.Out:
                {
                    var val = GetValue((a, b, c), instructions[i + 1]);
                    outputs.Add(val % 8);
                    i++;
                    break;
                }
                case InstructionEnum.Bdv:
                {
                    var val = GetValue((a, b, c), instructions[i + 1]);
                    b = a / Convert.ToInt32(Math.Pow(2, val));
                    i++;
                    break;
                }
                case InstructionEnum.Cdv:
                {
                    var val = GetValue((a, b, c), instructions[i + 1]);
                    c = a / Convert.ToInt32(Math.Pow(2, val));
                    i++;
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        Console.WriteLine(string.Join(',', outputs));
        Console.WriteLine($"Register A: {a}");
        Console.WriteLine($"Register B: {b}");
        Console.WriteLine($"Register C: {c}");
    }

    private static int GetValue((int a, int b, int c) registers, int value)
    {
        return value switch
        {
            4 => registers.a,
            5 => registers.b,
            6 => registers.c,
            _ => value,
        };
    }

    private static ((int a, int b, int c) registers, List<int> instructions) GetProgram(string input)
    {
        var lines = input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
        var a = int.Parse(lines[0].Replace("Register A: ", ""));
        var b = int.Parse(lines[1].Replace("Register B: ", ""));
        var c = int.Parse(lines[2].Replace("Register C: ", ""));
        var program = lines[3].Replace("Program: ", "").Split(",").Select(int.Parse).ToList();

        return ((a, b, c), program);
    }
}