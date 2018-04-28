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
using Windows.UI.Xaml.Media;

namespace Inventory
{
    partial class ElementSet<T>
    {
        public ElementSet<T> SetOpacity(double value) => ForEach(v => v.Opacity = value);

        public ElementSet<T> SetVisibility(Visibility value) => ForEach(v => v.Visibility = value);

        public ElementSet<T> SetForeground(Brush value) => ForEach<Control>(v => v.Foreground = value);

        public ElementSet<T> SetBackground(Brush value) => ForEach<Control>(v => v.Background = value).ForEach<Panel>(v => v.Background = value);
    }
}
