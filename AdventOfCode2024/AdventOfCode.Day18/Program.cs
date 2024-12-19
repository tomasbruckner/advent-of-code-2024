using AdventOfCode.Day18;

var part1ResultTest = Part1.Solve(File.ReadAllText("input-test.txt"), 7, 12);
Console.WriteLine(part1ResultTest);

var part1ResultFull = Part1.Solve(File.ReadAllText("input-full.txt"), 71, 1024);
Console.WriteLine(part1ResultFull);

Part2.Solve(File.ReadAllText("input-test.txt"), 7);
Part2.Solve(File.ReadAllText("input-full.txt"), 71);
