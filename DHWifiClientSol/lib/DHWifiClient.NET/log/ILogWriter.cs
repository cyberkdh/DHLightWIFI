//////////////////////////////////////////////////////////////////////////////////////////////////
//	Projects		: DHWifiClient
//	Author			: CYBERKDH
//	Module			: ILogWriter
//	History			:
//	Copyrights		: Copyright (C)CYBERKDH@HOTMAIL.COM. All Rights Reserved.
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
