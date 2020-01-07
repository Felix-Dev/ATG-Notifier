using System;
using System.Windows;
using System.Windows.Media;

namespace ATG_Notifier.Desktop.Helpers
{
    internal static class VisualTreeHelperEx
    {
        public static Visual? FindDescendantByType(Visual element, Type type)
        {
            return VisualTreeHelperEx.FindDescendantByType(element, type, true);
        }

        public static Visual? FindDescendantByType(Visual element, Type type, bool specificTypeOnly)
        {
            if (specificTypeOnly 
                ? (element.GetType() == type)
                : (element.GetType() == type) || element.GetType().IsSubclassOf(type))
                return element;

            Visual? foundElement = null;
            if (element is FrameworkElement frameworkElement)
            {
                frameworkElement.ApplyTemplate();
            }

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(element); i++)
            {
                if (!(VisualTreeHelper.GetChild(element, i) is Visual visual))
                {
                    continue;
                }

                foundElement = VisualTreeHelperEx.FindDescendantByType(visual, type, specificTypeOnly);
                if (foundElement != null)
                {
                    break;
                }
            }

            return foundElement;
        }

        public static T? FindDescendantByType<T>(Visual element) where T : Visual
        {
            return VisualTreeHelperEx.FindDescendantByType(element, typeof(T)) as T;
        }
    }
}
