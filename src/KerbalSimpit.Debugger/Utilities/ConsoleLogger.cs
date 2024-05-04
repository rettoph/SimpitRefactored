using KerbalSimpit.Core.Enums;
using KerbalSimpit.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KerbalSimpit.Debugger.Utilities
{
    internal class ConsoleLogger : ISimpitLogger
    {
        public SimpitLogLevelEnum LogLevel { get; set; }

        public ConsoleLogger(SimpitLogLevelEnum logLevel)
        {
            this.LogLevel = logLevel;
        }

        public void Log(SimpitLogLevelEnum level, string template, object[] args)
        {
            Console.ForegroundColor = this.GetColor(level);

            Console.WriteLine($"[{DateTime.Now}][{level}] {string.Format(template, args)}");
        }

        public void Log(SimpitLogLevelEnum level, Exception ex, string template, object[] args)
        {
            Console.ForegroundColor = this.GetColor(level);

            Console.WriteLine($"[{DateTime.Now}][{level}] {string.Format(template, args)}\n{ex}");
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
