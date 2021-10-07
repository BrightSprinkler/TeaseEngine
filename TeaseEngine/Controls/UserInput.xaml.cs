using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace TeaseEngine.Controls
{
    /// <summary>
    /// Interaction logic for UserInput.xaml
    /// </summary>
    public partial class UserInput : UserControl
    {
        private bool UserHasEnteredValue { get; set; }

        public UserInput()
        {
            InitializeComponent();
        }

        internal void Hide()
        {
            Visibility = Visibility.Hidden;
            InputTextBox.IsEnabled = false;
        }

        internal void Show()
        {
            Visibility = Visibility.Visible;
            UserHasEnteredValue = false;
            InputTextBox.IsEnabled = true;
            InputTextBox.Text = "";
            InputTextBox.Focus();
        }

        public T GetInput<T>(Action<string> onWrongInput = null, Func<T, bool> validator = null)
        {
            Show();

            T output;

            do
            {
                while (!UserHasEnteredValue) { DoEvents(); Thread.Sleep(10); }

                try
                {
                    output = (T)Convert.ChangeType(InputTextBox.Text, typeof(T));
                    if (validator != null && !validator(output))
                    {
                        InputTextBox.Text = "";
                        InputTextBox.IsEnabled = true;
                        UserHasEnteredValue = false;
                        InputTextBox.Focus();
                        continue;
                    }

                    break;
                }
                catch
                {
                    onWrongInput?.Invoke(InputTextBox.Text);
                    InputTextBox.Text = "";
                    InputTextBox.IsEnabled = true;
                    UserHasEnteredValue = false;
                    InputTextBox.Focus();
                }

            } while (true);

            Hide();

            return output;
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

        private void InpuTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter) return;

            UserHasEnteredValue = true;

            Dispatcher.Invoke(() =>
            {
                InputTextBox.IsEnabled = false;
            });

        }
    }
}
