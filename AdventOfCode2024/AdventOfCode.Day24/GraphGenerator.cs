using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode.Day24;

public static partial class GraphGenerator
{
    public static void Generate(string inputPath, string outputPath)
    {
        var lines = File.ReadAllText(inputPath).Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
        var opCountMap = new Dictionary<string, int>()
        {
            { "XOR", 0 },
            { "OR", 0 },
            { "AND", 0 },
        };
        var stringBuilder = new StringBuilder();
        stringBuilder.Append("graph G {\n    fontname = \"Helvetica,Arial,sans-serif\";\n    node [fontname = \"Helvetica,Arial,sans-serif\";];\n    edge [fontname = \"Helvetica,Arial,sans-serif\";];");
        foreach (var line in lines)
        {
            if (line.Contains(':'))
            {
                continue;
            }

            
            var parts = InstructionsRegex().Match(line);
            var op = parts.Groups[2].Value + opCountMap[parts.Groups[2].Value];
            opCountMap[parts.Groups[2].Value] += 1;
            
            stringBuilder.Append($"    {parts.Groups[1].Value} -- {op};\n");
            stringBuilder.Append($"    {parts.Groups[3].Value} -- {op};\n");
            stringBuilder.Append($"    {op} -- {parts.Groups[4].Value};\n");
        }
        
        stringBuilder.Append("}\n");
        
        File.WriteAllText(outputPath, stringBuilder.ToString());
    }
    
    [GeneratedRegex(@"^([\w\d]+) +(\w+) +([\w\d]+) +-> +([\w\d]+)")]
    private static partial Regex InstructionsRegex();
}