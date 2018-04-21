using System;

using Windows.ApplicationModel.Activation;

using Inventory.ViewModels;

namespace Inventory.Services
{
    #region ActivationInfo
    public class ActivationInfo
    {
        static public ActivationInfo CreateDefault() => Create<DashboardViewModel>();

        static public ActivationInfo Create<TViewModel>(object entryArgs = null) where TViewModel : ViewModelBase
        {
            return new ActivationInfo
            {
                EntryViewModel = typeof(TViewModel),
                EntryArgs = entryArgs
            };
        }

        public Type EntryViewModel { get; set; }
        public object EntryArgs { get; set; }
    }
    #endregion

    static public class ActivationService
    {
        static public ActivationInfo GetActivationInfo(IActivatedEventArgs args)
        {
            switch (args.Kind)
            {
                case ActivationKind.Protocol:
                    return GetProtocolActivationInfo(args as ProtocolActivatedEventArgs);

                case ActivationKind.Launch:
                default:
                    return ActivationInfo.CreateDefault();
            }
        }

        private static ActivationInfo GetProtocolActivationInfo(ProtocolActivatedEventArgs args)
        {
            if (args != null)
            {
                switch (args.Uri.AbsolutePath.ToLowerInvariant())
                {
                    case "customer":
                    case "customers":
                        long customerID = args.Uri.GetInt64Parameter("id");
                        if (customerID > 0)
                        {
                            return ActivationInfo.Create<CustomerDetailsViewModel>(new CustomerDetailsArgs { CustomerID = customerID });
                        }
                        return ActivationInfo.Create<CustomersViewModel>(new CustomerListArgs());
                    case "order":
                    case "orders":
                        long orderID = args.Uri.GetInt64Parameter("id");
                        if (orderID > 0)
                        {
                            return ActivationInfo.Create<OrderDetailsViewModel>(new OrderDetailsArgs { OrderID = orderID });
                        }
                        return ActivationInfo.Create<OrdersViewModel>(new OrderListArgs());
                    case "product":
                    case "products":
                        string productID = args.Uri.GetParameter("id");
                        if (productID != null)
                        {
                            return ActivationInfo.Create<ProductDetailsViewModel>(new ProductDetailsArgs { ProductID = productID });
                        }
                        return ActivationInfo.Create<ProductsViewModel>(new ProductListArgs());
                }
            }
            return ActivationInfo.CreateDefault();
        }
    }
}
