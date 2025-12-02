using AdventOfCode.Framework;
using AdventOfCode.Utilities;

namespace AdventOfCode.Solutions.Y2025.Day02;

[RegisterKeyedTransient("2025-02")] partial class Solution { }
[RegisterTransient()] partial class Solution { }

partial class Solution : ISolver
{
    public int Year => 2025;
    public int Day => 2;
    public string GetName() => "Gift Shop";

    public IEnumerable<object> Solve(string input, Func<TextWriter> getOutputFunction)
    {
        // var emptyOutput = () => new NullTextWriter();
        yield return PartOne(input, getOutputFunction);
        yield return PartTwo(input, getOutputFunction);
    }

    static object PartOne(string input, Func<TextWriter> getOutputFunction)
    {
        var ranges = input.Split(",")
            .Select(s => s.Trim())
            .Select(r => r.Split("-"))
            //.Select(e => e.Select(long.Parse).ToArray())
            .ToArray();

        var validRangesToCheck = ranges.Select(ToCheckableRange)
            .Where(IsValid)
            .ToArray();

        var invalidIds = validRangesToCheck
            .Select(r => FindInvalidIds(r[0], r[1]).ToArray())
            .SelectMany(a => a)
            .Select(long.Parse)
            .ToArray();
            //.Sum();

        var sum = invalidIds.Sum();
        return sum;
    }

    private static string[] ToCheckableRange(string[] range)
    {
        var start = range[0];
        if (start.Length % 2 != 0)
        {
            start = "1" + new string('0', start.Length);
        }

        var end = range[1];
        if (end.Length % 2 != 0)
        {
            end = new string('9', end.Length - 1);
        }

        return [start, end];
    }

    private static bool IsValid(string[] range)
    {
        return long.Parse(range[0]) < long.Parse(range[1]);
    }

    private static IEnumerable<string> FindInvalidIds(string s, string e)
    {
        var length = s.Length;

        // take the first half.
        var low = GetLow(s);
        var high = GetHigh(e);


        while (low <= high)
        {
            yield return low.ToString() + low.ToString();
            low++;
        }

        //return high - low;

        

        //yield return low;
        //yield return high;

        //yield break;
    }

    private static long GetLow(string s)
    {
        var left = long.Parse(s.Substring(0, s.Length / 2));
        var right = long.Parse(s.Substring(s.Length / 2, s.Length / 2));
        return Math.Max(left, right);
    }

    private static long GetHigh(string s)
    {
        var left = long.Parse(s.Substring(0, s.Length / 2));
        var right = long.Parse(s.Substring(s.Length / 2, s.Length / 2));
        return Math.Min(left, right);
    }

    static object PartTwo(string input, Func<TextWriter> getOutputFunction)
    {
        return 0;
    }
}