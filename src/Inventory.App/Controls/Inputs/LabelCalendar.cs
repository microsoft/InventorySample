using System;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Controls;

using Inventory.Animations;

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
                _calendar.MinDate = MinDate ?? DateTimeOffset.MinValue;
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
            _container.PointerExited += OnPointerExited;

            _calendar.GotFocus += OnGotFocus;
            _calendar.LostFocus += OnLostFocus;
            _calendar.Opened += OnOpened;

            UpdateMode();
            SetMinDate();
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

        private void OnOpened(object sender, object e)
        {
            _border.Opacity = 1.0;
            EnterFocus?.Invoke(this, new RoutedEventArgs());
        }

        private void UpdateMode()
        {
            if (_calendar != null)
            {
                switch (Mode)
                {
                    case TextEditMode.ReadOnly:
                        _calendar.IsTabStop = false;
                        _border.IsHitTestVisible = true;
                        _border.Opacity = 0.0;
                        break;
                    case TextEditMode.Auto:
                        _calendar.IsTabStop = true;
                        _border.IsHitTestVisible = false;
                        _border.Opacity = 0.0;
                        break;
                    case TextEditMode.ReadWrite:
                        _calendar.IsTabStop = true;
                        _border.IsHitTestVisible = false;
                        _border.Opacity = 1.0;
                        break;
                }
            }
        }
    }
}
