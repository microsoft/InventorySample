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
    public class LabelCalendar : Control, IInputControl
    {
        public event RoutedEventHandler EnterFocus;

        private Grid _container = null;
        private CalendarDatePicker _calendar = null;
        private Border _border = null;

        public LabelCalendar()
        {
            DefaultStyleKey = typeof(LabelCalendar);
            IsTabStop = false;
        }

        #region Label
        public string Label
        {
            get { return (string)GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }

        public static readonly DependencyProperty LabelProperty = DependencyProperty.Register(nameof(Label), typeof(string), typeof(LabelCalendar), new PropertyMetadata(null));
        #endregion

        #region Date
        public DateTimeOffset? Date
        {
            get { return (DateTimeOffset?)GetValue(DateProperty); }
            set { SetValue(DateProperty, value); }
        }

        public static readonly DependencyProperty DateProperty = DependencyProperty.Register(nameof(Date), typeof(DateTimeOffset?), typeof(LabelCalendar), new PropertyMetadata(null));
        #endregion

        #region MinDate*
        public DateTimeOffset? MinDate
        {
            get { return (DateTimeOffset?)GetValue(MinDateProperty); }
            set { SetValue(MinDateProperty, value); }
        }

        private static void MinDateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as LabelCalendar;
            control.SetMinDate();
        }

        private void SetMinDate()
        {
            if (_calendar != null)
            {
                _calendar.MinDate = MinDate ?? DateTimeOffset.Now.AddYears(-150);
            }
        }

        public static readonly DependencyProperty MinDateProperty = DependencyProperty.Register(nameof(MinDate), typeof(DateTimeOffset?), typeof(LabelCalendar), new PropertyMetadata(null, MinDateChanged));
        #endregion

        #region MaxDate
        public DateTimeOffset MaxDate
        {
            get { return (DateTimeOffset)GetValue(MaxDateProperty); }
            set { SetValue(MaxDateProperty, value); }
        }

        public static readonly DependencyProperty MaxDateProperty = DependencyProperty.Register(nameof(MaxDate), typeof(DateTimeOffset), typeof(LabelCalendar), new PropertyMetadata(DateTimeOffset.MaxValue));
        #endregion

        #region Mode*
        public TextEditMode Mode
        {
            get { return (TextEditMode)GetValue(ModeProperty); }
            set { SetValue(ModeProperty, value); }
        }

        private static void ModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as LabelCalendar;
            control.UpdateMode();
        }

        public static readonly DependencyProperty ModeProperty = DependencyProperty.Register(nameof(Mode), typeof(TextEditMode), typeof(LabelCalendar), new PropertyMetadata(TextEditMode.Auto, ModeChanged));
        #endregion

        public void SetFocus()
        {
            _calendar?.Focus(FocusState.Programmatic);
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _container = base.GetTemplateChild("container") as Grid;
            _calendar = base.GetTemplateChild("calendar") as CalendarDatePicker;
            _border = base.GetTemplateChild("border") as Border;

            _container.PointerEntered += OnPointerEntered;
            _container.PointerEntered += OnPointerEntered;
            _container.PointerExited += OnPointerExited;

            _calendar.GotFocus += OnGotFocus;
            _calendar.LostFocus += OnLostFocus;
            _calendar.Opened += OnOpened;

            UpdateMode();
            SetMinDate();
        }

        private void OnPointerPressed(object sender, PointerRoutedEventArgs e)
        {

            if (Mode == TextEditMode.Auto)
            {
                UpdateVisualState(_visualState.Edit);
                _calendar.Focus(FocusState.Programmatic);
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

        private void OnOpened(object sender, object e)
        {
            EnterFocus?.Invoke(this, new RoutedEventArgs());
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
                        _calendar.BorderThickness = new Thickness(0);
                        break;
                    case _visualState.Edit:
                        _border.Visibility = Visibility.Collapsed;
                        _calendar.BorderThickness = new Thickness(1);
                        break;
                }
            }
        }
    }
}
