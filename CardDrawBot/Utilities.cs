using System;
using System.Threading.Tasks;
using Discord;

namespace CardDrawBot
{
    internal static class Utilities
    {
        public static async Task Log(LogSeverity severity, string message)
        {
            Console.ForegroundColor = GetConsoleColor(severity);
            Console.WriteLine(message);
            Console.ResetColor();
        }

        private static ConsoleColor GetConsoleColor(LogSeverity severity)
        {
            switch (severity)
            {
                case LogSeverity.Critical:
                    return ConsoleColor.Red;
                case LogSeverity.Error:
                    return ConsoleColor.Red;
                case LogSeverity.Warning:
                    return ConsoleColor.DarkYellow;
                case LogSeverity.Info:
                    return ConsoleColor.Black;
                case LogSeverity.Verbose:
                    return ConsoleColor.Black;
                case LogSeverity.Debug:
                    return ConsoleColor.Black;
                default:
                    throw new ArgumentOutOfRangeException(nameof(severity), severity, null);
            }
        }
    }
}