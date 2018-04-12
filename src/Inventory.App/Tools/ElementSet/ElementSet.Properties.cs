using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Inventory
{
    partial class ElementSet<T>
    {
        public ElementSet<T> SetOpacity(double value) => ForEach(v => v.Opacity = value);

        public ElementSet<T> SetVisibility(Visibility value) => ForEach(v => v.Visibility = value);

        public ElementSet<T> SetForeground(Brush value) => ForEach<Control>(v => v.Foreground = value);

        public ElementSet<T> SetBackground(Brush value) => ForEach<Control>(v => v.Background = value).ForEach<Panel>(v => v.Background = value);
    }
}
