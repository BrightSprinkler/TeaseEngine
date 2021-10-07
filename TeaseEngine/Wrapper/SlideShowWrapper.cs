using System.Drawing;
using System.Windows.Media.Imaging;
using TeaseEngine.Controls;
using TeaseEngine.Utils;

namespace TeaseEngine.Wrapper
{
    public class SlideShowWrapper
    {
        private SlideShow SlideShow { get; set; }
        private Logger Logger { get; } = App.Logging.GetLogger<SlideShowWrapper>();

        public SlideShowWrapper(SlideShow slideShow)
        {
            SlideShow = slideShow;
        }

        public void Start(string directoryPath, int intervalInMilliseconds = -1, bool randomized = true)
        {
            Logger.Debug($"Starting slideshow {directoryPath} | interval {intervalInMilliseconds} | random {randomized}");

            SlideShow.Start(directoryPath, intervalInMilliseconds, randomized);
        }

        /// <summary>
        /// Removes the current image
        /// </summary>
        public void Clear()
        {
            Logger.Debug("Clearing slideshow");

            SlideShow.Clear();
        }

        /// <summary>
        /// Only works if an intervall has been specified
        /// </summary>
        public void Stop()
        {
            Logger.Debug("Stopping slideshow");

            SlideShow.Stop();
        }

        /// <summary>
        /// Only works if an intervall has been specified
        /// </summary>
        public void Pause()
        {
            Logger.Debug("Pausing slideshow");

            SlideShow.Pause();
        }

        /// <summary>
        /// Only works if an intervall has been specified
        /// </summary>
        public void Resume()
        {
            Logger.Debug("Resuming slideshow");

            SlideShow.Resume();
        }

        /// <summary>
        /// Displays the specified image
        /// Pauses the slide show if an intervall has been specified
        /// </summary>
        /// <param name="imagePath"></param>
        public void Show(string imagePath)
        {
            Logger.Debug($"Showing image {imagePath}");

            SlideShow.Show(imagePath);
        }

        public void Show(Bitmap image)
        {
            Logger.Debug($"Showing image.");

            SlideShow.Show(image);
        }

        public void Show(RenderTargetBitmap image)
        {
            Logger.Debug($"Showing image.");

            SlideShow.Show(image);
        }


        /// <summary>
        /// Shows the next image
        /// </summary>
        public void Next()
        {
            Logger.Debug("Showing next image");

            SlideShow.Next();
        }
    }
}
