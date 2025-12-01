namespace AdventOfCode.Framework;

public static class ApplicationUsage
{
    public static string Usage() => """
                             Usage: dotnet run [arguments]
                             Supported arguments:
                             
                              solve [year]-[day]    Solve the specified problems
                              solve [year]          Solve the whole year
                              solve today           Solve today's problem (only available during event).

                             To start working on new problems:
                             
                              update [year]-[day]   Prepares a folder for the given day, updates the input, 
                                                    the readme and creates a solution template.
                              update today          Same as above, but for the current day. (only available during event). 
                              
                             Useful commands during december:
                               dotnet run update today
                               dotnet run solve today 

                             """;
}
