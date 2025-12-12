using System.Collections.Immutable;
using AdventOfCode.Framework;
using AdventOfCode.Utilities;
using QuikGraph;
using QuikGraph.Algorithms.RankedShortestPath;

namespace AdventOfCode.Solutions.Y2025.Day11;

[RegisterKeyedTransient("2025-11")] partial class Solution { }
[RegisterTransient()] partial class Solution { }

partial class Solution : ISolver
{
    public int Year => 2025;
    public int Day => 11;
    public string GetName() => "Reactor";

    public IEnumerable<object> Solve(string input, Func<TextWriter> getOutputFunction)
    {
        // var emptyOutput = () => new NullTextWriter();
        yield return PartOne(input, getOutputFunction);
        yield return PartTwo(input, getOutputFunction);
    }

    static object PartOne(string input, Func<TextWriter> getOutputFunction)
    {
        var devices = input.Lines()
            .Select(l => l.Split(" "))
            .Select(a => new Device(a[0][..^1], a[1..]))
            .ToDictionary(d => d.Id, d => d);

        if (!devices.ContainsKey("you"))
        {
            return 0;
        }

        var start = devices["you"];

        var paths = start
            .Outputs
            .Select(l => new Path(["you", l]))
            .ToList();

        var pathsToEnd = new List<Path>();
        var pathsToTry = new Stack<Path>(paths);

        while (pathsToTry.Count > 0)
        {
            var p = pathsToTry.Pop();
            if (p.ReachedEnd)
            {
                pathsToEnd.Add(p);
                continue;
            }

            foreach (var l in devices[p.Current].Outputs)
            {
                // Check for circular routes.
                if (!p.Steps.Contains(l))
                {
                    var next = new Path(p.Steps.Append(l).ToImmutableList());
                    pathsToTry.Push(next);
                }
            }
        }

        return pathsToEnd.Count;
    }

    public readonly record struct Device(string Id, string[] Outputs);

    public readonly record struct Path(IReadOnlyList<string> Steps)
    {
        public bool ReachedEnd => Current == "out";
        public string Current => Steps[^1];
    }

    public readonly record struct D2(string Id);

    public readonly record struct P2(D2 Source, D2 Target) : IEdge<D2>;


    static object PartTwo(string input, Func<TextWriter> getOutputFunction)
    {
        var devices = input.Lines()
            .Select(l => l.Split(" "))
            .Select(a => new Device(a[0][..^1], a[1..]))
            .ToDictionary(d => d.Id, d => d);

        devices.Add("out", new Device("out", []));

        if (!devices.ContainsKey("dac")
            || !devices.ContainsKey("fft"))
        {
            return 0;
        }

        var graph = new BidirectionalGraph<D2, P2>();
        var d2 = devices.Values.Select(d => new D2(d.Id))
            .ToDictionary(d2 => d2.Id, d2 => d2);
        foreach (var device in d2.Values)
        {
            graph.AddVertex(device);
        }

        foreach (var device in devices.Values)
        {
            foreach (var o in device.Outputs)
            {
                graph.AddEdge(new P2(d2[device.Id], d2[o]));
            }
        }

        var a = new HoffmanPavleyRankedShortestPathAlgorithm<D2, P2>(graph, _ => 0)
        {
            ShortestPathCount = int.MaxValue / 4
        };

        a.Compute(d2["fft"], d2["dac"]);
        long fftDac = a.ComputedShortestPathCount;

        a.Compute(d2["dac"], d2["fft"]);
        long dacFft = a.ComputedShortestPathCount;

        if (fftDac != 0)
        {
            a.Compute(d2["svr"], d2["fft"]);
            long svrFft = a.ComputedShortestPathCount;
            a.Compute(d2["dac"], d2["out"]);
            long dacOut = a.ComputedShortestPathCount;
            return svrFft * fftDac * dacOut;
        }
        else if (dacFft != 0)
        {
            a.Compute(d2["svr"], d2["dac"]);
            long svrDac = a.ComputedShortestPathCount;
            a.Compute(d2["fft"], d2["out"]);
            long fftOut = a.ComputedShortestPathCount;
            return svrDac * dacFft * fftOut;
        }

        return 0;
    }
}