using System;
using System.Collections.Generic;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using Inventory.Models;

namespace Inventory.Views
{
    public sealed partial class CustomersPane : UserControl
    {
        public CustomersPane()
        {
            InitializeComponent();
        }

        #region ItemsSource
        public IList<CustomerModel> ItemsSource
        {
            get { return (IList<CustomerModel>)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register("ItemsSource", typeof(IList<CustomerModel>), typeof(CustomersPane), new PropertyMetadata(null));
        #endregion
    }
}
