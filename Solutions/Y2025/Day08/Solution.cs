using System.Reflection.Metadata.Ecma335;
using AdventOfCode.Framework;
using AdventOfCode.Utilities;
using SuperLinq;

namespace AdventOfCode.Solutions.Y2025.Day08;

[RegisterKeyedTransient("2025-08")] partial class Solution { }
[RegisterTransient()] partial class Solution { }

partial class Solution : ISolver
{
    public int Year => 2025;
    public int Day => 8;
    public string GetName() => "Playground";

    public IEnumerable<object> Solve(string input, Func<TextWriter> getOutputFunction)
    {
        // var emptyOutput = () => new NullTextWriter();
        yield return PartOne(input, getOutputFunction);
        yield return PartTwo(input, getOutputFunction);
    }

    static object PartOne(string input, Func<TextWriter> getOutputFunction)
    {
        // Note: Added a single line at the top to get the number of iterations, since they vary by file.
        var iterations = int.Parse(input.Lines().First());
        var id = 0;
        var junctionBoxes = input.Lines()
            .Skip(1)
            .Select(l => l.Split(",").Select(long.Parse).ToArray())
            .Select(a => new Point3(a[0], a[1], a[2], id++))
            .ToArray();

        var connections = junctionBoxes.Cartesian(junctionBoxes)
            .Where(t => t.Item1.Id < t.Item2.Id)
            .Select(t => new Connection(t.Item1, t.Item2))
            .OrderBy(c => c.SquaredDistance)
            //.Take(iterations)
            .ToArray();

        var lookup = junctionBoxes.ToDictionary(
            p => p, 
            p => new HashSet<Point3> { p });

        var circuits = lookup.Select(kvp => kvp.Value).ToList();

        foreach (var connection in connections.Take(iterations))
        {
            var firstCircuit = lookup[connection.From];
            var secondCircuit = lookup[connection.To];

            if (firstCircuit == secondCircuit)
            {
                continue;
            }

            // Combine them, and update the circuits
            foreach (var p in secondCircuit)
            {
                firstCircuit.Add(p);
            }

            lookup[connection.To] = firstCircuit;
            circuits.Remove(secondCircuit);
        }

        var topCircuits = circuits
            .OrderByDescending(set => set.Count)
            .Take(3)
            .ToArray();

         var result = topCircuits.Aggregate(1L, (v, set) => v * set.Count);
         return result;
    }
    
    public readonly record struct Point3(long X, long Y, long Z, long Id);

    public readonly record struct Connection(Point3 From, Point3 To)
    {
        public long SquaredDistance => (From.X - To.X) * (From.X - To.X)
                                      + (From.Y - To.Y) * (From.Y - To.Y)
                                      + (From.Z - To.Z) * (From.Z - To.Z);
    }

    static object PartTwo(string input, Func<TextWriter> getOutputFunction)
    {
        return 0;
    }
}