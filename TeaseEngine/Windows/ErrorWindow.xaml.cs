using System;
using System.Windows;

namespace TeaseEngine.Windows
{
    /// <summary>
    /// Interaction logic for ErrorWindow.xaml
    /// </summary>
    public partial class ErrorWindow : Window
    {
        /// <summary>
        /// Shows the error window as dialog
        /// </summary>
        /// <param name="ex"></param>
        public static void Show(Exception ex) => new ErrorWindow(ex).ShowDialog();

        public ErrorWindow()
        {
            InitializeComponent();
        }

        public ErrorWindow(Exception ex) : this()
        {
            TextBlockError.Text = ex.Message;
            TextBlockStackTrace.Text = ex.StackTrace;
        }

    }
}