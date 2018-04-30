#region copyright
// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************
#endregion

using System;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Controls;


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
            _container.PointerPressed += OnPointerPressed;

            _combo.GotFocus += OnGotFocus;
            _combo.LostFocus += OnLostFocus;

            UpdateMode();
        }

        private void OnPointerPressed(object sender, PointerRoutedEventArgs e)
        {
            if (Mode == TextEditMode.Auto)
            {
                UpdateVisualState(_visualState.Edit);
                _combo.Focus(FocusState.Programmatic);
            }
        }

        private void OnPointerEntered(object sender, PointerRoutedEventArgs e)
        {
            if (Mode == TextEditMode.Auto)
            {
                _border.Visibility = Visibility.Visible;
            }
        }

        private void OnPointerExited(object sender, PointerRoutedEventArgs e)
        {
            if (Mode == TextEditMode.Auto)
            {
                _border.Visibility = Visibility.Collapsed;
            }
        }

        private void OnGotFocus(object sender, RoutedEventArgs e)
        {
            EnterFocus?.Invoke(this, e);
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
            if (_border != null)
            {
                switch (mode)
                {
                    case _visualState.ReadOnly:
                        _border.Visibility = Visibility.Collapsed;
                        _combo.BorderThickness = new Thickness(0);
                        break;
                    case _visualState.Edit:
                        _border.Visibility = Visibility.Collapsed;
                        _combo.BorderThickness = new Thickness(1);
                        break;
                }
            }
        }
    }
}
