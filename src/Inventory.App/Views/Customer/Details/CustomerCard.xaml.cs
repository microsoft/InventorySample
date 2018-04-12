using System;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using Inventory.Models;

namespace Inventory.Views
{
    public sealed partial class CustomerCard : UserControl
    {
        public CustomerCard()
        {
            InitializeComponent();
        }

        #region Item
        public CustomerModel Item
        {
            get { return (CustomerModel)GetValue(ItemProperty); }
            set { SetValue(ItemProperty, value); }
        }

        public static readonly DependencyProperty ItemProperty = DependencyProperty.Register(nameof(Item), typeof(CustomerModel), typeof(CustomerCard), new PropertyMetadata(null));
        #endregion
    }
}
