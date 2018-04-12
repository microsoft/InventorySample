using System;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using Inventory.Models;

namespace Inventory.Views
{
    public sealed partial class OrderItemCard : UserControl
    {
        public OrderItemCard()
        {
            InitializeComponent();
        }

        #region Item
        public OrderItemModel Item
        {
            get { return (OrderItemModel)GetValue(ItemProperty); }
            set { SetValue(ItemProperty, value); }
        }

        public static readonly DependencyProperty ItemProperty = DependencyProperty.Register(nameof(Item), typeof(OrderItemModel), typeof(OrderItemCard), new PropertyMetadata(null));
        #endregion
    }
}
