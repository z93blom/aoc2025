using AdventOfCode.Framework;
using AdventOfCode.Utilities;

namespace AdventOfCode.Solutions.Y2025.Day12;

[RegisterKeyedTransient("2025-12")] partial class Solution { }
[RegisterTransient()] partial class Solution { }

partial class Solution : ISolver
{
    public int Year => 2025;
    public int Day => 12;
    public string GetName() => "Christmas Tree Farm";

    public IEnumerable<object> Solve(string input, Func<TextWriter> getOutputFunction)
    {
        // var emptyOutput = () => new NullTextWriter();
        yield return PartOne(input, getOutputFunction);
        yield return PartTwo(input, getOutputFunction);
    }

    static object PartOne(string input, Func<TextWriter> getOutputFunction)
    {
        var a = input.SplitByDoubleNewline()
            .ToArray();
        var presents = a[..^1]
            .Select(s => s
                .SplitBySingleNewline()
                .ToArray()
                
            )
            .Select(ParsePresent)
            .ToArray();

        var regions = a[^1]
            .Lines()
            .Select(s => s.Integers().ToArray())
            .Select(ia => new Region(ia[0], ia[1], ia[2..]))
            .ToArray();

        // Naive solution - ignore form, and just check if all the presents can fit in the space needed.
        var stillValidRegions = regions.Where(r => r.Width * r.Height > r.MinimumAreaNeeded(presents))
            .ToArray();
         
        return stillValidRegions.Length;
    }

    private static Present ParsePresent(string[] data)
    {
        var id = int.Parse(data[0][..^1]);
        var shape = data[1..].ToGrid(YAxisDirection.ZeroAtTop, c => c);
        return new Present(id, shape, shape.Points.Count(p => shape[p] == '#'));
    }

    private readonly record struct Present(int Id, Grid<char> Shape, int Area);

    private readonly record struct Region(int Width, int Height, int[] Quantities)
    {
        public long MinimumAreaNeeded(Present[] presents)
        {
            var area = 0;
            for (var i = 0; i < presents.Length; i++)
            {
                area += Quantities[i] * presents[i].Area;
            }

            return area;
        }
    }


    static object PartTwo(string input, Func<TextWriter> getOutputFunction)
    {
        return 0;
    }
}