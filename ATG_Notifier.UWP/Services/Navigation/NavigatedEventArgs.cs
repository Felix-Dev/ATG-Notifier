using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATG_Notifier.UWP.Services.Navigation
{
    public class NavigatedEventArgs : EventArgs
    {
        public NavigatedEventArgs(Type sourcePage, Type destinationPage)
        {
            this.SourcePage = sourcePage;
            this.DestinationPage = destinationPage ?? throw new ArgumentNullException(nameof(destinationPage));
        }

        public Type SourcePage { get; }

        public Type DestinationPage { get; }
    }
}
