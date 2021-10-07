using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using TeaseEngine.Utils;

namespace TeaseEngine.Controls
{
    /// <summary>
    /// Interaction logic for ButtonGroup.xaml
    /// </summary>
    public partial class ButtonGroup : UserControl
    {
        private Logger Logger { get; set; }

        public ButtonGroup()
        {
            InitializeComponent();
        }

        public void KeyPressed(Key key)
        {
            for (int i = 0; i < WrapPanel.Children.Count; i++)
            {
                Button button = ((Button)WrapPanel.Children[i]);
                if (button.Tag != null && (Key)button.Tag == key)
                {
                    button.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
                    break;
                }
            }
        }

        public void Add(string name, string text, Action onClick, Key? hotKey = null)
        {
            if (Logger is null) Logger = App.Logging.GetLogger<ButtonGroup>();

            if (hotKey != null)
                for (int i = 0; i < WrapPanel.Children.Count; i++)
                {
                    if (((Button)WrapPanel.Children[i]).Tag != null && (Key)((Button)WrapPanel.Children[i]).Tag == hotKey)
                        throw new ArgumentException($"The hot key {hotKey} is already taken.");
                }

            Button button = new Button()
            {
                Name = name,
                Content = hotKey != null ? $"{text} [{hotKey}]" : text,
                Tag = hotKey,
                Style = (Style)FindResource("NewGameButtonStyle"),
                IsTabStop = false,
                Focusable = false,
                Width = 290, // funny numbers here ;)
            };

            button.Click += (sender, e) =>
            {
                if (!button.IsEnabled) return;
                Logger.Debug($"Button {((Button)sender).Name} clicked");

                button.IsEnabled = false;
                onClick.Invoke();
                button.IsEnabled = true;
            };

            WrapPanel.Children.Add(button);
        }

        public void Remove(string name)
        {
            Button button = null;

            for (int i = 0; i < WrapPanel.Children.Count; i++)
                if (((Button)WrapPanel.Children[i]).Name == name)
                {
                    button = WrapPanel.Children[i] as Button;
                    break;
                }

            if (button is null) return;

            WrapPanel.Children.Remove(button);
        }

        public void Clear()
        {
            WrapPanel.Children.Clear();
        }

        public bool Contains(string name)
        {
            for (int i = 0; i < WrapPanel.Children.Count; i++)
                if (((Button)WrapPanel.Children[i]).Name == name)
                    return true;

            return false;
        }

        public bool IsHotKeyUsed(string hotKey)
        {
            for (int i = 0; i < WrapPanel.Children.Count; i++)
                if (((Button)WrapPanel.Children[i]).Tag.ToString() == hotKey)
                    return true;

            return false;
        }

    }
}
