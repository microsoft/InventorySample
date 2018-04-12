using System;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using Inventory.Models;

namespace Inventory.Views
{
    public sealed partial class OrderCard : UserControl
    {
        public OrderCard()
        {
            InitializeComponent();
        }

        #region Item
        public OrderModel Item
        {
            get { return (OrderModel)GetValue(ItemProperty); }
            set { SetValue(ItemProperty, value); }
        }

        public static readonly DependencyProperty ItemProperty = DependencyProperty.Register(nameof(Item), typeof(OrderModel), typeof(OrderCard), new PropertyMetadata(null));
        #endregion
    }
}
