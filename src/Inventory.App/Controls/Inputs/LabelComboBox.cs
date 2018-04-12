using System;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Controls;

using Inventory.Animations;

namespace Inventory.Controls
{
    public sealed class LabelComboBox : Control, IInputControl
    {
        public event RoutedEventHandler EnterFocus;

        private Grid _container = null;
        private ComboBox _combo = null;
        private Border _border = null;

        public LabelComboBox()
        {
            DefaultStyleKey = typeof(LabelComboBox);
            IsTabStop = false;
        }

        #region ItemsSource
        public object ItemsSource
        {
            get { return (object)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register("ItemsSource", typeof(object), typeof(LabelComboBox), new PropertyMetadata(null));
        #endregion

        #region Label
        public string Label
        {
            get { return (string)GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }

        public static readonly DependencyProperty LabelProperty = DependencyProperty.Register("Label", typeof(string), typeof(LabelComboBox), new PropertyMetadata(null));
        #endregion

        #region SelectedValue
        public object SelectedValue
        {
            get { return (object)GetValue(SelectedValueProperty); }
            set { SetValue(SelectedValueProperty, value); }
        }

        public static readonly DependencyProperty SelectedValueProperty = DependencyProperty.Register("SelectedValue", typeof(object), typeof(LabelComboBox), new PropertyMetadata(null));
        #endregion

        #region SelectedValuePath
        public string SelectedValuePath
        {
            get { return (string)GetValue(SelectedValuePathProperty); }
            set { SetValue(SelectedValuePathProperty, value); }
        }

        public static readonly DependencyProperty SelectedValuePathProperty = DependencyProperty.Register("SelectedValuePath", typeof(string), typeof(LabelComboBox), new PropertyMetadata(null));
        #endregion

        #region DisplayMemberPath
        public string DisplayMemberPath
        {
            get { return (string)GetValue(DisplayMemberPathProperty); }
            set { SetValue(DisplayMemberPathProperty, value); }
        }

        public static readonly DependencyProperty DisplayMemberPathProperty = DependencyProperty.Register("DisplayMemberPath", typeof(string), typeof(LabelComboBox), new PropertyMetadata(null));
        #endregion

        #region Mode*
        public TextEditMode Mode
        {
            get { return (TextEditMode)GetValue(ModeProperty); }
            set { SetValue(ModeProperty, value); }
        }

        private static void ModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as LabelComboBox;
            control.UpdateMode();
        }

        public static readonly DependencyProperty ModeProperty = DependencyProperty.Register("Mode", typeof(TextEditMode), typeof(LabelComboBox), new PropertyMetadata(TextEditMode.Auto, ModeChanged));
        #endregion

        public void SetFocus()
        {
            _combo?.Focus(FocusState.Programmatic);
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _container = base.GetTemplateChild("container") as Grid;
            _combo = base.GetTemplateChild("combo") as ComboBox;
            _border = base.GetTemplateChild("border") as Border;

            _container.PointerEntered += OnPointerEntered;
            _container.PointerExited += OnPointerExited;

            _combo.GotFocus += OnGotFocus;
            _combo.LostFocus += OnLostFocus;
        }

        private void OnPointerEntered(object sender, PointerRoutedEventArgs e)
        {
            if (Mode == TextEditMode.Auto)
            {
                _border.Fade(500, 0.0, 1.0);
            }
        }

        private void OnPointerExited(object sender, PointerRoutedEventArgs e)
        {
            if (Mode == TextEditMode.Auto)
            {
                _border.Fade(500, 1.0, 0.0);
            }
        }

        private void OnGotFocus(object sender, RoutedEventArgs e)
        {
            _border.Opacity = 1.0;
            EnterFocus?.Invoke(this, e);
        }

        private void OnLostFocus(object sender, RoutedEventArgs e)
        {
            if (Mode == TextEditMode.Auto)
            {
                _border.Opacity = 0.0;
            }
        }

        private void UpdateMode()
        {
            if (_combo != null)
            {
                switch (Mode)
                {
                    case TextEditMode.ReadOnly:
                        _combo.IsTabStop = false;
                        _border.IsHitTestVisible = true;
                        _border.Opacity = 0.0;
                        break;
                    case TextEditMode.Auto:
                        _combo.IsTabStop = true;
                        _border.IsHitTestVisible = false;
                        _border.Opacity = 0.0;
                        break;
                    case TextEditMode.ReadWrite:
                        _combo.IsTabStop = true;
                        _border.IsHitTestVisible = false;
                        _border.Opacity = 1.0;
                        break;
                }
            }
        }
    }
}
