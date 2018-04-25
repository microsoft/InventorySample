using System;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Controls;

using Windows.Foundation;

namespace Inventory.Controls
{
    public class LabelSuggestBox : Control, IInputControl
    {
        public event RoutedEventHandler GotFocus;

        public event TypedEventHandler<AutoSuggestBox, AutoSuggestBoxTextChangedEventArgs> TextChanged;
        public event TypedEventHandler<AutoSuggestBox, AutoSuggestBoxSuggestionChosenEventArgs> SuggestionChosen;
        public event TypedEventHandler<AutoSuggestBox, AutoSuggestBoxQuerySubmittedEventArgs> QuerySubmitted;

        private Grid _container = null;
        private AutoSuggestBox _autoSuggestBox = null;
        private TextBlock _displayText = null;
        private Border _border = null;

        public LabelSuggestBox()
        {
            DefaultStyleKey = typeof(LabelSuggestBox);
        }

        #region Label
        public string Label
        {
            get { return (string)GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }

        public static readonly DependencyProperty LabelProperty = DependencyProperty.Register(nameof(Label), typeof(string), typeof(LabelSuggestBox), new PropertyMetadata(null));
        #endregion

        #region Text
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(nameof(Text), typeof(string), typeof(LabelSuggestBox), new PropertyMetadata(null));
        #endregion

        #region Mode*
        public TextEditMode Mode
        {
            get { return (TextEditMode)GetValue(ModeProperty); }
            set { SetValue(ModeProperty, value); }
        }

        private static void ModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as LabelSuggestBox;
            control.UpdateMode();
        }

        public static readonly DependencyProperty ModeProperty = DependencyProperty.Register(nameof(Mode), typeof(TextEditMode), typeof(LabelSuggestBox), new PropertyMetadata(TextEditMode.Auto, ModeChanged));
        #endregion

        #region ItemsSource
        public object ItemsSource
        {
            get { return (object)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register(nameof(ItemsSource), typeof(object), typeof(LabelSuggestBox), new PropertyMetadata(null));
        #endregion

        #region ItemTemplate
        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }

        public static readonly DependencyProperty ItemTemplateProperty = DependencyProperty.Register(nameof(ItemTemplate), typeof(DataTemplate), typeof(LabelSuggestBox), new PropertyMetadata(null));
        #endregion

        #region ItemContainerStyle
        public Style ItemContainerStyle
        {
            get { return (Style)GetValue(ItemContainerStyleProperty); }
            set { SetValue(ItemContainerStyleProperty, value); }
        }

        public static readonly DependencyProperty ItemContainerStyleProperty = DependencyProperty.Register(nameof(ItemContainerStyle), typeof(Style), typeof(LabelSuggestBox), new PropertyMetadata(null));
        #endregion

        #region PlaceholderText
        public string PlaceholderText
        {
            get { return (string)GetValue(PlaceholderTextProperty); }
            set { SetValue(PlaceholderTextProperty, value); }
        }

        public static readonly DependencyProperty PlaceholderTextProperty = DependencyProperty.Register(nameof(PlaceholderText), typeof(string), typeof(LabelSuggestBox), new PropertyMetadata(null));
        #endregion

        #region AutoMaximizeSuggestionArea
        public bool AutoMaximizeSuggestionArea
        {
            get { return (bool)GetValue(AutoMaximizeSuggestionAreaProperty); }
            set { SetValue(AutoMaximizeSuggestionAreaProperty, value); }
        }

        public static readonly DependencyProperty AutoMaximizeSuggestionAreaProperty = DependencyProperty.Register(nameof(AutoMaximizeSuggestionArea), typeof(bool), typeof(LabelSuggestBox), new PropertyMetadata(false));
        #endregion

        #region TextMemberPath
        public string TextMemberPath
        {
            get { return (string)GetValue(TextMemberPathProperty); }
            set { SetValue(TextMemberPathProperty, value); }
        }

        public static readonly DependencyProperty TextMemberPathProperty = DependencyProperty.Register(nameof(TextMemberPath), typeof(string), typeof(LabelSuggestBox), new PropertyMetadata(null));
        #endregion

        public void SetFocus()
        {
            _autoSuggestBox?.Focus(FocusState.Programmatic);
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _container = base.GetTemplateChild("container") as Grid;
            _autoSuggestBox = base.GetTemplateChild("autoSuggestBox") as AutoSuggestBox;
            _displayText = base.GetTemplateChild("displayText") as TextBlock;
            _border = base.GetTemplateChild("border") as Border;

            _container.PointerEntered += OnPointerEntered;
            _container.PointerExited += OnPointerExited;
            _container.PointerPressed += OnPointerPressed;

            _container.GotFocus += OnGotFocus;
            _container.LostFocus += OnLostFocus;

            _autoSuggestBox.TextChanged += (s, a) => TextChanged?.Invoke(s, a);
            _autoSuggestBox.SuggestionChosen += (s, a) => SuggestionChosen?.Invoke(s, a);
            _autoSuggestBox.QuerySubmitted += (s, a) => QuerySubmitted?.Invoke(s, a);

            UpdateMode();            
        }

        private void OnPointerPressed(object sender, PointerRoutedEventArgs e)
        {
            if (_autoSuggestBox.Visibility != Visibility.Visible)
            {
                if (Mode == TextEditMode.Auto)
                {
                    UpdateVisualState(_visualState.Edit);
                    //Forzing an upddate of the layout to realise the TextBox's AutoSuggestBox
                    _autoSuggestBox.UpdateLayout();
                    _autoSuggestBox.Focus(FocusState.Programmatic);
                }
            }
        }

        private void OnPointerEntered(object sender, PointerRoutedEventArgs e)
        {
            if (_autoSuggestBox.Visibility != Visibility.Visible)
            {
                _border.Visibility = Visibility.Visible;
            }
        }

        private void OnPointerExited(object sender, PointerRoutedEventArgs e)
        {
            if (_autoSuggestBox.Visibility != Visibility.Visible)
            {
                _border.Visibility = Visibility.Collapsed;
            }

        }

        private void OnGotFocus(object sender, RoutedEventArgs e)
        {
            GotFocus?.Invoke(this, e);
        }

        private void OnLostFocus(object sender, RoutedEventArgs e)
        {
            if (Mode == TextEditMode.Auto)
            {
                UpdateVisualState(_visualState.ReadOnly);
            }
        }

        private void UpdateMode()
        {
            switch (Mode)
            {
                case TextEditMode.Auto:
                    UpdateVisualState(_visualState.ReadOnly);
                    System.Diagnostics.Debug.WriteLine("Mode.Auto");
                    break;
                case TextEditMode.ReadOnly:
                    System.Diagnostics.Debug.WriteLine("Mode.ReadOnly");
                    UpdateVisualState(_visualState.ReadOnly);
                    break;
                case TextEditMode.ReadWrite:
                    System.Diagnostics.Debug.WriteLine("Mode.Write");
                    UpdateVisualState(_visualState.Edit);
                    break;
            }
        }

        private enum _visualState { ReadOnly, Edit }
        private void UpdateVisualState(_visualState mode)
        {
            if (_autoSuggestBox != null && _displayText != null && _border != null)
            {
                switch (mode)
                {
                    case _visualState.ReadOnly:
                        _displayText.Text = _autoSuggestBox.Text;
                        _autoSuggestBox.Visibility = Visibility.Collapsed;
                        _border.Visibility = Visibility.Collapsed;
                        _displayText.Visibility = Visibility.Visible;
                        break;
                    case _visualState.Edit:
                        _autoSuggestBox.Visibility = Visibility.Visible;
                        _border.Visibility = Visibility.Collapsed;
                        _displayText.Visibility = Visibility.Collapsed;
                        break;
                }
            }
        }
    }
}
