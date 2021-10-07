using System;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using TeaseEngine.Utils;

namespace TeaseEngine.Controls
{
    /// <summary>
    /// Interaction logic for MetronomeTimer.xaml
    /// </summary>
    public partial class MetronomeTimer : UserControl
    {
        private int Bpm;
        public int BeatsPerMinute
        {
            get
            {
                return Bpm;
            }
            set
            {
                Bpm = value;
                BeatTimer.Interval = 60000 / value;
            }
        }
        public int DurationInSeconds { get; private set; }
        public int ElapsedSeconds { get; private set; }
        public int RemainingSeconds => DurationInSeconds - ElapsedSeconds;
        public bool ShowProgress { get; private set; }
        public bool PlayMetronome { get; private set; }
        public bool UseAlternatingSounds { get; private set; }
        private bool DidPlayMainSound { get; set; }
        public ProgressState State { get; private set; }

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

        private Timer UpdateTimer { get; set; }
        private Timer BeatTimer { get; set; }
        private DateTime LastUpdate { get; set; }
        private SoundService Sound { get; set; }

        public MetronomeTimer()
        {
            InitializeComponent();

            Sound = new SoundService();

            BeatTimer = new Timer();
            BeatTimer.Elapsed += BeatTimerElapsed;
            BeatTimer.Enabled = false;

            UpdateTimer = new Timer(100);
            UpdateTimer.Elapsed += UpdateTimerElapsed;
            UpdateTimer.Enabled = false;

            ProgressBar.Minimum = 0;
        }

        private void BeatTimerElapsed(object sender, ElapsedEventArgs e)
        {
            if (PlayMetronome)
            {
                if (UseAlternatingSounds)
                {
                    if (DidPlayMainSound)
                        Sound.PlayWav(Properties.Resources.metronome2);
                    else
                        Sound.PlayWav(Properties.Resources.metronome);

                    DidPlayMainSound = !DidPlayMainSound;
                }
                else
                    Sound.PlayWav(Properties.Resources.metronome);
            }

            BeatTimerTick?.Invoke(this, new EventArgs());
        }

        private void UpdateTimerElapsed(object sender, ElapsedEventArgs e)
        {
            if (LastUpdate.AddSeconds(1) <= DateTime.Now)
            {
                ElapsedSeconds += 1;
                LastUpdate = DateTime.Now;
            }

            Dispatcher.Invoke(() =>
            {
                if (ElapsedSeconds >= DurationInSeconds)
                {
                    Stop();
                    return;
                }

                if (ShowProgress)
                {
                    if (RemainingSeconds <= 60)
                        TimeLabel.Content = $"{RemainingSeconds}";
                    else
                        TimeLabel.Content = $"{(RemainingSeconds / 60).ToString().PadLeft(2, '0')}:{(RemainingSeconds % 60).ToString().PadLeft(2, '0')}";

                    ProgressBar.Value = ElapsedSeconds;
                }
                else
                {
                    TimeLabel.Content = "?";
                }
            });

            UpdateTimerTick?.Invoke(this, new EventArgs());
        }

        public void Start2(int strokeCount, int durationInSeconds, bool showProgress = true, bool playMentronome = true, bool useAlternatingSounds = false)
        {
            Start((int)(strokeCount / (durationInSeconds / 60.0)), durationInSeconds, showProgress, playMentronome, useAlternatingSounds);
        }

        public void Start(int beatsPerMinute, int durationInSeconds, bool showProgress = true, bool playMentronome = true, bool useAlternatingSounds = false)
        {
            BeatsPerMinute = beatsPerMinute;
            DurationInSeconds = durationInSeconds;
            ShowProgress = showProgress;
            PlayMetronome = playMentronome;
            UseAlternatingSounds = useAlternatingSounds;
            Start();
        }

        public void Start()
        {
            Stop();

            ProgressBar.Maximum = DurationInSeconds;
            ProgressBar.Value = 0;
            BeatTimer.Interval = 60000 / BeatsPerMinute; // https://guitargearfinder.com/guides/convert-ms-milliseconds-bpm-beats-per-minute-vice-versa/
            ElapsedSeconds = 0;
            Visibility = Visibility.Visible;
            UpdateTimer.Enabled = true;
            BeatTimer.Enabled = true;
            DidPlayMainSound = true;

            if (PlayMetronome) Sound.PlayWav(Properties.Resources.metronome);
            UpdateTimerElapsed(this, null);
            State = ProgressState.InProgress;
        }

        public void Stop()
        {
            UpdateTimer.Enabled = false;
            BeatTimer.Enabled = false;
            ProgressBar.Value = 0;
            ElapsedSeconds = 0;
            State = ProgressState.Finished;
            Visibility = Visibility.Hidden;
        }

        public void Pause()
        {
            State = ProgressState.Paused;
            UpdateTimer.Enabled = false;
            BeatTimer.Enabled = false;
        }

        public void Resume()
        {
            State = ProgressState.InProgress;
            UpdateTimer.Enabled = true;
            BeatTimer.Enabled = true;
        }

    }

    public enum ProgressState
    {
        Finished,
        InProgress,
        Paused
    }

}
