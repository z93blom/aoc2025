using System.Diagnostics;
using System.Net;
using System.Net.Http.Headers;
using AdventOfCode.Generator;
using AdventOfCode.Model;
using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Io;
using Spectre.Console;
using Calendar = AdventOfCode.Model.Calendar;

namespace AdventOfCode.Framework;

public static class Updater
{
    private const string SessionEnvironmentName = "AOCSESSION";
    private const string UserAgentEnvironmentName = "AOCUSERAGENT";

    public static async Task Update(int year, int day)
    {
        try
        {
            var baseAddress = new Uri("https://adventofcode.com/");
            var context = CreateContext(baseAddress);

            var calendar = await DownloadCalendar(context, baseAddress, year);
            var problem = await DownloadProblem(context, baseAddress, year, day);

            var dir = Dir(year, day);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            UpdateReadmeForYear(calendar);
            UpdateSplashScreen(calendar);
            UpdateReadmeForDay(problem);
            UpdateInput(problem);
            UpdateRefout(problem);
            UpdateSolutionTemplate(problem);
            UpdateExample(problem);
        }
        catch (HttpRequestException e)
        {
            AnsiConsole.WriteException(e);
            AnsiConsole.MarkupLine($"[darkorange]Is your[/] [maroon]'{SessionEnvironmentName}'[/] [darkorange] environment variable updated to a correct value?[/]");
        }
        catch (Exception e)
        {
            AnsiConsole.WriteException(e);
        }
    }

    private static IBrowsingContext CreateContext(Uri baseAddress)
    {
        if (!Environment.GetEnvironmentVariables().Contains(SessionEnvironmentName))
        {
            throw new Exception($"Specify '{SessionEnvironmentName}' environment variable.");
        }
        if (!Environment.GetEnvironmentVariables().Contains(UserAgentEnvironmentName))
        {
            throw new Exception($"Specify '{UserAgentEnvironmentName}' environment variable.");
        }


        var requester = new DefaultHttpRequester("github.com/encse/adventofcode by encse@csokavar.hu");

        var context = BrowsingContext.New(Configuration.Default
            .With(requester)
            .WithDefaultLoader()
            .WithCss()
            .WithDefaultCookies()
        );

        context.SetCookie(new Url(baseAddress.ToString()), "session=" + Environment.GetEnvironmentVariable(SessionEnvironmentName));

        var cookieContainer = new CookieContainer();
        using var client = new HttpClient(
            new HttpClientHandler
            {
                CookieContainer = cookieContainer,
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            });

        client.BaseAddress = baseAddress;
        client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("(" + Environment.GetEnvironmentVariable(UserAgentEnvironmentName) + ")"));
        cookieContainer.Add(baseAddress,
            new Cookie("session", Environment.GetEnvironmentVariable(SessionEnvironmentName)));
        return context;
    }

    public static async Task UpdateCalendar(int year)
    {
        try
        {
            var baseAddress = new Uri("https://adventofcode.com/");
            var context = CreateContext(baseAddress);

            var calendar = await DownloadCalendar(context,baseAddress, year);

            UpdateReadmeForYear(calendar);
            UpdateSplashScreen(calendar);
        }
        catch (HttpRequestException e)
        {
            AnsiConsole.WriteException(e);
            AnsiConsole.MarkupLine($"[darkorange]Is your[/] [maroon]'{SessionEnvironmentName}'[/] [darkorange] environment variable updated to a correct value?[/]");
        }
        catch (Exception e)
        {
            AnsiConsole.WriteException(e);
        }
    }

    static void WriteFile(string file, string content)
    {
        var dir = Path.GetDirectoryName(file);
        if (!Directory.Exists(dir))
        {
            Debug.Assert(dir != null, nameof(dir) + " != null");
            Directory.CreateDirectory(dir);
        }

        Console.WriteLine($"Writing {file}");
        File.WriteAllText(file, content);
    }

    static string Dir(int year, int day) => Path.Combine(SolverExtensions.WorkingDir(year, day));

    //static async Task<Calendar> DownloadCalendar(HttpClient client, int year)
    //{
    //    var html = await Download(client, year.ToString());
    //    return Calendar.Parse(year, html);
    //}

    static async Task<Calendar> DownloadCalendar(IBrowsingContext context, Uri baseUri, int year)
    {
        var document = await context.OpenAsync(baseUri.ToString() + year);
        if (document.StatusCode != HttpStatusCode.OK)
        {
            throw new AocCommunicationError("Could not fetch calendar", document.StatusCode, document.TextContent);
        }
        return Calendar.Parse(year, document);
    }


    //static async Task<Problem> DownloadProblem(HttpClient client, int year, int day)
    //{
    //    var problemStatement = await Download(client, $"{year}/day/{day}");
    //    var input = await Download(client, $"{year}/day/{day}/input");
    //    return Problem.Parse(year, day, client.BaseAddress + $"{year}/day/{day}", problemStatement, input);
    //}

    static async Task<Problem> DownloadProblem(IBrowsingContext context, Uri baseUri, int year, int day)
    {
        var uri = baseUri + $"{year}/day/{day}";
        var color = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Updating " + uri);
        Console.ForegroundColor = color;

        var problemStatement = await context.OpenAsync(uri);
        var input = await context.GetService<IDocumentLoader>()!.FetchAsync(
            new DocumentRequest(new Url(baseUri + $"{year}/day/{day}/input"))).Task;

        if (input.StatusCode != HttpStatusCode.OK)
        {
            throw new AocCommunicationError("Could not fetch input", input.StatusCode, new StreamReader(input.Content).ReadToEnd());
        }

        return Problem.Parse(
            year, day, baseUri + $"{year}/day/{day}", problemStatement,
            await new StreamReader(input.Content).ReadToEndAsync()
        );
    }

    static void UpdateReadmeForDay(Problem problem)
    {
        var file = Path.Combine(Environment.CurrentDirectory, Dir(problem.Year, problem.Day), "README.md");
        WriteFile(file, problem.ContentMd);
    }

    static void UpdateSolutionTemplate(Problem problem)
    {
        var file = Path.Combine(Environment.CurrentDirectory, Dir(problem.Year, problem.Day), "Solution.cs");
        if (!File.Exists(file))
        {
            WriteFile(file, SolutionTemplateGenerator.Generate(problem));
        }
    }

    static void UpdateExample(Problem problem)
    {
        var inputFile = Path.Combine(Environment.CurrentDirectory, SolverExtensions.RelativeInputDir(problem.Year, problem.Day), "example.in");
        if (!File.Exists(inputFile))
        {
            WriteFile(inputFile, string.Empty);
        }

        var answerFile = Path.Combine(Environment.CurrentDirectory, SolverExtensions.RelativeInputDir(problem.Year, problem.Day), "example.refout");
        if (!File.Exists(answerFile))
        {
            WriteFile(answerFile, string.Empty);
        }
    }

    static void UpdateReadmeForYear(Calendar calendar)
    {
        var file = Path.Combine(Environment.CurrentDirectory, SolverExtensions.WorkingDir(calendar.Year), "README.md");
        WriteFile(file, ReadmeGeneratorForYear.Generate(calendar));
    }

    static void UpdateSplashScreen(Calendar calendar)
    {
        var file = Path.Combine(Environment.CurrentDirectory, SolverExtensions.WorkingDir(calendar.Year), "SplashScreen.cs");
        WriteFile(file, SplashScreenGenerator.Generate(calendar));
    }

    static void UpdateInput(Problem problem)
    {
        var inputFile = Path.Combine(Environment.CurrentDirectory, SolverExtensions.RelativeInputDir(problem.Year, problem.Day), "input.in");
        if (!File.Exists(inputFile))
        {
            WriteFile(inputFile, problem.Input);
        }
    }

    static void UpdateRefout(Problem problem)
    {
        var file = Path.Combine(Environment.CurrentDirectory, SolverExtensions.RelativeInputDir(problem.Year, problem.Day), "input.refout");
        WriteFile(file, string.Join("\n", problem.Answers));
    }
}
    