using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using Inventory.Data;
using Inventory.Models;
using Inventory.Providers;

namespace Inventory.Controls
{
    public sealed partial class ProductSuggestBox : UserControl
    {
        public ProductSuggestBox()
        {
            ProviderFactory = ServiceLocator.Current.GetService<IDataProviderFactory>();
            InitializeComponent();
        }

        private IDataProviderFactory ProviderFactory { get; }

        #region Items
        public IList<ProductModel> Items
        {
            get { return (IList<ProductModel>)GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
        }

        public static readonly DependencyProperty ItemsProperty = DependencyProperty.Register(nameof(Items), typeof(IList<ProductModel>), typeof(ProductSuggestBox), new PropertyMetadata(null));
        #endregion

        #region DisplayText
        public string DisplayText
        {
            get { return (string)GetValue(DisplayTextProperty); }
            set { SetValue(DisplayTextProperty, value); }
        }

        public static readonly DependencyProperty DisplayTextProperty = DependencyProperty.Register(nameof(DisplayText), typeof(string), typeof(ProductSuggestBox), new PropertyMetadata(null));
        #endregion

        #region IsReadOnly*
        public bool IsReadOnly
        {
            get { return (bool)GetValue(IsReadOnlyProperty); }
            set { SetValue(IsReadOnlyProperty, value); }
        }

        private static void IsReadOnlyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as ProductSuggestBox;
            control.suggestBox.Mode = ((bool)e.NewValue == true) ? TextEditMode.ReadOnly : TextEditMode.Auto;
        }

        public static readonly DependencyProperty IsReadOnlyProperty = DependencyProperty.Register(nameof(IsReadOnly), typeof(bool), typeof(ProductSuggestBox), new PropertyMetadata(false, IsReadOnlyChanged));
        #endregion

        #region ProductSelectedCommand
        public ICommand ProductSelectedCommand
        {
            get { return (ICommand)GetValue(ProductSelectedCommandProperty); }
            set { SetValue(ProductSelectedCommandProperty, value); }
        }

        public static readonly DependencyProperty ProductSelectedCommandProperty = DependencyProperty.Register(nameof(ProductSelectedCommand), typeof(ICommand), typeof(ProductSuggestBox), new PropertyMetadata(null));
        #endregion

        private async void OnTextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                if (args.CheckCurrent())
                {
                    Items = String.IsNullOrEmpty(sender.Text) ? null : await GetItems(sender.Text);
                }
            }
        }

        private async Task<IList<ProductModel>> GetItems(string query)
        {
            using (var dataProvider = ProviderFactory.CreateDataProvider())
            {
                var request = new DataRequest<Product>()
                {
                    Query = query,
                    OrderBy = r => r.Name
                };
                return await dataProvider.GetProductsAsync(0, 20, request);
            }
        }

        private void OnSuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            ProductSelectedCommand?.TryExecute(args.SelectedItem);
        }
    }
}
