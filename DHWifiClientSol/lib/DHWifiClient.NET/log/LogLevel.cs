//////////////////////////////////////////////////////////////////////////////////////////////////
//	Projects		: DHWifiClient
//	Author			: CYBERKDH
//	Module			: LogLevel
//	History			:
//	Copyrights		: Copyright (C)CYBERKDH. Licensed under the MIT License.
//////////////////////////////////////////////////////////////////////////////////////////////////
namespace DHWifiClient.NET.log
{
    /// <summary>Severity level for a log message written via <see cref="Logger"/>.</summary>
    public enum LogLevel
    {
        /// <summary>Fine-grained diagnostic detail, not needed under normal operation.</summary>
        Debug,
        /// <summary>General informational messages about normal operation.</summary>
        Info,
        /// <summary>An unexpected but recoverable condition.</summary>
        Warn,
        /// <summary>An error that prevented an operation from completing.</summary>
        Error,
    }
}
