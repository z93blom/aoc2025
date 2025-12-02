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
            .ToArray();

        var invalidIds = ranges
            .Select(r => FindInvalidIds(r[0], r[1]).ToArray())
            .SelectMany(a => a)
            .ToArray();

        var sum = invalidIds.Sum();
        return sum;
    }

    private static IEnumerable<long> FindInvalidIds(string start, string end)
    {
        var low = long.Parse(start);
        var high = long.Parse(end);

        for (var i = low; i <= high; i++)
        {
            var s = i.ToString();
            if (s.Length % 2 == 0)
            {
                var left = s.Substring(0, s.Length / 2);
                var right = s.Substring(s.Length / 2, s.Length / 2);
                if (left == right)
                {
                    yield return i;
                }
            }
        }
    }

    private static IEnumerable<long> FindInvalidIds2(string start, string end)
    {
        var low = long.Parse(start);
        var high = long.Parse(end);

        for (var i = low; i <= high; i++)
        {
            var s = i.ToString();
            for (var idx = 1; idx < s.Length / 2 + 1; idx++)
            {
                if (s.Length % idx == 0)
                {
                    var pattern = s.Substring(0, idx);
                    var isMatch = true;
                    for (var ps = pattern.Length; isMatch && ps < s.Length; ps += idx)
                    {
                        if (pattern != s.Substring(ps, pattern.Length))
                        {
                            isMatch = false;
                        }
                    }

                    if (isMatch)
                    {
                        yield return i;
                        break;
                    }
                }

            }
        }
    }

    static object PartTwo(string input, Func<TextWriter> getOutputFunction)
    {
        var ranges = input.Split(",")
            .Select(s => s.Trim())
            .Select(r => r.Split("-"))
            .ToArray();

        var invalidIds = ranges
            .Select(r => FindInvalidIds2(r[0], r[1]).ToArray())
            .SelectMany(a => a)
            .ToArray();

        var sum = invalidIds.Sum();
        return sum;
    }
}