using System;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using Inventory.Models;
using Inventory.ViewModels;

namespace Inventory.Views
{
    public sealed partial class ProductCard : UserControl
    {
        public ProductCard()
        {
            InitializeComponent();
        }

        #region ViewModel
        public ProductDetailsViewModel ViewModel
        {
            get { return (ProductDetailsViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(nameof(ViewModel), typeof(ProductDetailsViewModel), typeof(ProductCard), new PropertyMetadata(null));
        #endregion

        #region Item
        public ProductModel Item
        {
            get { return (ProductModel)GetValue(ItemProperty); }
            set { SetValue(ItemProperty, value); }
        }

        public static readonly DependencyProperty ItemProperty = DependencyProperty.Register(nameof(Item), typeof(ProductModel), typeof(ProductCard), new PropertyMetadata(null));
        #endregion
    }
}
