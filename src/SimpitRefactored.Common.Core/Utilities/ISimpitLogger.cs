using SimpitRefactored.Common.Core.Enums;
using System;

namespace SimpitRefactored.Common.Core.Utilities
{
    public interface ISimpitLogger
    {
        SimpitLogLevelEnum LogLevel { get; set; }

        /// <summary>
        /// Log the given message regardless of the current <see cref="LogLevel"/>
        /// </summary>
        /// <param name="level"></param>
        /// <param name="template"></param>
        /// <param name="args"></param>
        /// <param name="ex"></param>
        void Log(SimpitLogLevelEnum level, string template, object[] args);

        /// <summary>
        /// Log the given message regardless of the current <see cref="LogLevel"/>
        /// </summary>
        /// <param name="level"></param>
        /// <param name="template"></param>
        /// <param name="args"></param>
        /// <param name="ex"></param>
        void Log(SimpitLogLevelEnum level, Exception ex, string template, object[] args);
    }

    public static class SimpitLoggerExtensions
    {
        #region LogError Ex
        public static void LogError(this ISimpitLogger logger, Exception ex, string message)
        {
            if (logger.LogLevel < SimpitLogLevelEnum.Error)
            {
                return;
            }

            logger.Log(SimpitLogLevelEnum.Error, ex, message, Array.Empty<object>());
        }

        public static void LogError<T1>(this ISimpitLogger logger, Exception ex, string message, T1 arg1)
        {
            if (logger.LogLevel < SimpitLogLevelEnum.Error)
            {
                return;
            }

            logger.Log(SimpitLogLevelEnum.Error, ex, message, new object[] { arg1 });
        }

        public static void LogError<T1, T2>(this ISimpitLogger logger, Exception ex, string message, T1 arg1, T2 arg2)
        {
            if (logger.LogLevel < SimpitLogLevelEnum.Error)
            {
                return;
            }

            logger.Log(SimpitLogLevelEnum.Error, ex, message, new object[] { arg1, arg2 });
        }

        public static void LogError<T1, T2, T3>(this ISimpitLogger logger, Exception ex, string message, T1 arg1, T2 arg2, T3 arg3)
        {
            if (logger.LogLevel < SimpitLogLevelEnum.Error)
            {
                return;
            }

            logger.Log(SimpitLogLevelEnum.Error, ex, message, new object[] { arg1, arg2, arg3 });
        }

        public static void LogError<T1, T2, T3, T4>(this ISimpitLogger logger, Exception ex, string message, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            if (logger.LogLevel < SimpitLogLevelEnum.Error)
            {
                return;
            }

            logger.Log(SimpitLogLevelEnum.Error, ex, message, new object[] { arg1, arg2, arg3, arg4 });
        }

        public static void LogError<T1, T2, T3, T4, T5>(this ISimpitLogger logger, Exception ex, string message, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
            if (logger.LogLevel < SimpitLogLevelEnum.Error)
            {
                return;
            }

            logger.Log(SimpitLogLevelEnum.Error, ex, message, new object[] { arg1, arg2, arg3, arg4, arg5 });
        }

        public static void LogError(this ISimpitLogger logger, Exception ex, string message, params object[] args)
        {
            if (logger.LogLevel < SimpitLogLevelEnum.Error)
            {
                return;
            }

            logger.Log(SimpitLogLevelEnum.Error, ex, message, args);
        }
        #endregion

        #region LogError
        public static void LogError(this ISimpitLogger logger, string message)
        {
            if (logger.LogLevel < SimpitLogLevelEnum.Error)
            {
                return;
            }

            logger.Log(SimpitLogLevelEnum.Error, message, Array.Empty<object>());
        }

        public static void LogError<T1>(this ISimpitLogger logger, string message, T1 arg1)
        {
            if (logger.LogLevel < SimpitLogLevelEnum.Error)
            {
                return;
            }

            logger.Log(SimpitLogLevelEnum.Error, message, new object[] { arg1 });
        }

        public static void LogError<T1, T2>(this ISimpitLogger logger, string message, T1 arg1, T2 arg2)
        {
            if (logger.LogLevel < SimpitLogLevelEnum.Error)
            {
                return;
            }

            logger.Log(SimpitLogLevelEnum.Error, message, new object[] { arg1, arg2 });
        }

        public static void LogError<T1, T2, T3>(this ISimpitLogger logger, string message, T1 arg1, T2 arg2, T3 arg3)
        {
            if (logger.LogLevel < SimpitLogLevelEnum.Error)
            {
                return;
            }

            logger.Log(SimpitLogLevelEnum.Error, message, new object[] { arg1, arg2, arg3 });
        }

        public static void LogError<T1, T2, T3, T4>(this ISimpitLogger logger, string message, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            if (logger.LogLevel < SimpitLogLevelEnum.Error)
            {
                return;
            }

            logger.Log(SimpitLogLevelEnum.Error, message, new object[] { arg1, arg2, arg3, arg4 });
        }

        public static void LogError<T1, T2, T3, T4, T5>(this ISimpitLogger logger, string message, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
            if (logger.LogLevel < SimpitLogLevelEnum.Error)
            {
                return;
            }

            logger.Log(SimpitLogLevelEnum.Error, message, new object[] { arg1, arg2, arg3, arg4, arg5 });
        }

        public static void LogError(this ISimpitLogger logger, string message, params object[] args)
        {
            if (logger.LogLevel < SimpitLogLevelEnum.Error)
            {
                return;
            }

            logger.Log(SimpitLogLevelEnum.Error, message, args);
        }
        #endregion

        #region LogWarning
        public static void LogWarning(this ISimpitLogger logger, string message)
        {
            if (logger.LogLevel < SimpitLogLevelEnum.Warning)
            {
                return;
            }

            logger.Log(SimpitLogLevelEnum.Warning, message, Array.Empty<object>());
        }

        public static void LogWarning<T1>(this ISimpitLogger logger, string message, T1 arg1)
        {
            if (logger.LogLevel < SimpitLogLevelEnum.Warning)
            {
                return;
            }

            logger.Log(SimpitLogLevelEnum.Warning, message, new object[] { arg1 });
        }

        public static void LogWarning<T1, T2>(this ISimpitLogger logger, string message, T1 arg1, T2 arg2)
        {
            if (logger.LogLevel < SimpitLogLevelEnum.Warning)
            {
                return;
            }

            logger.Log(SimpitLogLevelEnum.Warning, message, new object[] { arg1, arg2 });
        }

        public static void LogWarning<T1, T2, T3>(this ISimpitLogger logger, string message, T1 arg1, T2 arg2, T3 arg3)
        {
            if (logger.LogLevel < SimpitLogLevelEnum.Warning)
            {
                return;
            }

            logger.Log(SimpitLogLevelEnum.Warning, message, new object[] { arg1, arg2, arg3 });
        }

        public static void LogWarning<T1, T2, T3, T4>(this ISimpitLogger logger, string message, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            if (logger.LogLevel < SimpitLogLevelEnum.Warning)
            {
                return;
            }

            logger.Log(SimpitLogLevelEnum.Warning, message, new object[] { arg1, arg2, arg3, arg4 });
        }

        public static void LogWarning<T1, T2, T3, T4, T5>(this ISimpitLogger logger, string message, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
            if (logger.LogLevel < SimpitLogLevelEnum.Warning)
            {
                return;
            }

            logger.Log(SimpitLogLevelEnum.Error, message, new object[] { arg1, arg2, arg3, arg4, arg5 });
        }

        public static void LogWarning(this ISimpitLogger logger, string message, params object[] args)
        {
            if (logger.LogLevel < SimpitLogLevelEnum.Warning)
            {
                return;
            }

            logger.Log(SimpitLogLevelEnum.Warning, message, args);
        }
        #endregion

        #region LogInformation
        public static void LogInformation(this ISimpitLogger logger, string message)
        {
            if (logger.LogLevel < SimpitLogLevelEnum.Information)
            {
                return;
            }

            logger.Log(SimpitLogLevelEnum.Information, message, Array.Empty<object>());
        }

        public static void LogInformation<T1>(this ISimpitLogger logger, string message, T1 arg1)
        {
            if (logger.LogLevel < SimpitLogLevelEnum.Information)
            {
                return;
            }

            logger.Log(SimpitLogLevelEnum.Information, message, new object[] { arg1 });
        }

        public static void LogInformation<T1, T2>(this ISimpitLogger logger, string message, T1 arg1, T2 arg2)
        {
            if (logger.LogLevel < SimpitLogLevelEnum.Information)
            {
                return;
            }

            logger.Log(SimpitLogLevelEnum.Information, message, new object[] { arg1, arg2 });
        }

        public static void LogInformation<T1, T2, T3>(this ISimpitLogger logger, string message, T1 arg1, T2 arg2, T3 arg3)
        {
            if (logger.LogLevel < SimpitLogLevelEnum.Information)
            {
                return;
            }

            logger.Log(SimpitLogLevelEnum.Information, message, new object[] { arg1, arg2, arg3 });
        }

        public static void LogInformation<T1, T2, T3, T4>(this ISimpitLogger logger, string message, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            if (logger.LogLevel < SimpitLogLevelEnum.Information)
            {
                return;
            }

            logger.Log(SimpitLogLevelEnum.Information, message, new object[] { arg1, arg2, arg3, arg4 });
        }

        public static void LogInformation<T1, T2, T3, T4, T5>(this ISimpitLogger logger, string message, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
            if (logger.LogLevel < SimpitLogLevelEnum.Information)
            {
                return;
            }

            logger.Log(SimpitLogLevelEnum.Information, message, new object[] { arg1, arg2, arg3, arg4, arg5 });
        }

        public static void LogInformation(this ISimpitLogger logger, string message, params object[] args)
        {
            if (logger.LogLevel < SimpitLogLevelEnum.Information)
            {
                return;
            }

            logger.Log(SimpitLogLevelEnum.Information, message, args);
        }
        #endregion

        #region LogDebug
        public static void LogDebug(this ISimpitLogger logger, string message)
        {
            if (logger.LogLevel < SimpitLogLevelEnum.Debug)
            {
                return;
            }

            logger.Log(SimpitLogLevelEnum.Debug, message, Array.Empty<object>());
        }

        public static void LogDebug<T1>(this ISimpitLogger logger, string message, T1 arg1)
        {
            if (logger.LogLevel < SimpitLogLevelEnum.Debug)
            {
                return;
            }

            logger.Log(SimpitLogLevelEnum.Debug, message, new object[] { arg1 });
        }

        public static void LogDebug<T1, T2>(this ISimpitLogger logger, string message, T1 arg1, T2 arg2)
        {
            if (logger.LogLevel < SimpitLogLevelEnum.Debug)
            {
                return;
            }

            logger.Log(SimpitLogLevelEnum.Debug, message, new object[] { arg1, arg2 });
        }

        public static void LogDebug<T1, T2, T3>(this ISimpitLogger logger, string message, T1 arg1, T2 arg2, T3 arg3)
        {
            if (logger.LogLevel < SimpitLogLevelEnum.Debug)
            {
                return;
            }

            logger.Log(SimpitLogLevelEnum.Debug, message, new object[] { arg1, arg2, arg3 });
        }

        public static void LogDebug<T1, T2, T3, T4>(this ISimpitLogger logger, string message, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            if (logger.LogLevel < SimpitLogLevelEnum.Debug)
            {
                return;
            }

            logger.Log(SimpitLogLevelEnum.Debug, message, new object[] { arg1, arg2, arg3, arg4 });
        }

        public static void LogDebug<T1, T2, T3, T4, T5>(this ISimpitLogger logger, string message, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
            if (logger.LogLevel < SimpitLogLevelEnum.Debug)
            {
                return;
            }

            logger.Log(SimpitLogLevelEnum.Debug, message, new object[] { arg1, arg2, arg3, arg4, arg5 });
        }

        public static void LogDebug(this ISimpitLogger logger, string message, params object[] args)
        {
            if (logger.LogLevel < SimpitLogLevelEnum.Debug)
            {
                return;
            }

            logger.Log(SimpitLogLevelEnum.Debug, message, args);
        }
        #endregion

        #region LogVerbose
        public static void LogVerbose(this ISimpitLogger logger, string message)
        {
            if (logger.LogLevel < SimpitLogLevelEnum.Verbose)
            {
                return;
            }

            logger.Log(SimpitLogLevelEnum.Verbose, message, Array.Empty<object>());
        }

        public static void LogVerbose<T1>(this ISimpitLogger logger, string message, T1 arg1)
        {
            if (logger.LogLevel < SimpitLogLevelEnum.Verbose)
            {
                return;
            }

            logger.Log(SimpitLogLevelEnum.Verbose, message, new object[] { arg1 });
        }

        public static void LogVerbose<T1, T2>(this ISimpitLogger logger, string message, T1 arg1, T2 arg2)
        {
            if (logger.LogLevel < SimpitLogLevelEnum.Verbose)
            {
                return;
            }

            logger.Log(SimpitLogLevelEnum.Verbose, message, new object[] { arg1, arg2 });
        }

        public static void LogVerbose<T1, T2, T3>(this ISimpitLogger logger, string message, T1 arg1, T2 arg2, T3 arg3)
        {
            if (logger.LogLevel < SimpitLogLevelEnum.Verbose)
            {
                return;
            }

            logger.Log(SimpitLogLevelEnum.Verbose, message, new object[] { arg1, arg2, arg3 });
        }

        public static void LogVerbose<T1, T2, T3, T4>(this ISimpitLogger logger, string message, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            if (logger.LogLevel < SimpitLogLevelEnum.Verbose)
            {
                return;
            }

            logger.Log(SimpitLogLevelEnum.Verbose, message, new object[] { arg1, arg2, arg3, arg4 });
        }

        public static void LogVerbose<T1, T2, T3, T4, T5>(this ISimpitLogger logger, string message, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
            if (logger.LogLevel < SimpitLogLevelEnum.Verbose)
            {
                return;
            }

            logger.Log(SimpitLogLevelEnum.Verbose, message, new object[] { arg1, arg2, arg3, arg4, arg5 });
        }

        public static void LogVerbose(this ISimpitLogger logger, string message, params object[] args)
        {
            if (logger.LogLevel < SimpitLogLevelEnum.Verbose)
            {
                return;
            }

            logger.Log(SimpitLogLevelEnum.Verbose, message, args);
        }
        #endregion
    }
}
