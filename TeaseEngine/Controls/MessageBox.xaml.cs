using System;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace TeaseEngine.Controls
{
    /// <summary>
    /// Interaction logic for MessageBox.xaml
    /// </summary>
    public partial class MessageBox : UserControl
    {
        private Timer FadeOutTimer { get; set; }

        public MessageBox()
        {
            InitializeComponent();
            FadeOutTimer = new Timer(5000)
            {
                AutoReset = false,
                Enabled = false
            };
            FadeOutTimer.Elapsed += (sender, e) =>
            {
                Dispatcher.Invoke(() =>
                {
                    Opacity = 0.1;
                });
            };
        }

        public void Add(string text, bool waitAfter = true)
        {
            Add(text, Colors.White, waitAfter);
        }

        public void Add(string text, Color color, bool waitAfter = true)
        {
            FadeOutTimer.Stop();
            Opacity = 1;

            StackPanel.Children.Insert(0, new Label()
            {
                VerticalContentAlignment = VerticalAlignment.Center,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                Padding = new Thickness(1),
                Margin = new Thickness(4),
                Content = new TextBlock()
                {
                    Foreground = new SolidColorBrush(color),
                    Background = new SolidColorBrush(Colors.Black),
                    Opacity = 0.9,
                    Text = text,
                    FontSize = 22,
                    Padding = new Thickness(3),
                    TextWrapping = TextWrapping.Wrap
                }
            });

            ScrollViewer.ScrollToTop();

            // https://www.funtrivia.com/askft/Question145725.html
            if (!waitAfter) return;

            DateTime now = DateTime.Now;
            while (now.AddMilliseconds(((text.Length / 25) * 1250) + 2000) > DateTime.Now) DoEvents();
            FadeOutTimer.Start();
        }

        public void Clear()
        {
            StackPanel.Children.Clear();
        }

        protected void DoEvents()
        {
            try
            {
                Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate { }));
            }
            catch
            {
            }

        }

        private void UserControl_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            FadeOutTimer.Stop();
            Opacity = 1;
            FadeOutTimer.Start();
        }
    }
}
