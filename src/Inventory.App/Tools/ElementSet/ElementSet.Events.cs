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

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace Inventory
{
    partial class ElementSet<T>
    {
        public event RoutedEventHandler Click
        {
            add => ForEach<Button>(v => v.Click += value);
            remove => ForEach<Button>(v => v.Click -= value);
        }

        public event TappedEventHandler Tapped
        {
            add => ForEach<FrameworkElement>(v => v.Tapped += value);
            remove => ForEach<FrameworkElement>(v => v.Tapped -= value);
        }

        public event PointerEventHandler PointerEntered
        {
            add => ForEach<FrameworkElement>(v => v.PointerEntered += value);
            remove => ForEach<FrameworkElement>(v => v.PointerEntered -= value);
        }
        public event PointerEventHandler PointerExited
        {
            add => ForEach<FrameworkElement>(v => v.PointerExited += value);
            remove => ForEach<FrameworkElement>(v => v.PointerExited -= value);
        }

        public event RoutedEventHandler GotFocus
        {
            add => ForEach<UIElement>(v => v.GotFocus += value);
            remove => ForEach<UIElement>(v => v.GotFocus -= value);
        }
        public event RoutedEventHandler LostFocus
        {
            add => ForEach<UIElement>(v => v.LostFocus += value);
            remove => ForEach<UIElement>(v => v.LostFocus -= value);
        }
    }
}
