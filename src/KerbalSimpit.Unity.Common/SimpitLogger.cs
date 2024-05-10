using KerbalSimpit.Core.Enums;
using KerbalSimpit.Core.Utilities;
using System;
using UnityEngine;

namespace KerbalSimpit.Unity.Common
{
    public class SimpitLogger : ISimpitLogger
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
            Debug.Log($"Simpit: {string.Format(template, args)}");
        }

        public void Log(SimpitLogLevelEnum level, Exception ex, string template, object[] args)
        {
            Debug.Log($"Simpit: {string.Format(template, args)}\n{ex}");
        }
    }
}
