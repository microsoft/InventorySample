using System;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using Inventory.ViewModels;

namespace Inventory.Views
{
    public sealed partial class CustomersList : UserControl
    {
        public CustomersList()
        {
            InitializeComponent();
        }

        #region ViewModel
        public CustomerListViewModel ViewModel
        {
            get { return (CustomerListViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register("ViewModel", typeof(CustomerListViewModel), typeof(CustomersList), new PropertyMetadata(null));
        #endregion
    }
}
