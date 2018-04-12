using System;
using System.Collections.Generic;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using Inventory.Models;

namespace Inventory.Views
{
    public sealed partial class ProductsPane : UserControl
    {
        public ProductsPane()
        {
            InitializeComponent();
        }

        #region ItemsSource
        public IList<ProductModel> ItemsSource
        {
            get { return (IList<ProductModel>)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register("ItemsSource", typeof(IList<ProductModel>), typeof(ProductsPane), new PropertyMetadata(null));
        #endregion
    }
}
