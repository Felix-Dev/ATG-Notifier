using System;
using System.Collections.Generic;
using System.Text;

namespace ATG_Notifier.ViewModels.Services
{
    public interface INavigationService
    {
        Type? CurrentPage { get; }

        bool CanGoBack { get; }

        void Initialize(object frame);

        bool Navigate<TViewModel>(object? parameter = null);

        bool Navigate(Type viewModelType, object? parameter = null);

        void GoBack();
    }
}
