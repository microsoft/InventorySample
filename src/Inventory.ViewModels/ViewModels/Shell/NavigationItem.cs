using System;

using Inventory.Models;

namespace Inventory.ViewModels
{
    public class NavigationItem : ModelBase
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
