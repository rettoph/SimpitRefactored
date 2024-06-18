using KerbalSimpit.Common.Core.Enums;
using KerbalSimpit.Common.Core.Utilities;
using PimDeWitte.UnityMainThreadDispatcher;
using System;
using UnityEngine;

namespace KerbalSimpit.Unity.Common
{
    public sealed class SimpitLogger : ISimpitLogger
    {
        public static SimpitLogger Instance { get; private set; }
        public SimpitLogLevelEnum LogLevel { get; set; } = SimpitLogLevelEnum.Verbose;

        public SimpitLogger()
        {
            SimpitLogger.Instance = this;

            this.LogLevel = SimpitConfiguration.Instance.LogLevel;
        }

        public void Log(SimpitLogLevelEnum level, string template, object[] args)
        {
            UnityMainThreadDispatcher.Instance().Enqueue(() =>
            {
                Debug.Log($"[{level}] Simpit: {string.Format(template, args)}");
            });
        }

        public void Log(SimpitLogLevelEnum level, Exception ex, string template, object[] args)
        {
            UnityMainThreadDispatcher.Instance().Enqueue(() =>
            {
                Debug.LogError($"[{level}] Simpit: {string.Format(template, args)}\n{ex}");
            });
        }
    }
}
