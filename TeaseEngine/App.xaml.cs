using System;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using TeaseEngine.Utils;
using TeaseEngine.Windows;

namespace TeaseEngine
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private Mutex mutex;
        private bool handleExceptions = false;

        internal static LogQueue Logging { get; set; } = new LogQueue();
        private readonly Logger Logger = Logging.GetLogger<App>();

        private void Application_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = handleExceptions;
            Logger.Error(e.Exception);
            ErrorWindow.Show(e.Exception);
        }

        protected override void OnStartup(StartupEventArgs e)
        {

            try
            {
                Logging.Start();

                CheckAlreadyRunning();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                ErrorWindow.Show(ex);
                Application.Current.Shutdown();
            }
            finally
            {
                handleExceptions = true;
            }

        }

        private void CheckAlreadyRunning()
        {
            mutex = new Mutex(false, "TeaseEngine - 133742069");

            if (!mutex.WaitOne(0, false))
                throw new Exception("The application is already running.");
        }

        protected override void OnExit(ExitEventArgs e)
        {
            mutex?.Close();
            mutex?.Dispose();
            mutex = null;

            base.OnExit(e);
        }
    }
}
