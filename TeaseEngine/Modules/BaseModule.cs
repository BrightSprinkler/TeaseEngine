using System;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using TeaseEngine.Utils;
using TeaseEngine.Wrapper;

namespace TeaseEngine.Modules
{
    public abstract class BaseModule : IDisposable
    {
        public abstract string Name { get; }
        public virtual string Description { get; }
        /// <summary>
        /// If true the module can be selected as starting module
        /// </summary>
        public bool IsMain { get; protected set; }

        protected PathManager PathManager { get; }
        protected StorageService Storage { get; }
        protected SoundService Sound { get; }

        protected ButtonGroupWrapper Buttons { get; }
        protected MessageBoxWrapper Messages { get; }
        protected MetronomeTimerWrapper Metronome { get; }
        protected UserInputWrapper UserInput { get; }
        protected StopwatchWrapper Stopwatch { get; }
        protected SlideShowWrapper SlideShow { get; }
        protected StatusDisplayWrapper StatusDisplay { get; }
        protected VideoPlayerWrapper VideoPlayer{ get; }

        protected ModuleWrapper Modules { get; }
        protected Logger Logger { get; }

        private bool IsWaiting { get; set; }

        public BaseModule(WrapperWrapper wrapperWrapper)
        {
            Buttons = wrapperWrapper.Buttons;
            Messages = wrapperWrapper.Messages;
            Metronome = wrapperWrapper.Metronome;
            Modules = wrapperWrapper.Modules;
            UserInput = wrapperWrapper.UserInput;
            SlideShow = wrapperWrapper.SlideShow;
            StatusDisplay = wrapperWrapper.StatusDisplay;
            VideoPlayer = wrapperWrapper.VideoPlayer;
            PathManager = new PathManager();
            Storage = new StorageService();
            Sound = new SoundService();
            Stopwatch = new StopwatchWrapper();
            Logger = App.Logging.GetLogger(GetType());
        }

        public abstract void Start();

        protected void Sleep(int milliseconds)
        {
            if (milliseconds > 500) Logger.Debug($"Sleeping for {milliseconds} milliseconds");

            Thread.Sleep(milliseconds);
        }

        protected void Wait()
        {
            Logger.Info($"Started waiting");

            IsWaiting = true;
            while (IsWaiting) { DoEvents(); Sleep(100); }
        }

        protected void Continue()
        {
            Logger.Info($"Stopped waiting");

            IsWaiting = false;
        }

        protected void WaitForMetronome()
        {
            Logger.Info($"Waiting for Metronome");

            while (Metronome.State != Controls.ProgressState.Finished) { DoEvents(); Sleep(100); }
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

        protected void WaitForUserConfirmation(string message = "Continue if you are ready.", string hotkey = "Space", string hexColor = null)
        {
            Logger.Info($"Waiting for user confirmation");

            if (hexColor is null)
                Messages.Add(message);
            else
                Messages.Add(message, hexColor);

            Buttons.Add("TEASE_ENGINEBaseModuleUserConfirmationButton", "Continue", () => { Buttons.Remove("TEASE_ENGINEBaseModuleUserConfirmationButton"); Continue(); }, hotkey);
            Wait();
        }

        protected bool GetYesOrNo(string message, string hotkeyYes = "Y", string hotkeyNo = "N", string hexColor = null)
        {
            bool result = false;

            if (hexColor is null)
                Messages.Add(message);
            else
                Messages.Add(message, hexColor);

            Buttons.Add("Yes", "Yes", () => { result = true; Continue(); }, hotkeyYes);
            Buttons.Add("No", "No", () => { result = false; Continue(); }, hotkeyNo);
            Wait();

            Buttons.Remove("Yes");
            Buttons.Remove("No");

            return result;
        }


        public void Dispose()
        {
            Logger.Debug($"Disposing module {GetType().Name}");

            Continue();
            Metronome.Stop();
            SlideShow.Stop();
            Buttons.Clear();
            Messages.Clear();
            Stopwatch.Stop();
            StatusDisplay.Clear();
        }
    }
}
