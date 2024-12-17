using AdventOfCode.Day14;

var part1ResultTest = Part1.Solve(File.ReadAllText("input-test.txt"), (11, 7));
Console.WriteLine(part1ResultTest);

var part1ResultFull = Part1.Solve(File.ReadAllText("input-full.txt"), (101, 103));
Console.WriteLine(part1ResultFull);

var part2ResultFull = Part2.Solve(File.ReadAllText("input-full.txt"), (101, 103));
Console.WriteLine(part2ResultFull);