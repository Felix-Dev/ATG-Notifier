using System;
using System.Collections.Generic;
using System.Text;

namespace ATG_Notifier.Desktop.Services
{
    internal class NavigationEventArgs2 : EventArgs
    {
        public NavigationEventArgs2(Uri uri, object content, object parameter, Type sourcePageType)
        {
            this.Uri = uri;
            this.Content = content;
            this.Parameter = parameter;
            this.SourcePageType = sourcePageType;
        }

        //
        // Summary:
        //     Gets the Uniform Resource Identifier (URI) of the target.
        //
        // Returns:
        //     A value that represents the Uniform Resource Identifier (URI).
        public Uri Uri { get; }

        //
        // Summary:
        //     Gets the root node of the target page's content.
        //
        // Returns:
        //     The root node of the target page's content.
        public object Content { get; }

        //
        // Summary:
        //     Gets any "Parameter" object passed to the target page for the navigation.
        //
        // Returns:
        //     An object that potentially passes parameters to the navigation target. May be
        //     null.
        public object Parameter { get; }

        //
        // Summary:
        //     Gets the data type of the source page.
        //
        // Returns:
        //     The data type of the source page, represented as *namespace*.*type* or simply
        //     *type*.
        public Type SourcePageType { get; }
    }
}
