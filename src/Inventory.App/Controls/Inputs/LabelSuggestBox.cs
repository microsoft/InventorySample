using System;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Controls;

using Windows.Foundation;

using Inventory.Animations;

namespace Inventory.Controls
{
    public class LabelSuggestBox : Control, IInputControl
    {
        public event RoutedEventHandler EnterFocus;

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

        #region DisplayText
        public string DisplayText
        {
            get { return (string)GetValue(DisplayTextProperty); }
            set { SetValue(DisplayTextProperty, value); }
        }

        public static readonly DependencyProperty DisplayTextProperty = DependencyProperty.Register(nameof(DisplayText), typeof(string), typeof(LabelSuggestBox), new PropertyMetadata(null));
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

            _autoSuggestBox.GotFocus += OnGotFocus;
            _autoSuggestBox.LostFocus += OnLostFocus;

            _autoSuggestBox.TextChanged += (s, a) => TextChanged?.Invoke(s, a);
            _autoSuggestBox.SuggestionChosen += (s, a) => SuggestionChosen?.Invoke(s, a);
            _autoSuggestBox.QuerySubmitted += (s, a) => QuerySubmitted?.Invoke(s, a);

            UpdateMode();
        }

        private void OnPointerEntered(object sender, PointerRoutedEventArgs e)
        {
            if (Mode == TextEditMode.Auto)
            {
                if (_autoSuggestBox.Opacity == 0.0)
                {
                    _border.Fade(500, 0.0, 1.0);
                }
            }
        }

        private void OnPointerExited(object sender, PointerRoutedEventArgs e)
        {
            if (Mode == TextEditMode.Auto)
            {
                if (_autoSuggestBox.Opacity == 0.0)
                {
                    _border.Fade(500, 1.0, 0.0);
                }
            }
        }

        private void OnGotFocus(object sender, RoutedEventArgs e)
        {
            _autoSuggestBox.Text = DisplayText;
            _autoSuggestBox.Opacity = 1.0;
            _border.Opacity = 1.0;
            _displayText.Visibility = Visibility.Collapsed;
            EnterFocus?.Invoke(this, e);
        }

        private void OnLostFocus(object sender, RoutedEventArgs e)
        {
            if (Mode == TextEditMode.Auto)
            {
                _border.Opacity = 0.0;
            }
            _displayText.Visibility = Visibility.Visible;
            _autoSuggestBox.Opacity = 0.0;
        }

        private void UpdateMode()
        {
            if (_autoSuggestBox != null)
            {
                switch (Mode)
                {
                    case TextEditMode.ReadOnly:
                        _displayText.Opacity = 0.5;
                        _autoSuggestBox.Visibility = Visibility.Collapsed;
                        _border.Visibility = Visibility.Collapsed;
                        break;
                    case TextEditMode.Auto:
                        _displayText.Opacity = 1.0;
                        _autoSuggestBox.Visibility = Visibility.Visible;
                        _border.Opacity = 0.0;
                        _border.IsHitTestVisible = false;
                        _border.Visibility = Visibility.Visible;
                        break;
                    case TextEditMode.ReadWrite:
                        _displayText.Opacity = 1.0;
                        _autoSuggestBox.Visibility = Visibility.Visible;
                        _border.Opacity = 1.0;
                        _border.IsHitTestVisible = false;
                        _border.Visibility = Visibility.Visible;
                        break;
                }
            }
        }
    }
}
