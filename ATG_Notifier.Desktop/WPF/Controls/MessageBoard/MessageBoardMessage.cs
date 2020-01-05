using System.Windows.Input;

namespace ATG_Notifier.Desktop.WPF.Controls
{
    internal class MessageBoardMessage
    {
        public MessageBoardMessage(int id, MessageCardType type, string text)
        {
            this.Id = id;
            this.Type = type;
            this.Text = text;
        }

        public MessageBoardMessage(int id, MessageCardType type, string text, string actionText, ICommand actionCommand)
        {
            this.Id = id;
            this.Type = type;
            this.Text = text;
            this.ActionText = actionText;
            this.ActionCommand = actionCommand;
        }

        public int Id { get; }

        public MessageCardType Type { get; }

        public string Text { get; }

        public string? ActionText { get; }

        public ICommand? ActionCommand { get; }

        public bool DismissOnActionExecution { get; set; }
    }
}
