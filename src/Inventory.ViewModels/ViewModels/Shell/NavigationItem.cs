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

using Inventory.Models;

namespace Inventory.ViewModels
{
    public class NavigationItem : ObservableObject
    {
        public NavigationItem(Type viewModel)
        {
            ViewModel = viewModel;
        }
        public NavigationItem(int glyph, string label, Type viewModel) : this(viewModel)
        {
            Label = label;
            Glyph = Char.ConvertFromUtf32(glyph).ToString();
        }

        public readonly string Glyph;
        public readonly string Label;
        public readonly Type ViewModel;

        private string _badge = null;
        public string Badge
        {
            get => _badge;
            set => Set(ref _badge, value);
        }
    }
}
