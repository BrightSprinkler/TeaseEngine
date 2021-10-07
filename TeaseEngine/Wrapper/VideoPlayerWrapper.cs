using System;
using TeaseEngine.Controls;
using TeaseEngine.Utils;

namespace TeaseEngine.Wrapper
{
    public class VideoPlayerWrapper
    {
        private VideoPlayer VideoPlayer { get; set; }
        private Logger Logger { get; } = App.Logging.GetLogger<VideoPlayerWrapper>();

        /// <summary>
        /// Ticks every 250 ms while the video plays
        /// </summary>
        public event EventHandler VideoTimerTick;
        /// <summary>
        /// Is called when the video ends
        /// </summary>
        public event EventHandler VideoEnded;

        public VideoPlayerWrapper(VideoPlayer videoPlayer)
        {
            VideoPlayer = videoPlayer;

            VideoPlayer.VideoTimerTick += (sender, e) =>
            {
                VideoTimerTick?.Invoke(sender, e);
            };
            VideoPlayer.VideoEnded += (sender, e) =>
            {
                VideoEnded?.Invoke(sender, e);
            };
        }

        public void Play(string videoPath)
        {
            VideoPlayer.Play(videoPath);
        }

        public void Pause()
        {
            VideoPlayer.Pause();
        }

        public void Resume()
        {
            VideoPlayer.Resume();
        }

        public void Stop()
        {
            VideoPlayer.Stop();
        }

        public void JumpTo(TimeSpan time)
        {
            VideoPlayer.JumpTo(time);
        }

        public void FastForward(TimeSpan time)
        {
            VideoPlayer.FastForward(time);
        }

        public void Rewind(TimeSpan time)
        {
            VideoPlayer.Rewind(time);
        }
    }
}
