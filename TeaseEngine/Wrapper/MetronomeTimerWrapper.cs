using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeaseEngine.Controls;
using TeaseEngine.Utils;

namespace TeaseEngine.Wrapper
{
    public class MetronomeTimerWrapper
    {
        private MetronomeTimer MetronomeTimer { get; set; }

        public int BeatsPerMinute { get => MetronomeTimer.BeatsPerMinute; set { Logger.Debug($"Changing bpm to {value}"); MetronomeTimer.BeatsPerMinute = value; } }
        public int DurationInSeconds => MetronomeTimer.DurationInSeconds;
        public int ElapsedSeconds => MetronomeTimer.ElapsedSeconds;
        public int RemainingSeconds => MetronomeTimer.RemainingSeconds;
        public ProgressState State => MetronomeTimer.State;
        /// <summary>
        /// Is triggered every 500ms
        /// object = MetronomeTimer
        /// </summary>
        public event EventHandler UpdateTimerTick;
        /// <summary>
        /// Is triggered every beat
        /// object = MetronomeTimer
        /// </summary>
        public event EventHandler BeatTimerTick;

        private Logger Logger { get; } = App.Logging.GetLogger<MetronomeTimerWrapper>();

        public MetronomeTimerWrapper(MetronomeTimer metronomeTimer)
        {
            MetronomeTimer = metronomeTimer;
            MetronomeTimer.UpdateTimerTick += (sender, e) =>
            {
                UpdateTimerTick?.Invoke(sender, e);
            };
            MetronomeTimer.BeatTimerTick += (sender, e) =>
            {
                BeatTimerTick?.Invoke(sender, e);
            };
        }

        public void StartCount(int strokeCount, int durationInSeconds, bool showProgress = true, bool playMentronome = true, bool useAlternatingSounds = false)
        {
            Logger.Debug($"Starting metronome by count {strokeCount} | {durationInSeconds} sec | show progress {showProgress} | play sound {playMentronome} | alternating {useAlternatingSounds}");

            MetronomeTimer.Start2(strokeCount, durationInSeconds, showProgress, playMentronome, useAlternatingSounds);
        }

        public void StartBpm(int beatsPerMinute, int durationInSeconds, bool showProgress = true, bool playMentronome = true, bool useAlternatingSounds = false)
        {
            Logger.Debug($"Starting metronome by bpm {beatsPerMinute} | {durationInSeconds} sec | show progress {showProgress} | play sound {playMentronome} | alternating {useAlternatingSounds}");

            MetronomeTimer.Start(beatsPerMinute, durationInSeconds, showProgress, playMentronome, useAlternatingSounds);
        }

        public void Stop()
        {
            Logger.Debug("Stopping metronome");

            MetronomeTimer.Stop();
        }

        public void Pause()
        {
            Logger.Debug("Pausing metronome");

            MetronomeTimer.Pause();
        }

        public void Resume()
        {
            Logger.Debug("Resuming metronome");

            MetronomeTimer.Resume();
        }

    }
}
