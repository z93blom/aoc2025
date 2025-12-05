using AdventOfCode.Framework;
using AdventOfCode.Utilities;

namespace AdventOfCode.Solutions.Y2025.Day05;

[RegisterKeyedTransient("2025-05")] partial class Solution { }
[RegisterTransient()] partial class Solution { }

partial class Solution : ISolver
{
    public int Year => 2025;
    public int Day => 5;
    public string GetName() => "Cafeteria";

    public IEnumerable<object> Solve(string input, Func<TextWriter> getOutputFunction)
    {
        // var emptyOutput = () => new NullTextWriter();
        yield return PartOne(input, getOutputFunction);
        yield return PartTwo(input, getOutputFunction);
    }

    static object PartOne(string input, Func<TextWriter> getOutputFunction)
    {
        var parts =input.SplitByDoubleNewline()
            .ToArray();

        var freshRanges = parts[0].Lines()
            .Select(l => l.Longs().ToArray())
            .Select(a => new Range(a[0], -a[1]))
            .OrderBy(r => r.Start)
            .ToArray();

        var ingredients = parts[1]
            .Lines()
            .Select(long.Parse)
            .OrderBy(i => i)
            .ToArray();

        var count = 0;
        foreach (var i in ingredients)
        {
            foreach (var freshRange in freshRanges)
            {
                if (freshRange.Contains(i))
                {
                    count++;
                    break;
                }
            }
        }

        return count;
    }

    static object PartTwo(string input, Func<TextWriter> getOutputFunction)
    {
        var parts = input.SplitByDoubleNewline()
            .ToArray();

        var freshRanges = parts[0].Lines()
            .Select(l => l.Longs().ToArray())
            .Select(a => new Range(a[0], -a[1]))
            .OrderBy(r => r.Start)
            .ThenBy(r => r.End)
            .ToList();

        var i = 1;
        while (i < freshRanges.Count)
        {
            var c = freshRanges[i];
            var pIndex = i - 1;
            var p = freshRanges[pIndex];
            if (p.End >= c.End)
            {
                // This range is completely enclosed in the previous one.
                freshRanges.Remove(c);
                continue;
            }

            if (p.End >= c.Start)
            {
                // The previous range includes part of the start of this range
                freshRanges[i] = c with { Start = p.End + 1 };
            }
            
            i++;
        }

        return freshRanges.Sum(r => r.Count);
    }

    private readonly record struct Range(long Start, long End)
    {
        public bool Contains(long v)
        {
            return v >= Start && v <= End;
        }

        public long Count
        {
            get
            {
                if (End < Start)
                {
                    return 0;
                }
                return End - Start + 1;
            }
        }
    }
}