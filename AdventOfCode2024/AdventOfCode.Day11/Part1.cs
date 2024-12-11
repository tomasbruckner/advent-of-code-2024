namespace AdventOfCode.Day11;

public static class Part1
{
    private const int Iteration = 25;
    
    public static long Solve(string input)
    {
        var nums = input.Split(" ", StringSplitOptions.RemoveEmptyEntries)
            .Select(long.Parse)
            .ToList();
        ExpandNums(nums, Iteration);
        
        return nums.Count;
    }

    private static void ExpandNums(List<long> nums, int iteration)
    {
        for (var i = 0; i < iteration; i++)
        {
            ExpandNums(nums);    
        }
    }

    private static void ExpandNums(List<long> nums)
    {
        for (var i = 0; i < nums.Count; i++)
        {
            if (nums[i] == 0)
            {
                nums[i] = 1L;
            } else if (nums[i].ToString().Length % 2 == 0)
            {
                var (first, second) = SplitNumber(nums[i]);
                nums[i] = first;
                nums.Insert(i+1, second);
                i += 1;
            }
            else
            {
                nums[i] *= 2024;
            }
        }
    }
    
    private static (long, long) SplitNumber(long number)
    {
        var str = number.ToString();
        var middleIndex = str.Length / 2;
        var firstStr = str[..middleIndex];
        var secondStr = str[middleIndex..];

        var first = long.Parse(firstStr);
        var second = long.Parse(secondStr);

        return (first, second);
    }
}