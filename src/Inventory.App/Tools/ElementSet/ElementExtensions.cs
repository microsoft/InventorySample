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
using System.Linq;

using Windows.UI.Xaml;

namespace Inventory
{
    static public class ElementExtensions
    {
        static public void Show(this FrameworkElement elem, bool visible = true)
        {
            elem.Visibility = visible ? Visibility.Visible : Visibility.Collapsed;
        }

        static public void Hide(this FrameworkElement elem)
        {
            elem.Visibility = Visibility.Collapsed;
        }

        static public bool IsCategory(this FrameworkElement elem, string category)
        {
            if (elem.Tag is String tag)
            {
                return tag.Split(' ').Any(s => s == category);
            }
            return false;
        }

        static public bool IsCategory(this FrameworkElement elem, params string[] categories)
        {
            if (elem.Tag is String tag)
            {
                return tag.Split(' ').Any(s => categories.Any(c => s == c.Trim()));
            }
            return false;
        }
    }
}
