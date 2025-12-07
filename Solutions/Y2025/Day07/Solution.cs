using AdventOfCode.Framework;
using AdventOfCode.Utilities;

namespace AdventOfCode.Solutions.Y2025.Day07;

[RegisterKeyedTransient("2025-07")] partial class Solution { }
[RegisterTransient()] partial class Solution { }

partial class Solution : ISolver
{
    public int Year => 2025;
    public int Day => 7;
    public string GetName() => "Laboratories";

    public IEnumerable<object> Solve(string input, Func<TextWriter> getOutputFunction)
    {
        // var emptyOutput = () => new NullTextWriter();
        yield return PartOne(input, getOutputFunction);
        yield return PartTwo(input, getOutputFunction);
    }

    static object PartOne(string input, Func<TextWriter> getOutputFunction)
    {
        var grid = input.ToGrid(YAxisDirection.ZeroAtTop, c => c);
        var start = grid.XSlice(0).First(p => grid[p] == 'S');
        var beams = new HashSet<Point2> { start.Below };
        var y = start.Below.Below.Y;
        var numberOfSplits = 0;
        while (y < grid.Height)
        {
            var currentSet = beams.ToArray();
            beams.Clear();
            foreach (var beam in currentSet)
            {
                if (grid[beam.Below] == '.')
                {
                    beams.Add(beam.Below);
                }
                else if (grid[beam.Below] == '^')
                {
                    numberOfSplits++;
                    beams.Add(beam.Below.Left);
                    beams.Add(beam.Below.Right);
                }
            }

            y += 1;
        }
        return numberOfSplits;
    }

    static object PartTwo(string input, Func<TextWriter> getOutputFunction)
    {
        var grid = input.ToGrid(YAxisDirection.ZeroAtTop, c => c);

        var timeLines = GetPossibleTimeLines(grid);
        return timeLines;
    }

    public readonly record struct Gp(Grid<char> grid, Point2 p);

    static long GetPossibleTimeLines(Grid<char> grid)
    {
        Func<Gp, long> func = null;
        func = Memoizer.Memoize<Gp, long>(gp => GetPossibleTimeLines(gp, func!));
        var start = grid.XSlice(0).First(p => grid[p] == 'S');
        var timelines = func(new Gp(grid, start));
        return timelines;
    }

    static long GetPossibleTimeLines(Gp gp, Func<Gp, long> f)
    {
        var beam = gp.p;
        var grid = gp.grid;

        if (!grid.Contains(beam.Below))
        {
            return 1;
        }

        if (grid[beam.Below] == '.')
        {
            return f(gp with { p = beam.Below });
        }

        if (grid[beam.Below] == '^')
        {
            var left = f(gp with { p = beam.Below.Left });
            var right = f(gp with { p = beam.Below.Right });
            return left + right;
        }

        throw new ArgumentException("AHHH");
    }
}