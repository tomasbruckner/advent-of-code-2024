﻿namespace AdventOfCode.Day17;

public static class Part2
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

    public static long Solve(string input)
    {
        var program = GetProgram(input);
        var newA = Solve(program, 0, program.instructions);

        return newA;
    }

    private static long Solve(
        ((long a, long b, long c) registers, List<long> instructions) program, 
        long baseNum,
        List<long> expected)
    {
        if (expected.Count == 0)
        {
            return baseNum;
        }

        baseNum *= 8;

        for (var i = 0; i < 8; i++)
        {
            try
            {
                program.registers.a = baseNum + i;
                if (Execute(program, expected.Last()))
                {
                    var newExpected = expected.SkipLast(1).ToList();
                    var valid = Solve(program, baseNum + i, newExpected);
                    if (valid != -1)
                    {
                        return valid;
                    }
                }
            }
            catch
            {
                // ignored
            }
        }

        return -1;
    }

    private static bool Execute(
        ((long a, long b, long c) registers, List<long> instructions) program,
        long expected
    )
    {
        var (a, b, c) = program.registers;
        var instructions = program.instructions;

        for (var i = 0; i < instructions.Count; i++)
        {
            switch ((InstructionEnum)instructions[i])
            {
                case InstructionEnum.Adv:
                {
                    var val = GetValue((a, b, c), instructions[i + 1]);
                    a /= Convert.ToInt64(Math.Pow(2, val));
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
                        i = (int)instructions[i + 1] - 1;
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
                    var val = GetValue((a, b, c), instructions[i + 1]) % 8;
                    return val == expected;
                }
                case InstructionEnum.Bdv:
                {
                    var val = GetValue((a, b, c), instructions[i + 1]);
                    b = a / Convert.ToInt64(Math.Pow(2, val));
                    i++;
                    break;
                }
                case InstructionEnum.Cdv:
                {
                    var val = GetValue((a, b, c), instructions[i + 1]);
                    c = a / Convert.ToInt64(Math.Pow(2, val));
                    i++;
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        return false;
    }

    private static long GetValue((long a, long b, long c) registers, long value)
    {
        return value switch
        {
            4 => registers.a,
            5 => registers.b,
            6 => registers.c,
            _ => value,
        };
    }

    private static ((long a, long b, long c) registers, List<long> instructions) GetProgram(string input)
    {
        var lines = input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
        var a = long.Parse(lines[0].Replace("Register A: ", ""));
        var b = long.Parse(lines[1].Replace("Register B: ", ""));
        var c = long.Parse(lines[2].Replace("Register C: ", ""));
        var program = lines[3].Replace("Program: ", "").Split(",").Select(long.Parse).ToList();

        return ((a, b, c), program);
    }
}