using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Timers;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using TeaseEngine.Utils;

namespace TeaseEngine.Controls
{
    /// <summary>
    /// Interaction logic for SlideShow.xaml
    /// </summary>
    public partial class SlideShow : UserControl
    {
        public int IntervalInMilliseconds { get; private set; }
        public bool Randomized { get; private set; }
        public bool IsIntervalSet => IntervalInMilliseconds > 0;
        public string RootDirectory { get; private set; }

        private Timer Timer { get; set; }
        private int CurrentSubDirectoryIndex { get; set; }
        private int CurrentImageIndex { get; set; }
        private List<string> SubDirectories { get; set; }
        private List<string> Images { get; set; }

        internal VideoPlayer VideoPlayer { get; set; }

        private Logger Logger { get; set; } = App.Logging.GetLogger<SlideShow>();

        public SlideShow()
        {
            InitializeComponent();

            Timer = new Timer();
            Timer.Elapsed += TimerElapsed;
            Timer.Enabled = false;

            Clear();
        }

        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                Next();
            });
        }

        public void Start(string directoryPath, int intervalInMilliseconds = -1, bool randomized = true)
        {
            Stop();
            VideoPlayer.Stop();

            RootDirectory = directoryPath;
            IntervalInMilliseconds = intervalInMilliseconds;
            Randomized = randomized;

            CurrentSubDirectoryIndex = 0;
            CurrentImageIndex = 0;

            SubDirectories = null;
            Images = null;

            if (IsIntervalSet) Timer.Interval = intervalInMilliseconds;

            Next();

            if (IsIntervalSet) Timer.Enabled = true;
        }

        /// <summary>
        /// Removes the current image
        /// </summary>
        public void Clear()
        {
            SlideShowImage.Source = null;
        }

        /// <summary>
        /// Only works if an intervall has been specified
        /// </summary>
        public void Stop()
        {
            if (!IsIntervalSet) return;
            Timer.Enabled = false;
            Clear();
        }

        /// <summary>
        /// Only works if an intervall has been specified
        /// </summary>
        public void Pause()
        {
            if (!IsIntervalSet) return;
            Timer.Enabled = false;
        }

        /// <summary>
        /// Only works if an intervall has been specified
        /// </summary>
        public void Resume()
        {
            if (!IsIntervalSet) return;
            
            VideoPlayer.Stop();
            Timer.Enabled = true;
        }

        /// <summary>
        /// Displays the specified image
        /// Pauses the slide show if an intervall has been specified
        /// </summary>
        /// <param name="imagePath"></param>
        public void Show(string imagePath)
        {
            Pause();
            ShowInternal(imagePath);
        }

        public void Show(Bitmap image)
        {
            Pause();
            ShowInternal(image);
        }

        public void Show(RenderTargetBitmap image)
        {
            Pause();
            ShowInternal(image);
        }

        /// <summary>
        /// Shows the next image
        /// </summary>
        public void Next()
        {
            string directory;
            Random random = new();

            if (Images is null)
            {
                if (SubDirectories is null)
                    SubDirectories = System.IO.Directory.GetDirectories(RootDirectory).ToList();

                if (!SubDirectories.Any())
                    directory = RootDirectory;
                else
                {
                    if (Randomized)
                        CurrentSubDirectoryIndex = random.Next(0, SubDirectories.Count);

                    List<string> subSubDirectories = System.IO.Directory.GetDirectories(SubDirectories[CurrentSubDirectoryIndex]).ToList();
                    if (subSubDirectories.Any())
                        SubDirectories = subSubDirectories;

                    if (Randomized)
                        CurrentSubDirectoryIndex = random.Next(0, SubDirectories.Count);

                    directory = SubDirectories[CurrentSubDirectoryIndex];
                }

                Logger.Trace($"Getting images from {directory}");

                Images = System.IO.Directory.GetFiles(directory).ToList();
            }

            if (Images.Count == 0) return;

            if (Randomized)
            {
                ShowInternal(Images[random.Next(0, Images.Count)]);
            }
            else
            {
                ShowInternal(Images[CurrentImageIndex]);
            }

            CurrentImageIndex++;

            if (CurrentImageIndex >= Images.Count)
            {
                Images = null;
                CurrentSubDirectoryIndex++;
                if (CurrentSubDirectoryIndex >= SubDirectories.Count) CurrentSubDirectoryIndex = 0;
                CurrentImageIndex = 0;
            }
        }

        private void ShowInternal(string imagePath)
        {
            if (Logger != null) Logger.Trace($"Showing image {imagePath}");

            try
            {
                SlideShowImage.Source = new BitmapImage(new Uri(imagePath, UriKind.RelativeOrAbsolute));
            }
            catch (Exception ex)
            {
                if (Logger != null) Logger.Warn(ex);
            }

        }

        private void ShowInternal(Bitmap image)
        {
            if (Logger != null) Logger.Trace($"Showing image");

            try
            {
                SlideShowImage.Source = new Converter().ConvertToBitmapSource(image);
            }
            catch (Exception ex)
            {
                if (Logger != null) Logger.Warn(ex);
            }

        }

        private void ShowInternal(RenderTargetBitmap image)
        {
            if (Logger != null) Logger.Trace($"Showing image");

            try
            {
                SlideShowImage.Source = new Converter().RenderTargetBitmapToBitmapSource(image);
            }
            catch (Exception ex)
            {
                if (Logger != null) Logger.Warn(ex);
            }

        }
    }
}
