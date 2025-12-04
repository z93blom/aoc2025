using AdventOfCode.Framework;
using AdventOfCode.Utilities;

namespace AdventOfCode.Solutions.Y2025.Day04;

[RegisterKeyedTransient("2025-04")] partial class Solution { }
[RegisterTransient()] partial class Solution { }

partial class Solution : ISolver
{
    public int Year => 2025;
    public int Day => 4;
    public string GetName() => "Printing Department";

    public IEnumerable<object> Solve(string input, Func<TextWriter> getOutputFunction)
    {
        // var emptyOutput = () => new NullTextWriter();
        yield return PartOne(input, getOutputFunction);
        yield return PartTwo(input, getOutputFunction);
    }

    static object PartOne(string input, Func<TextWriter> getOutputFunction)
    {
        var grid = input.ToGrid(YAxisDirection.ZeroAtTop, c => c);

        var rolls = 0;
        foreach (var p in grid.Points)
        {
            if (grid[p] == '@')
            {
                var diags = p.AdjacentPoints.Where(a => grid.Contains(a) && grid[a] == '@')
                    .ToArray();

                if (diags.Length < 4)
                {
                    rolls += 1;
                }
            }
        }
        return rolls;
    }

    static object PartTwo(string input, Func<TextWriter> getOutputFunction)
    {
        var grid = input.ToGrid(YAxisDirection.ZeroAtTop, c => c);

        var removed = 0;
        var remove = new HashSet<Point2>();
        do
        {
            remove.Clear();
            foreach (var p in grid.Points)
            {
                if (grid[p] == '@')
                {
                    var diags = p.AdjacentPoints.Where(a => grid.Contains(a) && grid[a] == '@')
                        .ToArray();

                    if (diags.Length < 4)
                    {
                        remove.Add(p);
                    }
                }
            }

            foreach (var p in remove)
            {
                grid[p] = '.';
            }

            removed += remove.Count;
        } while (remove.Count > 0);

        return removed;
    }
}