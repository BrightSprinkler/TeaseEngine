using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace TeaseEngine.Controls
{
    /// <summary>
    /// Interaction logic for StatusDisplay.xaml
    /// </summary>
    public partial class StatusDisplay : UserControl
    {
        public StatusDisplay()
        {
            InitializeComponent();
        }

        public void Add(string name, string text)
        {
            TextBlock textBlock = new()
            {
                Name = name,
                Text = text,
                Focusable = false,
                Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#C9565D")),
                Foreground = new SolidColorBrush(Colors.White),
                Margin = new Thickness(5, 5, 0, 0),
                TextAlignment = TextAlignment.Center,
                FontSize = 22,
                Width = 290 // funny numbers here ;)
            };

            WrapPanel.Children.Add(textBlock);
        }

        public void Remove(string name)
        {
            TextBlock textBlock = null;

            for (int i = 0; i < WrapPanel.Children.Count; i++)
            {
                if (((TextBlock)WrapPanel.Children[i]).Name == name)
                {
                    textBlock = WrapPanel.Children[i] as TextBlock;
                    break;
                }
            }

            if (textBlock is null) return;

            WrapPanel.Children.Remove(textBlock);
        }

        public void Clear()
        {
            WrapPanel.Children.Clear();
        }

        public bool HasStatus(string name)
        {
            foreach (TextBlock control in WrapPanel.Children)
                if (control.Name == name) return true;

            return false;
        }

        public void Update(string name, string text)
        {
            foreach (TextBlock control in WrapPanel.Children)
                if (control.Name == name)
                {
                    control.Text = text;
                    return;
                }
        }

    }
}
