using System;

namespace TeaseEngine.Models
{
    public enum MessageType
    {
        Trace,
        Debug,
        Info,
        Warning,
        Error
    }

    public class LogMessage
    {
        public MessageType Type { get; set; }
        public string Text { get; set; }
        public DateTime TimeStamp { get; set; }
        public Type Sender { get; set; }

        public LogMessage(MessageType type, string text, Type sender)
        {
            Type = type;
            Text = text;
            Sender = sender;
            TimeStamp = DateTime.Now;
        }

        public override string ToString() => $"{TimeStamp:s} | {Type.ToString().PadLeft(MessageType.Warning.ToString().Length)} | {Sender.Name.PadLeft(30)} | {Text} {Environment.NewLine}";
    }
}
