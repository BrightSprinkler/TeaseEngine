using System;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using TeaseEngine.Utils;

namespace TeaseEngine.Controls
{
    /// <summary>
    /// Interaction logic for VideoPlayer.xaml
    /// </summary>
    public partial class VideoPlayer : UserControl
    {
        private Logger Logger { get; set; } = App.Logging.GetLogger<VideoPlayer>();
        internal SlideShow SlideShow { get; set; }
        private Timer VideoPlayedTimer { get; set; }
        /// <summary>
        /// Ticks every 250 ms while the video plays
        /// </summary>
        public event EventHandler VideoTimerTick;
        /// <summary>
        /// Is called when the video ends
        /// </summary>
        public event EventHandler VideoEnded;

        public VideoPlayer()
        {
            InitializeComponent();

            VideoPlayedTimer = new Timer();
            VideoPlayedTimer.Elapsed += TimerElapsed;
            VideoPlayerMediaElement.MediaEnded += (sender, e) => {
                Stop();
                VideoEnded?.Invoke(null, e); 
            };
            VideoPlayedTimer.Interval = 250;
            VideoPlayedTimer.Enabled = false;
        }

        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                VideoTimerTick?.Invoke(null, e);
            });
        }

        public void Play(string videoPath)
        {
            Logger.Info($"Playing {videoPath}");
            Stop();

            SlideShow.Stop();
            SlideShow.Visibility = Visibility.Hidden;

            Visibility = Visibility.Visible;
            VideoPlayerMediaElement.Source = new Uri(videoPath, UriKind.RelativeOrAbsolute);
            VideoPlayedTimer.Enabled = true;
            VideoPlayerMediaElement.Play();
        }

        public void Pause()
        {
            Logger.Info("Pausing video player");

            VideoPlayedTimer.Enabled = false;
            VideoPlayerMediaElement.Pause();
        }

        public void Resume()
        {
            Logger.Info("Resuming video player");

            VideoPlayedTimer.Enabled = true;
            VideoPlayerMediaElement.Play();
        }

        public void Stop()
        {
            Logger.Info("Stopping video player");

            VideoPlayedTimer.Enabled = false;
            VideoPlayerMediaElement.Stop();
            VideoPlayerMediaElement.Source = null;

            Visibility = Visibility.Hidden;
            SlideShow.Visibility = Visibility.Visible;
        }

        public void JumpTo(TimeSpan time)
        {
            Logger.Info($"Jumping to {time}");

            VideoPlayerMediaElement.Position = time;
        }

        public void FastForward(TimeSpan time)
        {
            Logger.Info($"Fast forwarding by {time}");

            VideoPlayerMediaElement.Position = VideoPlayerMediaElement.Position.Add(time);
        }

        public void Rewind(TimeSpan time)
        {
            Logger.Info($"Rewinding by {time}");

            VideoPlayerMediaElement.Position = VideoPlayerMediaElement.Position.Subtract(time);
        }
    }
}
