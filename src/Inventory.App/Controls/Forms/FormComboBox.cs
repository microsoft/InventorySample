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

using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Controls;

namespace Inventory.Controls
{
    public class FormComboBox : ComboBox, IFormControl
    {
        public event EventHandler<FormVisualState> VisualStateChanged;

        private Border _backgroundBorder = null;

        private bool _isInitialized = false;

        public FormComboBox()
        {
            DefaultStyleKey = typeof(FormComboBox);
        }

        public FormVisualState VisualState { get; private set; }

        #region Mode*
        public FormEditMode Mode
        {
            get { return (FormEditMode)GetValue(ModeProperty); }
            set { SetValue(ModeProperty, value); }
        }

        private static void ModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as FormComboBox;
            control.UpdateMode();
            control.UpdateVisualState();
        }

        public static readonly DependencyProperty ModeProperty = DependencyProperty.Register(nameof(Mode), typeof(FormEditMode), typeof(FormComboBox), new PropertyMetadata(FormEditMode.Auto, ModeChanged));
        #endregion

        protected override void OnApplyTemplate()
        {
            _backgroundBorder = base.GetTemplateChild("Background") as Border;

            _isInitialized = true;

            UpdateMode();
            UpdateVisualState();

            base.OnApplyTemplate();
        }

        protected override void OnGotFocus(RoutedEventArgs e)
        {
            if (Mode == FormEditMode.Auto)
            {
                SetVisualState(FormVisualState.Focused);
            }

            base.OnGotFocus(e);
        }

        protected override void OnLostFocus(RoutedEventArgs e)
        {
            if (VisualState == FormVisualState.Focused)
            {
                SetVisualState(FormVisualState.Ready);
            }

            base.OnLostFocus(e);
        }

        private void UpdateMode()
        {
            switch (Mode)
            {
                case FormEditMode.Auto:
                    VisualState = FormVisualState.Idle;
                    break;
                case FormEditMode.ReadWrite:
                    VisualState = FormVisualState.Ready;
                    break;
                case FormEditMode.ReadOnly:
                    VisualState = FormVisualState.Disabled;
                    break;
            }
        }

        public void SetVisualState(FormVisualState visualState)
        {
            if (Mode == FormEditMode.ReadOnly)
            {
                visualState = FormVisualState.Disabled;
            }

            if (visualState != VisualState)
            {
                VisualState = visualState;
                UpdateVisualState();
                VisualStateChanged?.Invoke(this, visualState);
            }
        }

        private void UpdateVisualState()
        {
            if (_isInitialized)
            {
                switch (VisualState)
                {
                    case FormVisualState.Idle:
                        _backgroundBorder.Opacity = 0.40;
                        _backgroundBorder.Background = TransparentBrush;
                        break;
                    case FormVisualState.Ready:
                        _backgroundBorder.Opacity = 1.0;
                        _backgroundBorder.Background = OpaqueBrush;
                        break;
                    case FormVisualState.Focused:
                        _backgroundBorder.Opacity = 1.0;
                        _backgroundBorder.Background = OpaqueBrush;
                        break;
                    case FormVisualState.Disabled:
                        _backgroundBorder.Opacity = 0.40;
                        _backgroundBorder.Background = TransparentBrush;
                        IsEnabled = false;
                        Opacity = 0.75;
                        break;
                }
            }
        }

        private readonly Brush TransparentBrush = new SolidColorBrush(Colors.Transparent);
        private readonly Brush OpaqueBrush = new SolidColorBrush(Colors.White);
    }
}
