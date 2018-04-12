using System;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using Inventory.ViewModels;

namespace Inventory.Views
{
    public sealed partial class OrderItemsDetails : UserControl
    {
        public OrderItemsDetails()
        {
            InitializeComponent();
        }

        #region ViewModel
        public OrderItemDetailsViewModel ViewModel
        {
            get { return (OrderItemDetailsViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register("ViewModel", typeof(OrderItemDetailsViewModel), typeof(OrderItemsDetails), new PropertyMetadata(null));
        #endregion
    }
}
