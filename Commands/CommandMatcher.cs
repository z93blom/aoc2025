using System.Text.RegularExpressions;

namespace AdventOfCode.Commands;

record struct CommandMatcher(ArgumentExpressions ArgumentExpressions, Func<Func<string[], IServiceProvider, Task>> Function)
{
    public Func<string[], IServiceProvider, Task>? GetCommand(string[] args)
    {
        if (args.Length == 0 && ArgumentExpressions.Expressions.Length == 0)
        {
            return Function();
        }

        if (args.Length != ArgumentExpressions.Expressions.Length)
        {
            return null;
        }

        var matches = args.Zip(ArgumentExpressions.Expressions, (arg, regex) => new Regex("^" + regex + "$").Match(arg)).ToArray();
        if (!matches.All(match => match.Success))
        {
            return null;
        }

        return Function();
    }
}