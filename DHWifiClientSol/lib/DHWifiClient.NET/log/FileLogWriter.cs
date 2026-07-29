//////////////////////////////////////////////////////////////////////////////////////////////////
//	Projects		: DHWifiClient
//	Author			: CYBERKDH
//	Module			: FileLogWriter
//	History			:
//	Copyrights		: Copyright (C)CYBERKDH. Licensed under the MIT License.
//////////////////////////////////////////////////////////////////////////////////////////////////
using System;
using System.IO;

namespace DHWifiClient.NET.log
{
    /// <summary>Default <see cref="ILogWriter"/> implementation that writes logs to a daily text file.</summary>
    public class FileLogWriter : ILogWriter
    {
        private readonly object m_oSyncRoot = new object();
        private readonly string m_strDirectory;

        public FileLogWriter(string directory = null)
        {
            m_strDirectory = directory ?? Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
                "DHWifiClient", "logs");
        }

        public void Write(LogLevel level, string message, Exception exception)
        {
            try
            {
                lock (m_oSyncRoot)
                {
                    Directory.CreateDirectory(m_strDirectory);
                    string strFilePath = Path.Combine(m_strDirectory, $"DHWifiClient_{DateTime.Now:yyyyMMdd}.log");

                    string strLine = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] [{level}] {message}";
                    if (exception != null)
                    {
                        strLine += Environment.NewLine + exception;
                    }

                    File.AppendAllText(strFilePath, strLine + Environment.NewLine);
                }
            }
            catch
            {
                // Swallow logging failures so they never affect library behavior.
            }
        }
    }
}
