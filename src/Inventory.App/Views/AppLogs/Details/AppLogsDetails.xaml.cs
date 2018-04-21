using System;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using Inventory.ViewModels;

namespace Inventory.Views
{
    public sealed partial class AppLogsDetails : UserControl
    {
        public AppLogsDetails()
        {
            InitializeComponent();
        }

        #region ViewModel
        public AppLogDetailsViewModel ViewModel
        {
            get { return (AppLogDetailsViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register("ViewModel", typeof(AppLogDetailsViewModel), typeof(AppLogsDetails), new PropertyMetadata(null));
        #endregion
    }
}
