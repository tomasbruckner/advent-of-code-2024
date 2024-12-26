using AdventOfCode.Day20;

var part1ResultTest = Part1.Solve(File.ReadAllText("input-test.txt"), 30);
Console.WriteLine(part1ResultTest);

var part1ResultFull = Part1.Solve(File.ReadAllText("input-full.txt"), 100);
Console.WriteLine(part1ResultFull);

var part2ResultTest2 = Part2.Solve(File.ReadAllText("input-test.txt"), 76);
Console.WriteLine(part2ResultTest2);

var part2ResultFull = Part2.Solve(File.ReadAllText("input-full.txt"), 100);
Console.WriteLine(part2ResultFull);