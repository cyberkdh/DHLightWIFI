//////////////////////////////////////////////////////////////////////////////////////////////////
//	Projects		: DHWifiClient
//	Author			: CYBERKDH
//	Module			: TextBoxLogWriter
//	History			:
//	Copyrights		: Copyright (C)CYBERKDH. Licensed under the MIT License.
//////////////////////////////////////////////////////////////////////////////////////////////////
using System;
using System.Windows.Forms;
using DHWifiClient.NET.log;

namespace DHWifiClientSample
{
    /// <summary>Log sink that writes to both the on-screen log panel (TextBox) and a file (FileLogWriter).</summary>
    internal class TextBoxLogWriter : ILogWriter
    {
        private readonly TextBox m_oTextBox;
        private readonly ILogWriter m_oFileWriter;

        /// <summary>When false, log entries are still shown in the TextBox but are not written to the log file.</summary>
        public bool FileLoggingEnabled { get; set; } = true;

        public TextBoxLogWriter(TextBox textBox, string logDirectory = null)
        {
            m_oTextBox = textBox ?? throw new ArgumentNullException(nameof(textBox));
            m_oFileWriter = new FileLogWriter(logDirectory);
        }

        public void Write(LogLevel level, string message, Exception exception)
        {
            if (FileLoggingEnabled)
            {
                m_oFileWriter.Write(level, message, exception);
            }

            string strLine = $"[{DateTime.Now:HH:mm:ss}] [{level}] {message}"
                + (exception != null ? $" - {exception.Message}" : string.Empty);

            if (m_oTextBox.IsDisposed)
            {
                return;
            }

            if (m_oTextBox.InvokeRequired)
            {
                m_oTextBox.BeginInvoke(new Action(() => AppendLine(strLine)));
            }
            else
            {
                AppendLine(strLine);
            }
        }

        private void AppendLine(string line)
        {
            if (m_oTextBox.IsDisposed)
            {
                return;
            }

            m_oTextBox.AppendText(line + Environment.NewLine);
        }
    }
}
