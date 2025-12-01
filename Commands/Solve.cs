using AdventOfCode.Framework;
using Microsoft.Extensions.DependencyInjection;

namespace AdventOfCode.Commands;

static class Solve
{
    public static Task SolveSpecificDate(string[] args, IServiceProvider services)
    {
        var year = int.Parse(args[1][..4]);
        var day = int.Parse(args[1][5..]);

        return SolveSpecificDate(year, day, services);
    }

    public static Task SolveSpecificYear(string[] args, IServiceProvider services)
    {
        var year = int.Parse(args[1]);

        var solvers = services.GetServices<ISolver>()
            .Where(s => s.Year == year);
        Runner.RunAll(solvers.ToArray());
        return Task.CompletedTask;
    }

    public static Task SolveAll(string[] args, IServiceProvider services)
    {
        var solvers = services.GetServices<ISolver>();
        Runner.RunAll(solvers.ToArray());
        return Task.CompletedTask;
    }

    public static Task SolveToday(string[] args, IServiceProvider services)
    {
        var now = DateTime.Now;
        if (now is { Month: 12, Day: >= 1 and <= 25 })
        {
            return SolveSpecificDate(now.Year, now.Day, services);
        }
        else
        {
            Console.WriteLine("Event is not active. This option only works from 1st Dec to 25th Dec.");
            return Task.CompletedTask;
        }
    }

    public static Task SolveSpecificDate(int year, int day, IServiceProvider services)
    {
        var solver = services.GetKeyedService<ISolver>($"{year}-{day:D2}");
        if (solver != null)
        {
            Runner.RunAll(solver);
        }
        else
        {
            Console.WriteLine($"Unable to find a problem solver for {year}-{day}.");
        }

        return Task.CompletedTask;
    }
}