using System;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using Inventory.Models;

namespace Inventory.Views
{
    public sealed partial class OrderItemsCard : UserControl
    {
        public OrderItemsCard()
        {
            InitializeComponent();
        }

        #region Item
        public OrderItemModel Item
        {
            get { return (OrderItemModel)GetValue(ItemProperty); }
            set { SetValue(ItemProperty, value); }
        }

        public static readonly DependencyProperty ItemProperty = DependencyProperty.Register(nameof(Item), typeof(OrderItemModel), typeof(OrderItemsCard), new PropertyMetadata(null));
        #endregion
    }
}
