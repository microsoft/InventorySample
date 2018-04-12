using System;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Inventory.Controls
{
    public sealed class LabelTextBlock : Control
    {
        public LabelTextBlock()
        {
            this.DefaultStyleKey = typeof(LabelTextBlock);
            this.IsTabStop = false;
        }

        #region Label
        public string Label
        {
            get { return (string)GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }

        public static readonly DependencyProperty LabelProperty = DependencyProperty.Register("Label", typeof(string), typeof(LabelTextBlock), new PropertyMetadata(null));
        #endregion

        #region Text*
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        private static void TextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as LabelTextBlock;
            control.UpdateControl();
        }

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(nameof(Text), typeof(string), typeof(LabelTextBlock), new PropertyMetadata(null, TextChanged));
        #endregion

        #region ValueType*
        public TextValueType ValueType
        {
            get { return (TextValueType)GetValue(ValueTypeProperty); }
            set { SetValue(ValueTypeProperty, value); }
        }

        private static void ValueTypeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as LabelTextBlock;
            control.UpdateControl();
        }

        public static readonly DependencyProperty ValueTypeProperty = DependencyProperty.Register(nameof(ValueType), typeof(TextValueType), typeof(LabelTextBlock), new PropertyMetadata(TextValueType.String, ValueTypeChanged));
        #endregion

        #region DisplayText
        public string DisplayText
        {
            get { return (string)GetValue(DisplayTextProperty); }
            set { SetValue(DisplayTextProperty, value); }
        }

        public static readonly DependencyProperty DisplayTextProperty = DependencyProperty.Register(nameof(DisplayText), typeof(string), typeof(LabelTextBlock), new PropertyMetadata(null));
        #endregion

        #region TextAlignment
        public TextAlignment TextAlignment
        {
            get { return (TextAlignment)GetValue(TextAlignmentProperty); }
            set { SetValue(TextAlignmentProperty, value); }
        }

        public static readonly DependencyProperty TextAlignmentProperty = DependencyProperty.Register("TextAlignment", typeof(TextAlignment), typeof(LabelTextBlock), new PropertyMetadata(TextAlignment.Left));
        #endregion

        #region TextWrapping
        public TextWrapping TextWrapping
        {
            get { return (TextWrapping)GetValue(TextWrappingProperty); }
            set { SetValue(TextWrappingProperty, value); }
        }

        public static readonly DependencyProperty TextWrappingProperty = DependencyProperty.Register("TextWrapping", typeof(TextWrapping), typeof(LabelTextBlock), new PropertyMetadata(TextWrapping.NoWrap));
        #endregion

        private void UpdateControl()
        {
            string str = Text;

            switch (ValueType)
            {
                case TextValueType.Int16:
                    Int16.TryParse(Text, out Int16 n16);
                    str = n16.ToString();
                    break;
                case TextValueType.Int32:
                    Int32.TryParse(Text, out Int32 n32);
                    str = n32.ToString();
                    break;
                case TextValueType.Int64:
                    Int64.TryParse(Text, out Int64 n64);
                    str = n64.ToString();
                    break;
                case TextValueType.Decimal:
                    Decimal.TryParse(Text, out Decimal m);
                    str = m.ToString("0.00");
                    break;
                case TextValueType.Double:
                    Double.TryParse(Text, out Double d);
                    str = d.ToString("0.00");
                    break;
            }
            DisplayText = null;
            DisplayText = str;
        }
    }
}
