using System;
using System.Windows;
using System.Windows.Input;
using TeaseEngine.Controls;
using TeaseEngine.Utils;

namespace TeaseEngine.Wrapper
{
    public class ButtonGroupWrapper
    {
        private ButtonGroup ButtonGroup { get; set; }
        private Logger Logger { get; } = App.Logging.GetLogger<ButtonGroupWrapper>();

        public ButtonGroupWrapper(ButtonGroup buttonGroup)
        {
            ButtonGroup = buttonGroup;
        }

        public void Add(string name, string text, Action onClick, string hotKey = null)
        {
            Logger.Debug($"Adding button {name} | {text} | hotkey: {hotKey}");

            if (Enum.TryParse(hotKey, out Key key))
                ButtonGroup.Add(name, text, onClick, key);
            else
                ButtonGroup.Add(name, text, onClick, null);
        }

        public void Remove(string name)
        {
            Logger.Debug($"Removing button {name}");

            ButtonGroup.Remove(name);
        }

        public void Clear()
        {
            Logger.Debug("Clearing buttons");

            ButtonGroup.Clear();
        }

        public bool Contains(string name)
        {
            return ButtonGroup.Contains(name);
        }

        public bool IsHotKeyUsed(string hotKey)
        {
            return ButtonGroup.IsHotKeyUsed(hotKey);
        }
    }
}
