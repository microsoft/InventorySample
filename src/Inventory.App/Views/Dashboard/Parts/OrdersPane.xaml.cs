using System;
using System.Collections.Generic;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using Inventory.Models;

namespace Inventory.Views
{
    public sealed partial class OrdersPane : UserControl
    {
        public OrdersPane()
        {
            InitializeComponent();
        }

        #region ItemsSource
        public IList<OrderModel> ItemsSource
        {
            get { return (IList<OrderModel>)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register("ItemsSource", typeof(IList<OrderModel>), typeof(OrdersPane), new PropertyMetadata(null));
        #endregion
    }
}
