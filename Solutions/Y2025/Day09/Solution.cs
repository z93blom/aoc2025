using AdventOfCode.Framework;
using AdventOfCode.Utilities;
using AngleSharp.Css.Dom;
using SuperLinq;
using System.Collections.Generic;

namespace AdventOfCode.Solutions.Y2025.Day09;

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
        var id = 0L;
        var tiles = input.Lines()
            .Select(l => l.Longs().ToArray())
            .Select(a => new Tile(id++, a[0], a[1]))
            .ToArray();

        var pairs = tiles.Cartesian(tiles)
            .Where(t => t.Item1.Id < t.Item2.Id)
            .Select(t => new Pair(t.Item1, t.Item2))
            .OrderByDescending(p => p.Area)
            .ToArray();

        return pairs[0].Area;
    }

    private readonly record struct Tile(long Id, long X, long Y);

    private readonly record struct Pair(Tile First, Tile Second)
    {
        public long Area => Math.Abs((First.X - Second.X + 1) * (First.Y - Second.Y + 1));
    }

    static object PartTwo(string input, Func<TextWriter> getOutputFunction)
    {
        var id = 0L;
        var redTiles = input.Lines()
            .Select(l => l.Longs().ToArray())
            .Select(a => new Tile(id++, a[0], a[1]))
            .ToArray();

        var pairs = redTiles.Cartesian(redTiles)
            .Where(t => t.Item1.Id < t.Item2.Id)
            .Select(t => new Pair(t.Item1, t.Item2))
            .OrderByDescending(p => p.Area)
            .ToArray();

        var corners = redTiles.Select((t, i) =>
                new Corners(t,
                    redTiles[(i + redTiles.Length - 1) % redTiles.Length],
                    redTiles[(i + redTiles.Length + 1) % redTiles.Length]))
            .ToArray();

        var greenBorders = corners.SelectMany(c => c.Greens())
            .Distinct()
            .ToHashSet();
        var reds = redTiles.Select(t => new Point2(t.X, t.Y, YAxisDirection.ZeroAtTop))
            .ToHashSet();

        // Find one point on the inside.
        Point2 inside;
        var topMostRed = reds.MinBy(p => p.Y);
        if (greenBorders.Contains(topMostRed.Right) || reds.Contains(topMostRed.Right))
        {
            inside = topMostRed.Right.Below;
        }
        else
        {
            inside = topMostRed.Left.Below;
        }

        var borders = reds.Concat(greenBorders).ToHashSet();
        var insideGreens = FloodFill(inside, borders);

        var greens = insideGreens.Concat(greenBorders).ToHashSet();

        var colored = greens.Concat(reds).ToHashSet();

        var pairsWithOnlyColored = pairs.Where(p => Points(p).All(colored.Contains))
            .OrderByDescending(p => p.Area)
            .ToArray();

        return pairsWithOnlyColored[0].Area;
    }

    private static IEnumerable<Point2> Points(Pair p)
    {
        for (var x = Math.Min(p.First.X, p.Second.X); x <= Math.Max(p.First.X, p.Second.X); x++)
        {
            for (var y = Math.Min(p.First.Y, p.Second.Y); y <= Math.Max(p.First.Y, p.Second.Y); y++)
            {
                yield return new Point2(x, y, YAxisDirection.ZeroAtTop);
            }
        }
    }

    private static HashSet<Point2> FloodFill(Point2 start, HashSet<Point2> borders)
    {
        var inside = new HashSet<Point2>();
        var pointsToTry = new Stack<Point2>();
        pointsToTry.Push(start);

        while (pointsToTry.Count > 0)
        {
            var p = pointsToTry.Pop();
            inside.Add(p);

            foreach (var adj in p.AdjacentPoints.Where(adj => !borders.Contains(adj) && !inside.Contains(adj) && !pointsToTry.Contains(adj)))
            {
                pointsToTry.Push(adj);
            }
        }

        return inside;
    }

    private readonly record struct Corners(Tile Red, Tile PreviousRed, Tile NextRed)
    {
        public IEnumerable<Point2> Greens()
        {
            static IEnumerable<Point2> PointsBetween(Tile first, Tile second)
            {
                if (first.X == second.X)
                {
                    for (var y = Math.Min(first.Y, second.Y) + 1; y < Math.Max(first.Y, second.Y); y++)
                    {
                        yield return new Point2(first.X, y, YAxisDirection.ZeroAtTop);
                    }
                }
                else
                {
                    for (var x = Math.Min(first.X, second.X) + 1; x < Math.Max(first.X, second.X); x++)
                    {
                        yield return new Point2(x, first.Y, YAxisDirection.ZeroAtTop);
                    }
                }
            }

            foreach (var p in PointsBetween(Red, PreviousRed)) 
                yield return p;
            foreach (var p in PointsBetween(Red, NextRed))
                yield return p;
        }
    }
}