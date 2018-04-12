using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;

using Inventory.Models;
using Inventory.Services;
using Inventory.Providers;

namespace Inventory.ViewModels
{
    public class OrderItemDetailsViewModel : DetailsViewModel<OrderItemModel>
    {
        public OrderItemDetailsViewModel(IDataProviderFactory providerFactory, IServiceManager serviceManager)
            : base(providerFactory, serviceManager)
        {
        }

        override public string Title => (Item?.IsNew ?? true) ? TitleNew : TitleEdit;
        public string TitleNew => $"New Order Item, Order #{OrderID}";
        public string TitleEdit => $"Order Item #{Item?.OrderID} - {Item?.OrderLine}" ?? String.Empty;

        public override bool IsNewItem => Item?.IsNew ?? false;

        public long OrderID { get; set; }

        public ICommand ProductSelectedCommand => new RelayCommand<ProductModel>(ProductSelected);
        private void ProductSelected(ProductModel product)
        {
            Item.ProductID = product.ProductID;
            Item.UnitPrice = product.ListPrice;
            Item.Product = product;

            Item.NotifyChanges();
        }

        protected override void ItemUpdated()
        {
            NotifyPropertyChanged(nameof(Title));
        }

        public async Task LoadAsync(OrderItemViewState state)
        {
            OrderID = state.OrderID;
            if (state.OrderLine > 0)
            {
                using (var dp = ProviderFactory.CreateDataProvider())
                {
                    var item = await dp.GetOrderItemAsync(OrderID, state.OrderLine);
                    Item = item ?? new OrderItemModel { OrderID = state.OrderID, OrderLine = state.OrderLine, IsDeleted = true };
                }
            }
            else
            {
                Item = new OrderItemModel { OrderID = OrderID };
                IsEditMode = true;
            }
        }

        protected override async Task SaveItemAsync(OrderItemModel model)
        {
            using (var dataProvider = ProviderFactory.CreateDataProvider())
            {
                await Task.Delay(100);
                await dataProvider.UpdateOrderItemAsync(model);
                NotifyPropertyChanged(nameof(Title));
            }
        }

        protected override async Task DeleteItemAsync(OrderItemModel model)
        {
            using (var dataProvider = ProviderFactory.CreateDataProvider())
            {
                await dataProvider.DeleteOrderItemAsync(model);
            }
        }

        protected override async Task<bool> ConfirmDeleteAsync()
        {
            return await DialogService.ShowAsync("Confirm Delete", "Are you sure you want to delete current order item?", "Ok", "Cancel");
        }

        override protected IEnumerable<IValidationConstraint<OrderItemModel>> ValidationConstraints
        {
            get
            {
                yield return new RequiredConstraint<OrderItemModel>("Product", m => m.ProductID);
                yield return new NonZeroConstraint<OrderItemModel>("Quantity", m => m.Quantity);
                yield return new PositiveConstraint<OrderItemModel>("Quantity", m => m.Quantity);
                yield return new LessThanConstraint<OrderItemModel>("Quantity", m => m.Quantity, 100);
                yield return new PositiveConstraint<OrderItemModel>("Discount", m => m.Discount);
                yield return new NonGreaterThanConstraint<OrderItemModel>("Discount", m => m.Discount, (double)Item.Subtotal, "'Subtotal'");
            }
        }
    }
}
