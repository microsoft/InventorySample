using System;
using System.Linq;

using Windows.UI.Xaml;

namespace Inventory
{
    static public class ElementExtensions
    {
        static public void Show(this FrameworkElement elem, bool visible = true)
        {
            elem.Visibility = visible ? Visibility.Visible : Visibility.Collapsed;
        }

        static public void Hide(this FrameworkElement elem)
        {
            elem.Visibility = Visibility.Collapsed;
        }

        static public bool IsCategory(this FrameworkElement elem, string category)
        {
            if (elem.Tag is String tag)
            {
                return tag.Split(' ').Any(s => s == category);
            }
            return false;
        }

        static public bool IsCategory(this FrameworkElement elem, params string[] categories)
        {
            if (elem.Tag is String tag)
            {
                return tag.Split(' ').Any(s => categories.Any(c => s == c));
            }
            return false;
        }
    }
}
