using KerbalSimpit.Common.Core.Enums;
using KerbalSimpit.Common.Core.Utilities;

namespace KerbalSimpit.Debugger
{
    internal class BasicSimpitLogger : ISimpitLogger
    {
        private static object _lock = new();

        public SimpitLogLevelEnum LogLevel { get; set; }

        public BasicSimpitLogger(SimpitLogLevelEnum logLevel)
        {
            this.LogLevel = logLevel;
        }

        public void Log(SimpitLogLevelEnum level, string template, object[] args)
        {
            lock (_lock)
            {
                Console.ForegroundColor = this.GetColor(level);

                Console.WriteLine($"[{DateTime.Now}][{level}] {string.Format(template, args)}");
            }
        }

        public void Log(SimpitLogLevelEnum level, Exception ex, string template, object[] args)
        {
            lock (_lock)
            {
                Console.ForegroundColor = this.GetColor(level);

                Console.WriteLine($"[{DateTime.Now}][{level}] {string.Format(template, args)}\n{ex}");
            }
        }

        private ConsoleColor GetColor(SimpitLogLevelEnum level)
        {
            switch (level)
            {
                case SimpitLogLevelEnum.Error:
                    return ConsoleColor.Red;
                case SimpitLogLevelEnum.Warning:
                    return ConsoleColor.Yellow;
                case SimpitLogLevelEnum.Information:
                    return ConsoleColor.White;
                case SimpitLogLevelEnum.Debug:
                    return ConsoleColor.Cyan;
                case SimpitLogLevelEnum.Verbose:
                    return ConsoleColor.Magenta;
            }

            throw new ArgumentOutOfRangeException(nameof(level));
        }
    }
}
