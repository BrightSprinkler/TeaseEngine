using System;
using System.Collections.Generic;
using System.Linq;
using TeaseEngine.Modules;
using TeaseEngine.Utils;

namespace TeaseEngine.Wrapper
{
    public class ModuleWrapper
    {
        private List<BaseModule> Modules { get; set; }
        private Logger Logger { get; } = App.Logging.GetLogger<ModuleWrapper>();

        public ModuleWrapper(List<BaseModule> modules)
        {
            Modules = modules;
        }

        public T GetModule<T>() where T : BaseModule
        {
            if (Modules is null || !Modules.Any()) return null;

            Logger.Debug($"Resolving module {typeof(T).Name}");

            return (T)Modules.FirstOrDefault(x => x is T);
        }

        public object GetModule(Type moduleType )
        {
            if (Modules is null || !Modules.Any()) return null;

            Logger.Debug($"Resolving module {moduleType.Name}");

            return Modules.FirstOrDefault(x => x.GetType().FullName == moduleType.FullName);
        }

    }
}
