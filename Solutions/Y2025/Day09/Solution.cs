using AdventOfCode.Framework;
using AdventOfCode.Utilities;
using SuperLinq;

namespace AdventOfCode.Solutions.Y2025.Day09
{
    [RegisterKeyedTransient("2025-09")] partial class Solution { }
    [RegisterTransient()] partial class Solution { }

    partial class Solution : ISolver
    {
        public int Year => 2025;
        public int Day => 9;
        public string GetName() => "Movie Theater";

        public IEnumerable<object> Solve(string input, Func<TextWriter> getOutputFunction)
        {
            // var emptyOutput = () => new NullTextWriter();
            yield return PartOne(input, getOutputFunction);
            yield return PartTwo(input, getOutputFunction);
        }

        static object PartOne(string input, Func<TextWriter> getOutputFunction)
        {
            var tiles = input.Lines()
                .Select(l => l.Longs().ToArray())
                .Select(a => new Tile(a[0], a[1]))
                .ToArray();

            var pairs = tiles.Combinations()
                .Select(t => new Pair(t.Item1, t.Item2))
                .OrderByDescending(p => p.Area)
                .ToArray();

            return pairs[0].Area;
        }

        private readonly record struct Tile(long X, long Y);

        private readonly record struct Pair(Tile First, Tile Second)
        {
            public long Area => (Math.Abs(First.X - Second.X) + 1) * (Math.Abs(First.Y - Second.Y) + 1);

            public bool CrossesEdge(Tile start, Tile end)
            {
                // If the line, given by start <-> end, crosses any of the edges of area
                // the area cannot be completely inside the polygon.
                var result = Math.Min(First.X, Second.X) >= Math.Max(start.X, end.X)
                             || Math.Max(First.X, Second.X) <= Math.Min(start.X, end.X)
                             || Math.Min(First.Y, Second.Y) >= Math.Max(start.Y, end.Y)
                             || Math.Max(First.Y, Second.Y) <= Math.Min(start.Y, end.Y);
                return result;
            }
        }

        static object PartTwo(string input, Func<TextWriter> getOutputFunction)
        {
            var redTiles = input.Lines()
                .Select(l => l.Longs().ToArray())
                .Select(a => new Tile(a[0], a[1]))
                .ToArray();

            var lines = redTiles
                .CircularWindows()
                .ToArray();

            var result = redTiles.Combinations()
                .Select(p => new Pair(p.First, p.Second))
                .Where(pair => lines.All(line => pair.CrossesEdge(line.First, line.Second)))
                .OrderByDescending(p => p.Area)
                .ToArray();

            return result[0].Area;
        }
    }
}
