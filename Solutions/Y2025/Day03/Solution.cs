using AdventOfCode.Framework;
using AdventOfCode.Utilities;

namespace AdventOfCode.Solutions.Y2025.Day03;

[RegisterKeyedTransient("2025-03")] partial class Solution { }
[RegisterTransient()] partial class Solution { }

partial class Solution : ISolver
{
    public int Year => 2025;
    public int Day => 3;
    public string GetName() => "Lobby";

    public IEnumerable<object> Solve(string input, Func<TextWriter> getOutputFunction)
    {
        // var emptyOutput = () => new NullTextWriter();
        yield return PartOne(input, getOutputFunction);
        yield return PartTwo(input, getOutputFunction);
    }

    static object PartOne(string input, Func<TextWriter> getOutputFunction)
    {
        var lines = input.Lines()
            .Select(l => l.ToArray());
        var result = lines.Select(line => FindMax2(line, 2))
            .ToArray();

        return result.Sum();
    }

    public static int FindMax(string l)
    {
        var max = int.Parse(l[0].ToString() + l[1]);
        for (var i = 0; i < l.Length - 1; i++)
        {
            for (var j = i + 1; j < l.Length; j++)
            {
                var v = int.Parse(l[i].ToString() + l[j]);
                if (v > max)
                {
                    max = v;
                }
            }
        }

        return max;
    }


    static object PartTwo(string input, Func<TextWriter> getOutputFunction)
    {
        var lines = input.Lines()
            .Select(l => l.ToArray());
        var result = lines.Select(line => FindMax2(line, 12))
            .ToArray();

        return result.Sum();
    }

    public record struct V(char[] Line, int Start, int Count);

    public static long FindMax2(char[] line, int count)
    {
        Func<V, long>? maxFinder = null;
        maxFinder = Memoizer.Memoize<V, long>(v => FindMax2(v, maxFinder!));
        var result = maxFinder(new V(line, 0, count));
        return result;
    }

    public static long FindMax2(V input, Func<V, long> func)
    {
        if (input.Start >= input.Line.Length)
        {
            return 0;
        }

        if (input.Line.Length == input.Start + input.Count)
        {
            // Take the remaining items.
            return long.Parse(input.Line[input.Start..]);
        }

        var current = (long)((input.Line[input.Start] - '0') * Math.Pow(10,input.Count - 1));
        var maxLater = func(input with { Start = input.Start + 1 });

        var maxExceptThis = func(input with { Start = input.Start + 1, Count = input.Count - 1 });
        var maxThis = current + maxExceptThis;
//        var maxThis = long.Parse(current + func(input with { Start = input.Start + 1, Count = input.Count - 1 }).ToString());

        return Math.Max(maxThis, maxLater);
    }

}