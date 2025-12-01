using Microsoft.Extensions.DependencyInjection;

namespace AdventOfCode.Commands;

interface ICommand
{
    public Task Run(string[] args, IServiceProvider services);
}