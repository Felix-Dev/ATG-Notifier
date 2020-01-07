using ATG_Notifier.ViewModels.Helpers.Extensions;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

namespace ATG_Notifier.Desktop.WPF.Controls
{
    [TemplatePart(Name = CloseButtonName, Type = typeof(Button))]
    [TemplatePart(Name = ActionHyperlinkName, Type = typeof(Hyperlink))]
    internal class MessageCard : Control
    {
        private const string CloseButtonName = "PART_CloseButton";
        private const string ActionHyperlinkName = "PART_MessageActionHyperlink";

        private Button closeButton = null!;
        private Hyperlink? actionHyperlink;

        private bool isInExpandedState;
        private bool isInWrappedState;

        static MessageCard()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MessageCard), new FrameworkPropertyMetadata(typeof(MessageCard)));
        }

        public MessageCard()
        {
            SizeChanged += OnSizeChanged;
        }

        #region TextProperty

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(nameof(Text), typeof(string), typeof(MessageCard), new PropertyMetadata(null));

        #endregion // TextProperty

        #region TypeProperty

        public MessageCardType Type
        {
            get => (MessageCardType)GetValue(TypeProperty);
            set => SetValue(TypeProperty, value);
        }

        public static readonly DependencyProperty TypeProperty =
            DependencyProperty.Register(nameof(Type), typeof(MessageCardType), typeof(MessageCard));

        #endregion // TypeProperty

        #region ActionTextProperty

        public string? ActionText
        {
            get => (string)GetValue(ActionTextProperty);
            set => SetValue(ActionTextProperty, value);
        }

        public static readonly DependencyProperty ActionTextProperty =
            DependencyProperty.Register(nameof(ActionText), typeof(string), typeof(MessageCard), new PropertyMetadata(null));

        #endregion // ActionTextProperty

        #region ActionCommandProperty

        public ICommand? ActionCommand
        {
            get => (ICommand)GetValue(ActionCommandProperty);
            set => SetValue(ActionCommandProperty, value);
        }

        public static readonly DependencyProperty ActionCommandProperty =
            DependencyProperty.Register(nameof(ActionCommand), typeof(ICommand), typeof(MessageCard), new PropertyMetadata(null));

        #endregion // ActionCommandProperty

        #region CloseCommandProperty

        public ICommand? CloseCommand
        {
            get => (ICommand)GetValue(CloseCommandProperty);
            set => SetValue(CloseCommandProperty, value);
        }

        public static readonly DependencyProperty CloseCommandProperty =
            DependencyProperty.Register(nameof(CloseCommand), typeof(ICommand), typeof(MessageCard), new PropertyMetadata(null));

        #endregion // CloseCommandProperty

        #region DismissOnActionClickProperty

        public bool DismissOnActionClick
        {
            get => (bool)GetValue(DismissOnActionClickProperty);
            set => SetValue(DismissOnActionClickProperty, value);
        }

        public static readonly DependencyProperty DismissOnActionClickProperty =
            DependencyProperty.Register(nameof(DismissOnActionClick), typeof(bool), typeof(MessageCard), new PropertyMetadata(false));

        #endregion // DismissOnActionClickProperty

        public event EventHandler? Closed;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.closeButton = (Button)Template.FindName(CloseButtonName, this)
                ?? throw new InvalidOperationException();

            this.closeButton.Click += OnCloseButtonClick;

            this.actionHyperlink = Template.FindName(ActionHyperlinkName, this) as Hyperlink;
            if (this.actionHyperlink != null)
            {
                this.actionHyperlink.Click += OnActionHyperlinkClick;
            }
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (e.NewSize.Width <= 450 && !this.isInWrappedState)
            {
                VisualStateManager.GoToState(this, "StateWrapped", false);

                this.isInWrappedState = true;
                this.isInExpandedState = false;
            }
            else if (e.NewSize.Width > 450 && !this.isInExpandedState)
            {
                VisualStateManager.GoToState(this, "StateExpanded", false);

                this.isInWrappedState = false;
                this.isInExpandedState = true;
            }
        }

        private void OnActionHyperlinkClick(object sender, RoutedEventArgs e)
        {
            this.ActionCommand?.TryExecute();

            if (this.DismissOnActionClick)
            {
                Close();
            }
        }

        private void OnCloseButtonClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Close()
        {
            this.Visibility = Visibility.Collapsed;

            Closed?.Invoke(this, EventArgs.Empty);
        }  
    }
}
