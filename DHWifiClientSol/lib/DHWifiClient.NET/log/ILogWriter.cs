//////////////////////////////////////////////////////////////////////////////////////////////////
//	Projects		: DHWifiClient
//	Author			: CYBERKDH
//	Module			: ILogWriter
//	History			:
//	Copyrights		: Copyright (C)CYBERKDH. Licensed under the MIT License.
//////////////////////////////////////////////////////////////////////////////////////////////////
using System;

namespace DHWifiClient.NET.log
{
    /// <summary>Abstracts a log output destination (sink). Host applications can implement this and plug it in via <see cref="Logger.SetWriter"/>.</summary>
    public interface ILogWriter
    {
        void Write(LogLevel level, string message, Exception exception);
    }
}
