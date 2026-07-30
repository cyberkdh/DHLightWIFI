//////////////////////////////////////////////////////////////////////////////////////////////////
//	Projects		: DHWifiClient
//	Author			: CYBERKDH
//	Module			: Logger
//	History			:
//	Copyrights		: Copyright (C)CYBERKDH. Licensed under the MIT License.
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

        /// <summary>Minimum level a message must meet to be written. Messages below this level are dropped.</summary>
        public static LogLevel MinimumLevel { get; set; } = LogLevel.Info;

        /// <summary>Replaces the log sink. Passing null disables logging.</summary>
        public static void SetWriter(ILogWriter writer)
        {
            m_oWriter = writer;
        }

        /// <summary>Writes a message at <see cref="LogLevel.Debug"/>.</summary>
        public static void Debug(string message) => Write(LogLevel.Debug, message, null);
        /// <summary>Writes a message at <see cref="LogLevel.Info"/>.</summary>
        public static void Info(string message) => Write(LogLevel.Info, message, null);
        /// <summary>Writes a message at <see cref="LogLevel.Warn"/>, optionally attaching an exception.</summary>
        public static void Warn(string message, Exception exception = null) => Write(LogLevel.Warn, message, exception);
        /// <summary>Writes a message at <see cref="LogLevel.Error"/>, optionally attaching an exception.</summary>
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
