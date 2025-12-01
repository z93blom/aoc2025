using AdventOfCode.Framework;
using AdventOfCode.Utilities;
using QuikGraph.Algorithms.ShortestPath;

namespace AdventOfCode.Solutions.Y2025.Day01;

[RegisterKeyedTransient("2025-01")] partial class Solution { }
[RegisterTransient()] partial class Solution { }

partial class Solution : ISolver
{
    public int Year => 2025;
    public int Day => 1;
    public string GetName() => "Secret Entrance";

    public IEnumerable<object> Solve(string input, Func<TextWriter> getOutputFunction)
    {
        // var emptyOutput = () => new NullTextWriter();
        yield return PartOne(input, getOutputFunction);
        yield return PartTwo(input, getOutputFunction);
    }

    static object PartOne(string input, Func<TextWriter> getOutputFunction)
    {
        var dial = 50;
        var count = 0;
        foreach (var l in input.Lines())
        {
            var clicks = int.Parse(l[1..]);
            var multiplier = l[0] == 'L' ? -1 : 1;
            dial += clicks * multiplier;
            dial %= 100;
            if (dial == 0)
            {
                count++;
            }

        }
        return count;
    }

    static object PartTwo(string input, Func<TextWriter> getOutputFunction)
    {
        var dial = 50;
        var count = 0;
        foreach (var l in input.Lines())
        {
            var clicks = int.Parse(l[1..]);
            while (clicks > 0)
            {
                if (l[0] == 'L')
                {
                    dial -= 1;
                }
                else
                {
                    dial += 1;
                }

                if (dial == -1)
                {
                    dial = 99;
                }
                if (dial == 100)
                {
                    dial = 0;
                }

                if (dial == 0)
                {
                    count++;
                }

                clicks--;
            }
        }

        return count;
    }
}