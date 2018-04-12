using System;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using Inventory.ViewModels;

namespace Inventory.Views
{
    public sealed partial class OrdersOrderItems : UserControl
    {
        public OrdersOrderItems()
        {
            InitializeComponent();
        }

        #region ViewModel
        public OrderItemListViewModel ViewModel
        {
            get { return (OrderItemListViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(nameof(ViewModel), typeof(OrderItemListViewModel), typeof(OrdersOrderItems), new PropertyMetadata(null));
        #endregion
    }
}
