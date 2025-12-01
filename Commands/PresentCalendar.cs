using AdventOfCode.Framework;
using Microsoft.Extensions.DependencyInjection;

namespace AdventOfCode.Commands;

static class PresentCalendar
{
    public static Task Run(string[] args, IServiceProvider services)
    {
        var year = DateTime.Now.Year;
        if (args.Length > 1)
        {
            year = int.Parse(args[1]);
        }

        var splashScreen = services.GetKeyedService<ISplashScreen>(year.ToString());
        if (splashScreen == null)
        {
            Console.WriteLine($"No calendar for {year}.");
        }
        else
        {
            splashScreen.Show();
        }

        return Task.CompletedTask;
    }
}