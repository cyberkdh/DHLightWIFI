//////////////////////////////////////////////////////////////////////////////////////////////////
//	Projects		: DHWifiClient
//	Author			: CYBERKDH
//	Module			: Logger
//	History			:
//	Copyrights		: Copyright (C)CYBERKDH@HOTMAIL.COM. All Rights Reserved.
//////////////////////////////////////////////////////////////////////////////////////////////////
using System;

namespace DHWifiClient.NET.log
{
    /// <summary>
    /// Library-wide logger facade. <see cref="FileLogWriter"/> is enabled by default,
    /// and host applications can plug in their own sink (console, database, etc.) via <see cref="SetWriter"/>.
    /// </summary>
    public static class Logger
    {
        private static ILogWriter m_oWriter = new FileLogWriter();

        public static LogLevel MinimumLevel { get; set; } = LogLevel.Info;

        /// <summary>Replaces the log sink. Passing null disables logging.</summary>
        public static void SetWriter(ILogWriter writer)
        {
            m_oWriter = writer;
        }

        public static void Debug(string message) => Write(LogLevel.Debug, message, null);
        public static void Info(string message) => Write(LogLevel.Info, message, null);
        public static void Warn(string message, Exception exception = null) => Write(LogLevel.Warn, message, exception);
        public static void Error(string message, Exception exception = null) => Write(LogLevel.Error, message, exception);

        private static void Write(LogLevel level, string message, Exception exception)
        {
            if (level < MinimumLevel || m_oWriter == null)
            {
                return;
            }

            m_oWriter.Write(level, message, exception);
        }
    }
}
