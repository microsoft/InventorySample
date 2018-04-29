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
using Windows.UI.Xaml.Controls;

using Inventory.Animations;

namespace Inventory.Controls
{
    public sealed class Section : ContentControl
    {
        public event RoutedEventHandler HeaderButtonClick;

        private Border _container = null;
        private Grid _content = null;
        private IconLabelButton _button = null;

        public Section()
        {
            DefaultStyleKey = typeof(Section);
        }

        #region Header
        public object Header
        {
            get { return (object)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        private static void HeaderChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as Section;
            control.UpdateControl();
        }

        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register("Header", typeof(object), typeof(Section), new PropertyMetadata(null, HeaderChanged));
        #endregion

        #region HeaderTemplate
        public DataTemplate HeaderTemplate
        {
            get { return (DataTemplate)GetValue(HeaderTemplateProperty); }
            set { SetValue(HeaderTemplateProperty, value); }
        }

        public static readonly DependencyProperty HeaderTemplateProperty = DependencyProperty.Register("HeaderTemplate", typeof(DataTemplate), typeof(Section), new PropertyMetadata(null));
        #endregion

        #region HeaderButtonGlyph
        public string HeaderButtonGlyph
        {
            get { return (string)GetValue(HeaderButtonGlyphProperty); }
            set { SetValue(HeaderButtonGlyphProperty, value); }
        }

        private static void HeaderButtonGlyphChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as Section;
            control.UpdateControl();
        }

        public static readonly DependencyProperty HeaderButtonGlyphProperty = DependencyProperty.Register("HeaderButtonGlyph", typeof(string), typeof(Section), new PropertyMetadata(null, HeaderButtonGlyphChanged));
        #endregion

        #region HeaderButtonLabel
        public string HeaderButtonLabel
        {
            get { return (string)GetValue(HeaderButtonLabelProperty); }
            set { SetValue(HeaderButtonLabelProperty, value); }
        }

        private static void HeaderButtonLabelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as Section;
            control.UpdateControl();
        }

        public static readonly DependencyProperty HeaderButtonLabelProperty = DependencyProperty.Register("HeaderButtonLabel", typeof(string), typeof(Section), new PropertyMetadata(null, HeaderButtonLabelChanged));
        #endregion

        #region IsButtonVisible
        public bool IsButtonVisible
        {
            get { return (bool)GetValue(IsButtonVisibleProperty); }
            set { SetValue(IsButtonVisibleProperty, value); }
        }

        private static void IsButtonVisibleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as Section;
            control.UpdateControl();
        }

        public static readonly DependencyProperty IsButtonVisibleProperty = DependencyProperty.Register("IsButtonVisible", typeof(bool), typeof(Section), new PropertyMetadata(true, IsButtonVisibleChanged));
        #endregion

        #region Footer
        public object Footer
        {
            get { return (object)GetValue(FooterProperty); }
            set { SetValue(FooterProperty, value); }
        }

        public static readonly DependencyProperty FooterProperty = DependencyProperty.Register("Footer", typeof(object), typeof(Section), new PropertyMetadata(null));
        #endregion

        #region FooterTemplate
        public DataTemplate FooterTemplate
        {
            get { return (DataTemplate)GetValue(FooterTemplateProperty); }
            set { SetValue(FooterTemplateProperty, value); }
        }

        public static readonly DependencyProperty FooterTemplateProperty = DependencyProperty.Register("FooterTemplate", typeof(DataTemplate), typeof(Section), new PropertyMetadata(null));
        #endregion

        private void UpdateControl()
        {
            if (_content != null)
            {
                _content.RowDefinitions[0].Height = Header == null ? GridLengths.Zero : GridLengths.Auto;
                _content.RowDefinitions[2].Height = Footer == null ? GridLengths.Zero : GridLengths.Auto;
                if (_button != null)
                {
                    _button.Visibility = IsButtonVisible && !String.IsNullOrEmpty($"{HeaderButtonGlyph}{HeaderButtonLabel}") ? Visibility.Visible : Visibility.Collapsed;
                }
                UpdateContainer();
            }
        }

        public void UpdateContainer()
        {
            if (IsEnabled)
            {
                _container.ClearEffects();
                _content.Opacity = 1.0;
            }
            else
            {
                _container.Grayscale();
                _content.Opacity = 0.5;
            }
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _container = base.GetTemplateChild("container") as Border;
            _content = base.GetTemplateChild("content") as Grid;

            _button = base.GetTemplateChild("button") as IconLabelButton;
            if (_button != null)
            {
                _button.Click += OnClick;
            }
            IsEnabledChanged += OnIsEnabledChanged;

            UpdateControl();
        }

        private void OnClick(object sender, RoutedEventArgs e)
        {
            HeaderButtonClick?.Invoke(this, e);
        }

        private void OnIsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            UpdateContainer();
        }
    }
}
