using System;
using System.Collections.Generic;
using System.Text;

namespace ATG_Notifier.ViewModels.Services
{
    public interface IUpdateService
    {
        event EventHandler<ChapterUpdateEventArgs>? ChapterUpdated;

        bool IsRunning { get; }

        void Start();

        void Stop();
    }
}
