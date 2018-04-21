using System;

using Windows.ApplicationModel.Activation;

using Inventory.ViewModels;
using Windows.Foundation;

namespace Inventory.Services
{
    public class ActivationInfo
    {
        static public ActivationInfo CreateDefault()
        {
            return new ActivationInfo
            {
                EntryViewModel = typeof(DashboardViewModel),
                EntryArgs = null
            };
        }

        public Type EntryViewModel { get; set; }
        public object EntryArgs { get; set; }
    }

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
                        long customerID = GetParameterID(args.Uri);
                        if (customerID > 0)
                        {
                            return new ActivationInfo
                            {
                                EntryViewModel = typeof(CustomerDetailsViewModel),
                                EntryArgs = new CustomerDetailsArgs { CustomerID = customerID }
                            };
                        }
                        break;
                    case "order":
                        long orderID = GetParameterID(args.Uri);
                        if (orderID > 0)
                        {
                            return new ActivationInfo
                            {
                                EntryViewModel = typeof(OrderDetailsViewModel),
                                EntryArgs = new OrderDetailsArgs { OrderID = orderID }
                            };
                        }
                        break;
                    case "product":
                        var decoder = new WwwFormUrlDecoder(args.Uri.Query);
                        string productID = decoder.GetFirstValueByName("id");
                        if (productID != null)
                        {
                            return new ActivationInfo
                            {
                                EntryViewModel = typeof(ProductDetailsViewModel),
                                EntryArgs = new ProductDetailsArgs { ProductID = productID }
                            };
                        }
                        break;
                }
            }
            return ActivationInfo.CreateDefault();
        }

        private static long GetParameterID(Uri uri)
        {
            string query = uri.Query;
            if (!String.IsNullOrEmpty(query))
            {
                var decoder = new WwwFormUrlDecoder(uri.Query);
                string value = decoder.GetFirstValueByName("id");
                if (Int64.TryParse(value, out Int64 id))
                {
                    return id;
                }
            }
            return 0;
        }
    }
}
