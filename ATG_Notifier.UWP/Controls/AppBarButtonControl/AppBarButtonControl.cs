using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

// The Templated Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234235

namespace ATG_Notifier.UWP.Controls
{
    public sealed class AppBarButtonControl : AppBarButton
    {
        #region LabelPosition

        public static readonly DependencyProperty LabelPosition2Property =
            DependencyProperty.Register(nameof(LabelPosition2), typeof(LabelPosition), typeof(AppBarButtonControl),
                new PropertyMetadata(Controls.LabelPosition.Right, OnPropertyChanged));

        public LabelPosition LabelPosition2
        {
            get => (LabelPosition)GetValue(LabelPosition2Property);
            set => SetValue(LabelPosition2Property, value);
        }

        #endregion // LabelPosition

        public AppBarButtonControl()
        {
            this.DefaultStyleKey = typeof(AppBarButtonControl);
        }

        protected override void OnApplyTemplate()
        {
            UpdateLabelPosition();
        }

        private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (AppBarButtonControl)d;

            if (e.Property == LabelPosition2Property)
            {
                control.UpdateLabelPosition();
            }
        }

        private void UpdateLabelPosition()
        {
            if (this.LabelPosition2 == Controls.LabelPosition.Right)
            {
                this.Width = double.NaN;
                VisualStateManager.GoToState(this, "LabelOnRight", false);
            }
            else if (this.LabelPosition2 == Controls.LabelPosition.Below)
            {
                this.Width = 64;
                VisualStateManager.GoToState(this, "LabelBelow", false);
            }
            else // labelPosition == LabelPosition.Collapsed
            {
                this.Width = 42;
                VisualStateManager.GoToState(this, "LabelCollapsed", false);
            }
        }
    }
}
