using System.Windows.Media;
using System.Windows.Threading;
using TeaseEngine.Controls;
using TeaseEngine.Utils;

namespace TeaseEngine.Wrapper
{
    public class MessageBoxWrapper
    {
        private MessageBox MessageBox { get; set; }
        private Logger Logger { get; } = App.Logging.GetLogger<MessageBoxWrapper>();

        public MessageBoxWrapper(MessageBox messageBox)
        {
            MessageBox = messageBox;
        }

        public void Add(string text, bool waitAfter = true)
        {
            Logger.Debug($"{text} | {waitAfter}");

            MessageBox.Add(text, waitAfter);
        }

        public void Add(string text, string hexColor, bool waitAfter = false)
        {
            Logger.Debug($"{text} | {waitAfter} | {hexColor}");

            MessageBox.Add(text, (Color)ColorConverter.ConvertFromString(hexColor), waitAfter);
        }

        public void Clear()
        {
            Logger.Debug("Clearing messages");

            MessageBox.Clear();
        }

    }
}
