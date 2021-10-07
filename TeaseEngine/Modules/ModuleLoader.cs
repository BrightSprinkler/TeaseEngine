using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using TeaseEngine.Controls;
using TeaseEngine.Utils;
using TeaseEngine.Wrapper;

namespace TeaseEngine.Modules
{
    internal class ModuleLoader
    {
        public List<BaseModule> Modules { get; private set; } = new List<BaseModule>();
        private PathManager PathManager { get; } = new PathManager();
        private readonly Logger Logger = App.Logging.GetLogger<ModuleLoader>();

        public void Load(MetronomeTimer metronomeTimer, ButtonGroup buttonGroup, MessageBox messageBox, UserInput userInput, SlideShow slideShow, StatusDisplay statusDisplay, VideoPlayer videoPlayer)
        {
            foreach (string dll in GetDlls())
            {
                try
                {
                    foreach (Type t in Assembly.LoadFrom(dll).GetTypes())
                    {
                        if (!typeof(BaseModule).IsAssignableFrom(t)) continue;
                        if (t.IsAbstract) continue;

                        Logger.Debug($"Loading module {t.Name} from {Assembly.LoadFrom(dll).FullName}");

                        Modules.Add((BaseModule)
                            Activator.CreateInstance(t, new WrapperWrapper(metronomeTimer, buttonGroup, messageBox, userInput, slideShow, statusDisplay, videoPlayer, Modules)));
                    }

                }
                catch (Exception ex) { Logger.Warn(ex); }

            }
        }

        private List<string> GetDlls() => Directory.GetFiles(PathManager.ModuleDirectory).Where(x => x.EndsWith(".dll")).ToList();
    }
}
