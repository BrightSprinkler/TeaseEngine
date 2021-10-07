using System.Collections.Generic;
using TeaseEngine.Controls;
using TeaseEngine.Modules;

namespace TeaseEngine.Wrapper
{
    public class WrapperWrapper
    {
        public ButtonGroupWrapper Buttons { get; }
        public MessageBoxWrapper Messages { get; }
        public MetronomeTimerWrapper Metronome { get; }
        public UserInputWrapper UserInput { get; }
        public SlideShowWrapper SlideShow { get; }
        public StatusDisplayWrapper StatusDisplay { get; }
        public VideoPlayerWrapper VideoPlayer { get; }
        public ModuleWrapper Modules { get; }

        public WrapperWrapper(MetronomeTimer metronomeTimer, ButtonGroup buttonGroup, MessageBox messageBox, UserInput userInput, SlideShow slideShow, StatusDisplay statusDisplay, VideoPlayer videoPlayer, List<BaseModule> modules)
        {
            Buttons = new ButtonGroupWrapper(buttonGroup);
            Messages = new MessageBoxWrapper(messageBox);
            Metronome = new MetronomeTimerWrapper(metronomeTimer);
            Modules = new ModuleWrapper(modules);
            UserInput = new UserInputWrapper(userInput);
            SlideShow = new SlideShowWrapper(slideShow);
            StatusDisplay = new StatusDisplayWrapper(statusDisplay);
            VideoPlayer = new VideoPlayerWrapper(videoPlayer);
        }
    }
}
