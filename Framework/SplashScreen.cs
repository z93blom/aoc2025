using System.Text;
using Spectre.Console;

namespace AdventOfCode.Framework;

public abstract class SplashScreen :  ISplashScreen
{
    readonly FigletFont _font;

    protected SplashScreen()
    {
        _font = FigletFont.Parse(Encoding.UTF8.GetString(Resource.serifcap));
    }

    public abstract void Show();

    protected static void Write(int rgb, bool bold, string text)
    {
        var b = bold ? "bold " : "";
        AnsiConsole.Markup($"[{b}#{rgb:x6}]{text.EscapeMarkup()}[/]");
    }

    protected void WriteFiglet(string text)
    {
        AnsiConsole.Write(new FigletText(_font, text));
    }

    protected void WriteFiglet(string text, Color color)
    {
        AnsiConsole.Write(new FigletText(_font, text).Color(color));
    }

}