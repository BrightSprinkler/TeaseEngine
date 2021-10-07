using System;
using TeaseEngine.Utils;

namespace TeaseEngine.Wrapper
{
    public class StopwatchWrapper
    {
        private DateTime StartTime { get; set; }
        private Logger Logger { get; } = App.Logging.GetLogger<StopwatchWrapper>();
        
        public void Start()
        {
            Logger.Debug("Starting stopwatch");

            StartTime = DateTime.Now;
        }

        /// <summary>
        /// returns the elapsed milliseconds
        /// </summary>
        /// <returns></returns>
        public int Stop()
        {
            Logger.Debug("Stopping stopwatch");

            return (int)(DateTime.Now - StartTime).TotalMilliseconds;
        }

    }
}
