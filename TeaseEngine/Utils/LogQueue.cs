using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading.Tasks;
using TeaseEngine.Models;

namespace TeaseEngine.Utils
{
    public class LogQueue : IDisposable
    {
        private PathManager PathManager { get; } = new PathManager();
        private ConcurrentQueue<LogMessage> Messages { get; set; }
        private Task LogTask { get; set; }
        private bool Run { get; set; }
        private Logger Logger { get; set; }

        public LogQueue()
        {
            Logger = GetLogger<LogQueue>();
        }

        public Logger GetLogger<T>() => new Logger(this, typeof(T));
        public Logger GetLogger(Type sender) => new Logger(this, sender);

        public void Add(LogMessage message)
        {
            if (!Run) return;

            Messages.Enqueue(message);
        }

        public void Start()
        {
            Stop();

            Messages = new ConcurrentQueue<LogMessage>();
            Run = true;
            LogTask = new Task(Log);
            LogTask.Start();

            Logger.Info("Logging started");
        }

        public void Stop()
        {
            Logger.Info("Stopping logging");

            Run = false;
            
            if (LogTask != null)
            {
                LogTask.Wait();
                LogTask.Dispose();
            }

            Messages = null;
        }

        private void Log()
        {
            while (Run)
            {
                LogMessage message;
                if (!Messages.TryDequeue(out message)) continue;

                string logFile = Path.Combine(PathManager.LogDirectory, $"{DateTime.Now:dd-MM-yyyy}.log.txt");

                File.AppendAllText(logFile, message.ToString());
            }
        }

        public void Dispose()
        {
            Stop();
        }

    }
}
