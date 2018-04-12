using System;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using Inventory.ViewModels;

namespace Inventory.Views
{
    public sealed partial class ProductsList : UserControl
    {
        public ProductsList()
        {
            InitializeComponent();
        }

        #region ViewModel
        public ProductListViewModel ViewModel
        {
            get { return (ProductListViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register("ViewModel", typeof(ProductListViewModel), typeof(ProductsList), new PropertyMetadata(null));
        #endregion
    }
}
