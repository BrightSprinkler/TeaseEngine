using TeaseEngine.Controls;
using TeaseEngine.Utils;

namespace TeaseEngine.Wrapper
{
    public class StatusDisplayWrapper
    {
        private StatusDisplay StatusDisplay { get; set; }
        private Logger Logger { get; } = App.Logging.GetLogger<StatusDisplay>();
        
        public StatusDisplayWrapper(StatusDisplay statusDisplay)
        {
            StatusDisplay = statusDisplay;
        }

        public void Add(string name, string text)
        {
            Logger.Debug($"Adding status {name} | {text}");

            StatusDisplay.Add(name, text);
        }

        public void Remove(string name)
        {
            Logger.Debug($"Removing status {name}");

            StatusDisplay.Remove(name);
        }

        public void Clear()
        {
            Logger.Debug("Clearing stati");

            StatusDisplay.Clear();
        }

        public bool HasStatus(string name)
        {
            return StatusDisplay.HasStatus(name);
        }

        public void Update(string name, string text)
        {
            StatusDisplay.Update(name, text);
        }

    }
}
