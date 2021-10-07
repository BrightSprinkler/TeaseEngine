using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using TeaseEngine.Modules;
using TeaseEngine.Utils;
using TeaseEngine.Windows;

namespace TeaseEngine
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ModuleLoader ModuleLoader { get; set; }
        private Logger Logger { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            Logger = App.Logging.GetLogger<MainWindow>();

            SlideShow.VideoPlayer = VideoPlayer;
            VideoPlayer.SlideShow = SlideShow;

            ModuleLoader = new ModuleLoader();
            MetronomeTimer.Stop();
            UserInput.Hide();
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (UserInput.Visibility == Visibility.Visible) return;

            ButtonGroup.KeyPressed(e.Key);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Logger.Info("Application starting");

            ModuleLoader.Load(MetronomeTimer, ButtonGroup, MessageBox, UserInput, SlideShow, StatusDisplay, VideoPlayer);

            ShowSettings();
        }

        private void ShowSettings()
        {
            SettingsWindow window = new SettingsWindow(ModuleLoader.Modules);

            bool? result = window.ShowDialog();

            if (result != null && (bool)result)
            {
                ModuleLoader.Modules.First(x => x.Name == window.StartingModuleName).Start();

                SlideShow.Clear();
                SlideShow.Stop();
                VideoPlayer.Stop();
                MessageBox.Clear();
                ButtonGroup.Clear();
                StatusDisplay.Clear();
                MetronomeTimer.Stop();
                UserInput.Hide();
            }
            else
            {
                Application.Current.Shutdown();
                return;
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Logger.Info("Application closing");

            ModuleLoader.Modules.ForEach(x => x.Dispose());
            ModuleLoader = null;
            MetronomeTimer = null;
            SlideShow = null;
            ButtonGroup = null;
            UserInput = null;

            App.Logging.Stop();
            Environment.Exit(0); // This stops all running threads
        }

    }
}
