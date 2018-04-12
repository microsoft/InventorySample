using System;

using Windows.UI.Xaml.Controls;
using Windows.UI.ViewManagement;
using Windows.ApplicationModel.Core;

namespace Inventory
{
    static public class PageExtensions
    {
        static public void SetTitle(this Control page, string title)
        {
            ApplicationView.GetForCurrentView().Title = title;
        }

        static public bool IsMainView(this Control page)
        {
            return CoreApplication.GetCurrentView().IsMain;
        }
    }
}
