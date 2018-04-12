using System;

using Windows.UI.Xaml;

namespace Inventory
{
    static public class GridLengths
    {
        static public readonly GridLength Zero = new GridLength(0);
        static public readonly GridLength Star = new GridLength(1, GridUnitType.Star);
        static public readonly GridLength Auto = new GridLength(1, GridUnitType.Auto);
    }
}
