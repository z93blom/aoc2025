using AdventOfCode.Framework;

namespace AdventOfCode.Commands;

static class Update
{
    public static async Task UpdateSpecificDate(string[] args, IServiceProvider services)
    {
        var year = int.Parse(args[1][..4]);
        var day = int.Parse(args[1][5..]);

        await Updater.Update(year, day);
    }

    public static async Task UpdateToday(string[] args, IServiceProvider services)
    {
        var now = DateTime.Now;
        if (now is { Month: 12, Day: >= 1 and <= 25 })
        {
            await Updater.Update(now.Year, now.Day);
        }
        else
        {
            Console.WriteLine("Event is not active. This option only works in Dec (1-25)");
        }
    }

    public static async Task UpdateCalendar(string[] args, IServiceProvider services)
    {
        var now = DateTime.Now;
        await Updater.UpdateCalendar(now.Year);
    }
}