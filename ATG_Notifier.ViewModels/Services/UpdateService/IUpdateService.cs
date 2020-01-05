using System;
using System.Collections.Generic;
using System.Text;

namespace ATG_Notifier.ViewModels.Services
{
    public interface IUpdateService
    {
        event EventHandler<ChapterUpdateEventArgs>? ChapterUpdated;

        event EventHandler Started;

        event EventHandler Stopped;

        bool IsRunning { get; }

        void Start();

        void Stop();
    }
}
