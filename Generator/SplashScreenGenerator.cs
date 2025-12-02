using System.Text;
using AdventOfCode.Model;
using AdventOfCode.Utilities;

namespace AdventOfCode.Generator;

public class SplashScreenGenerator {
    public static string Generate(Calendar calendar)
    {
        string calendarPrinter = CalendarPrinter(calendar);
        return $$"""
            using AdventOfCode.Framework;
            namespace AdventOfCode.Y{{calendar.Year}};
            
            [RegisterKeyedTransient("{{calendar.Year}}", typeof(ISplashScreen))] partial class SplashScreenImpl { }
            partial class SplashScreenImpl : SplashScreen
            {
                public override void Show()
                {
                    WriteFiglet("Advent of code {{calendar.Year}}", Spectre.Console.Color.Yellow);
                    {{calendarPrinter.Indent(8)}}
                    Console.WriteLine();
                }
            }
            """;
    }

    private static string CalendarPrinter(Calendar calendar) {
        var lines = calendar.Lines.Select(line =>
            new[] { new CalendarToken { Text = "           " } }.Concat(line)).ToList();

        var bw = new BufferWriter();
        foreach (var line in lines)
        {
            foreach (var token in line)
            {
                bw.Write(token.ConsoleColor, token.Text, token.Bold);
            }

            bw.Write(-1, "\n", false);
        }
        return bw.GetContent();
    }

    class BufferWriter {
        readonly StringBuilder sb = new();
        int bufferColor = -1;
        string buffer = "";
        private bool bufferBold;

        public void Write(int color, string text, bool bold) 
        {
            if (!string.IsNullOrWhiteSpace(text)) {
                if ((color != bufferColor || bufferBold != bold) && !string.IsNullOrWhiteSpace(buffer)) 
                {
                    Flush();
                }
                
                bufferColor = color;
                bufferBold = bold;
            }

            buffer += text;
        }

        private void Flush() {
            while (buffer.Length > 0) {
                var block = buffer[..Math.Min(100, buffer.Length)];
                buffer = buffer[block.Length..];
                block = block.Replace("\\", "\\\\").Replace("\"", "\\\"").Replace("\r", "\\r").Replace("\n", "\\n");
                sb.AppendLine($@"Write(0x{bufferColor:x6}, {bufferBold.ToString().ToLowerInvariant()}, ""{block}"");");
            }
            buffer = "";
        }

        public string GetContent() {
            Flush();
            return sb.ToString();
        }
    }
}
