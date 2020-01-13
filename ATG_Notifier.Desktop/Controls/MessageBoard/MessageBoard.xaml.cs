using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace ATG_Notifier.Desktop.Controls
{
    internal partial class MessageBoard : UserControl
    {
        public MessageBoard()
        {
            this.DataContext = this;

            InitializeComponent();
        }

        #region MessagesProperty

        public IList<MessageBoardMessage> Messages
        {
            get => (IList<MessageBoardMessage>)GetValue(MessagesProperty);
            set => SetValue(MessagesProperty, value);
        }

        public static readonly DependencyProperty MessagesProperty =
            DependencyProperty.Register(nameof(Messages), typeof(IList<MessageBoardMessage>), typeof(MessageBoard), new PropertyMetadata(null));

        #endregion // MessagesProperty

        private void OnMessageCardClosed(object sender, EventArgs e)
        {
            this.Messages.Remove((MessageBoardMessage)((MessageCard)sender).DataContext);
        }
    }
}
