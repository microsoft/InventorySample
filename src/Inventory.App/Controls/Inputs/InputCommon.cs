using System;

using Windows.UI.Xaml;

namespace Inventory.Controls
{
    public interface IInputControl
    {
        event RoutedEventHandler EnterFocus;

        TextEditMode Mode { get; set; }

        void SetFocus();
    }

    public enum TextEditMode
    {
        Auto,
        ReadOnly,
        ReadWrite,
    }

    public enum TextValueType
    {
        String,
        Int16,
        Int32,
        Int64,
        Decimal,
        Double
    }
}
