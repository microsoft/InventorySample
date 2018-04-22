using System;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using Inventory.Models;
using Inventory.ViewModels;

namespace Inventory.Views
{
    public sealed partial class CustomerCard : UserControl
    {
        public CustomerCard()
        {
            InitializeComponent();
        }

        #region ViewModel
        public CustomerDetailsViewModel ViewModel
        {
            get { return (CustomerDetailsViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(nameof(ViewModel), typeof(CustomerDetailsViewModel), typeof(CustomerCard), new PropertyMetadata(null));
        #endregion

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
