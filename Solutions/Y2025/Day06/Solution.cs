using AdventOfCode.Framework;
using AdventOfCode.Utilities;
using AngleSharp.Text;

namespace AdventOfCode.Solutions.Y2025.Day06;

[RegisterKeyedTransient("2025-06")] partial class Solution { }
[RegisterTransient()] partial class Solution { }

partial class Solution : ISolver
{
    public int Year => 2025;
    public int Day => 6;
    public string GetName() => "Trash Compactor";

    public IEnumerable<object> Solve(string input, Func<TextWriter> getOutputFunction)
    {
        // var emptyOutput = () => new NullTextWriter();
        yield return PartOne(input, getOutputFunction);
        yield return PartTwo(input, getOutputFunction);
    }

    static object PartOne(string input, Func<TextWriter> getOutputFunction)
    {
        var lines = input.Lines()
            .Select(l => l.SplitSpaces().ToArray())
            .ToArray();

        var ops = lines[^1];

        var values = lines[..^1]
            .Select(v => v.Select(long.Parse).ToArray())
            .ToArray();

        var sum = 0L;
        for (var opIndex = 0; opIndex < ops.Length; opIndex++)
        {
            var op = ops[opIndex];
            Func<long, long, long> fun = op == "*" 
                ? (l, r) => l * r 
                : (l, r) => l + r;
            var val = values[0][opIndex];
            for (var j = 1; j < values.Length; j++)
            {
                var r = values[j][opIndex];
                val = fun(val, r);
            }

            sum += val;
        }
        return sum;
    }

    static object PartTwo(string input, Func<TextWriter> getOutputFunction)
    {
        var grid = input.ToGrid(YAxisDirection.ZeroAtTop, c => c);
        var opRow = grid.XSlice(grid.Height - 1).ToArray();

        var sum = 0L;
        var val = 0L;
        Func<long, long, long> fun = (l, r) => l;
        for (var col = 0; col < grid.Width; col++)
        {
            var op = grid[opRow[col]];
            if (op != ' ')
            {
                fun = op == '*'
                    ? (l, r) => l * r
                    : (l, r) => l + r;
                sum += val;
                val = GetVal(grid, col)!.Value;
            }
            else
            {
                var r = GetVal(grid, col);
                if (r != null)
                {
                    val = fun(val, r.Value);
                }
            }
        }

        sum += val;
        return sum;
    }

    private static long? GetVal(Grid<char> grid, int col)
    {
        var chars = grid.YSlice(col)
            .Select(p => grid[p])
            .Where(c => c.IsDigit())
            .ToArray();

        if (chars.Length == 0)
        {
            return null;
        }

        var val = long.Parse(chars);
        return val;
    }
}