using System;
using TeaseEngine.Models;

namespace TeaseEngine.Utils
{
    public class Logger
    {
        private LogQueue LogQueue { get; set; }
        private Type Sender { get; set; }

        public Logger(LogQueue logQueue, Type sender) { LogQueue = logQueue; Sender = sender; }

        public void Trace(string text)
        {
            LogQueue.Add(new LogMessage(MessageType.Trace, text, Sender));
        }

        public void Debug(string text)
        {
            LogQueue.Add(new LogMessage(MessageType.Debug, text, Sender));
        }

        public void Info(string text)
        {
            LogQueue.Add(new LogMessage(MessageType.Info, text, Sender));
        }

        public void Warn(string text)
        {
            LogQueue.Add(new LogMessage(MessageType.Warning, text, Sender));
        }

        public void Warn(Exception ex)
        {
            Warn(ex.ToString());
        }

        public void Error(string text)
        {
            LogQueue.Add(new LogMessage(MessageType.Error, text, Sender));
        }

        public void Error(Exception ex)
        {
            Error(ex.ToString());
        }

    }
}
