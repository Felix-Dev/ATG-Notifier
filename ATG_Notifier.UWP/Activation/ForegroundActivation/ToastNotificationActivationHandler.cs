using ATG_Notifier.UWP.Configuration;
using ATG_Notifier.UWP.Helpers;
using ATG_Notifier.UWP.Services.Activation;
using ATG_Notifier.UWP.Services.Navigation;
using ATG_Notifier.UWP.ViewModels;
using ATG_Notifier.UWP.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

namespace ATG_Notifier.UWP.Activation
{
    public class ToastNotificationActivationHandler : ActivationHandler<ToastNotificationActivatedEventArgs>
    {
        private readonly Type navigationTarget;
        private readonly object argument;

        protected override bool CanHandleInternal(ToastNotificationActivatedEventArgs args)
        {
            return true;
        }

        protected override async Task HandleInternalAsync(ToastNotificationActivatedEventArgs args)
        {
            // TODO: Content is too UI structure dependent. Move parts of it to AppShell (such as navigation clea history)
            // and to the navigation service

            if (Window.Current.Content is Frame frame)
            {
                long.TryParse(args.Argument, out long chapterId);

                if (frame.Content is AppShell appShell)
                {
                    var navigationService = Singleton<NavigationService>.Instance;
                    if (navigationService.CurrentPage == typeof(ChaptersView))
                    {
                        if (appShell.CurrentPage is ChaptersView chaptersView)
                        {
                            chaptersView.SetFocusOnChapterProfile(chapterId);
                        }
                    }
                    else
                    {
                        navigationService.Navigate(typeof(ChaptersView),
                                                   new ChaptersViewArguments { SelectedChapterId = chapterId, SetFocusOnSelectedChapter = true });

                        navigationService.ResetNavigationHistory();
                    }
                }
                else
                {
                    var shellArgs = new ShellArguments(typeof(ChaptersView),
                                                      new ChaptersViewArguments { SelectedChapterId = chapterId, SetFocusOnSelectedChapter = true });
                    frame.Navigate(typeof(AppShell), shellArgs);
                }
            }

            await Task.CompletedTask;
        }
    }
}
