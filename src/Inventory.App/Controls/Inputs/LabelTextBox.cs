using System;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Controls;

namespace Inventory.Controls
{
    public sealed class LabelTextBox : Control, IInputControl
    {
        public event RoutedEventHandler GotFocus;

        private Grid _container = null;
        private TextBox _inputText = null;
        private TextBlock _displayText = null;
        private Border _border = null;

        public LabelTextBox()
        {
            DefaultStyleKey = typeof(LabelTextBox);
        }

        #region Label
        public string Label
        {
            get { return (string)GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }

        public static readonly DependencyProperty LabelProperty = DependencyProperty.Register("Label", typeof(string), typeof(LabelTextBox), new PropertyMetadata(null));
        #endregion

        #region Text
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        private static void TextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as LabelTextBox;
            control.UpdateFormattedText();
        }

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(LabelTextBox), new PropertyMetadata(null, TextChanged));
        #endregion

        #region FormattedText
        public string FormattedText
        {
            get { return (string)GetValue(FormattedTextProperty); }
            set { SetValue(FormattedTextProperty, value); }
        }

        public static readonly DependencyProperty FormattedTextProperty = DependencyProperty.Register("FormattedText", typeof(string), typeof(LabelTextBox), new PropertyMetadata(null));
        #endregion

        #region ValueType
        public TextValueType ValueType
        {
            get { return (TextValueType)GetValue(ValueTypeProperty); }
            set { SetValue(ValueTypeProperty, value); }
        }

        public static readonly DependencyProperty ValueTypeProperty = DependencyProperty.Register("ValueType", typeof(TextValueType), typeof(LabelTextBox), new PropertyMetadata(TextValueType.String));
        #endregion

        #region Mode
        public TextEditMode Mode
        {
            get { return (TextEditMode)GetValue(ModeProperty); }
            set { SetValue(ModeProperty, value); }
        }

        private static void ModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as LabelTextBox;
            control.UpdateMode();
        }

        public static readonly DependencyProperty ModeProperty = DependencyProperty.Register("Mode", typeof(TextEditMode), typeof(LabelTextBox), new PropertyMetadata(TextEditMode.Auto, ModeChanged));
        #endregion

        #region MaxLength
        public int MaxLength
        {
            get { return (int)GetValue(MaxLengthProperty); }
            set { SetValue(MaxLengthProperty, value); }
        }

        public static readonly DependencyProperty MaxLengthProperty = DependencyProperty.Register("MaxLength", typeof(int), typeof(LabelTextBox), new PropertyMetadata(0));
        #endregion

        public void SetFocus()
        {
            _inputText?.Focus(FocusState.Programmatic);
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _container = base.GetTemplateChild("container") as Grid;
            _inputText = base.GetTemplateChild("inputText") as TextBox;
            _displayText = base.GetTemplateChild("displayText") as TextBlock;
            _border = base.GetTemplateChild("border") as Border;

            _container.PointerEntered += OnPointerEntered;
            _container.PointerExited += OnPointerExited;
            _container.PointerPressed += OnPointerPressed;

            _inputText.BeforeTextChanging += OnBeforeTextChanging;
            _inputText.GotFocus += OnGotFocus;
            _inputText.LostFocus += OnLostFocus;

            UpdateMode();
        }

        private void OnPointerPressed(object sender, PointerRoutedEventArgs e)
        {
            if (_inputText.Visibility != Visibility.Visible)
            {
                if (Mode == TextEditMode.Auto)
                {
                    UpdateVisualState(_visualState.Edit);
                    _inputText.Focus(FocusState.Programmatic);
                }
            }
        }

        private void OnPointerEntered(object sender, PointerRoutedEventArgs e)
        {
            if (_inputText.Visibility != Visibility.Visible)
            {
                _border.Visibility = Visibility.Visible;
            }
        }

        private void OnPointerExited(object sender, PointerRoutedEventArgs e)
        {
            if (_inputText.Visibility != Visibility.Visible)
            {
                _border.Visibility = Visibility.Collapsed;
            }
        }

        private void OnGotFocus(object sender, RoutedEventArgs e)
        {
            _inputText.SelectionStart = _inputText.Text.Length;
            GotFocus?.Invoke(this, e);
        }

        private void OnLostFocus(object sender, RoutedEventArgs e)
        {
            if (Mode == TextEditMode.Auto)
            {
                UpdateVisualState(_visualState.ReadOnly);
            }
        }

        private void OnBeforeTextChanging(TextBox sender, TextBoxBeforeTextChangingEventArgs args)
        {
            if (ValueType == TextValueType.String)
            {
                return;
            }

            string str = args.NewText;

            if (String.IsNullOrEmpty(str) || str == "-")
            {
                return;
            }

            switch (ValueType)
            {
                case TextValueType.Int16:
                    args.Cancel = !Int16.TryParse(str, out Int16 n16);
                    break;
                case TextValueType.Int32:
                    args.Cancel = !Int32.TryParse(str, out Int32 n32);
                    break;
                case TextValueType.Int64:
                    args.Cancel = !Int64.TryParse(str, out Int64 n64);
                    break;
                case TextValueType.Decimal:
                    args.Cancel = !Decimal.TryParse(str, out Decimal m);
                    break;
                case TextValueType.Double:
                    args.Cancel = !Double.TryParse(str, out Double d);
                    break;
                default:
                    break;
            }
        }

        private void UpdateMode()
        {
            switch (Mode)
            {
                case TextEditMode.Auto:
                case TextEditMode.ReadOnly:
                    UpdateVisualState(_visualState.ReadOnly);
                    break;
                case TextEditMode.ReadWrite:
                    UpdateVisualState(_visualState.Edit);
                    break;
            }
        }

        private enum _visualState { ReadOnly, Edit }
        private void UpdateVisualState(_visualState mode)
        {
            if (_inputText != null && _displayText != null && _border != null)
            {
                switch (mode)
                {
                    case _visualState.ReadOnly:
                        _inputText.Visibility = Visibility.Collapsed;
                        _border.Visibility = Visibility.Collapsed;
                        _displayText.Visibility = Visibility.Visible;
                        break;
                    case _visualState.Edit:
                        _displayText.Visibility = Visibility.Collapsed;
                        _inputText.Visibility = Visibility.Visible;
                        _border.Visibility = Visibility.Collapsed;
                        break;
                }
            }
        }

        private void UpdateText()
        {
            string str = Text;

            switch (ValueType)
            {
                case TextValueType.String:
                    return;
                case TextValueType.Int16:
                    Int16.TryParse(Text, out Int16 n16);
                    str = n16 == 0 ? "" : n16.ToString();
                    break;
                case TextValueType.Int32:
                    Int32.TryParse(Text, out Int32 n32);
                    str = n32 == 0 ? "" : n32.ToString();
                    break;
                case TextValueType.Int64:
                    Int64.TryParse(Text, out Int64 n64);
                    str = n64 == 0 ? "" : n64.ToString();
                    break;
                case TextValueType.Decimal:
                    Decimal.TryParse(Text, out Decimal m);
                    str = m == 0 ? "" : m.ToString("0.00");
                    break;
                case TextValueType.Double:
                    Double.TryParse(Text, out Double d);
                    str = d == 0 ? "" : d.ToString("0.00");
                    break;
            }
            Text = null;
            Text = str;
        }

        private void UpdateFormattedText()
        {
            switch (ValueType)
            {
                case TextValueType.String:
                    FormattedText = Text;
                    break;
                case TextValueType.Int16:
                    Int16.TryParse(Text, out Int16 n16);
                    FormattedText = n16.ToString();
                    break;
                case TextValueType.Int32:
                    Int32.TryParse(Text, out Int32 n32);
                    FormattedText = n32.ToString();
                    break;
                case TextValueType.Int64:
                    Int64.TryParse(Text, out Int64 n64);
                    FormattedText = n64.ToString();
                    break;
                case TextValueType.Decimal:
                    Decimal.TryParse(Text, out Decimal m);
                    FormattedText = m.ToString("0.00");
                    break;
                case TextValueType.Double:
                    Double.TryParse(Text, out Double d);
                    FormattedText = d.ToString("0.00");
                    break;
            }
        }
    }
}
