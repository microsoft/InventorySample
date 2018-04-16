using System;

using Inventory.Services;

namespace Inventory.Models
{
    public class OrderModel : ModelBase
    {
        static public OrderModel CreateEmpty() => new OrderModel { OrderID = -1, CustomerID = -1, IsEmpty = true };

        public long OrderID { get; set; }
        public long CustomerID { get; set; }

        private DateTimeOffset _orderDate;
        public DateTimeOffset OrderDate
        {
            get => _orderDate;
            set => Set(ref _orderDate, value);
        }

        private DateTimeOffset? _shippedDate;
        public DateTimeOffset? ShippedDate
        {
            get => _shippedDate;
            set => Set(ref _shippedDate, value);
        }

        private DateTimeOffset? _deliveredDate;
        public DateTimeOffset? DeliveredDate
        {
            get => _deliveredDate;
            set => Set(ref _deliveredDate, value);
        }

        private int _status;
        public int Status
        {
            get => _status;
            set { if (Set(ref _status, value)) UpdateStatusDependencies(); }
        }

        public int? PaymentType { get; set; }
        public string TrackingNumber { get; set; }

        public int? ShipVia { get; set; }
        public string ShipAddress { get; set; }
        public string ShipCity { get; set; }
        public string ShipRegion { get; set; }
        public string ShipCountryCode { get; set; }
        public string ShipPostalCode { get; set; }
        public string ShipPhone { get; set; }

        public CustomerModel Customer { get; set; }

        public bool IsNew => OrderID <= 0;

        private bool _canEditCustomer = false;
        public bool CanEditCustomer
        {
            get => _canEditCustomer;
            set => Set(ref _canEditCustomer, value);
        }

        public bool CanEditPayment => Status > 0;
        public bool CanEditShipping => Status > 1;
        public bool CanEditDelivery => Status > 2;

        public string StatusDesc => LookupTablesProxy.Instance.GetOrderStatus(Status);
        public string PaymentTypeDesc => LookupTablesProxy.Instance.GetPaymentType(PaymentType);
        public string ShipViaDesc => ShipVia == null ? "" : LookupTablesProxy.Instance.GetShipper(ShipVia.Value);
        public string ShipCountryName => LookupTablesProxy.Instance.GetCountry(ShipCountryCode);

        private void UpdateStatusDependencies()
        {
            switch (Status)
            {
                case 0:
                case 1:
                    ShippedDate = null;
                    DeliveredDate = null;
                    break;
                case 2:
                    ShippedDate = ShippedDate ?? OrderDate;
                    DeliveredDate = null;
                    break;
                case 3:
                    ShippedDate = ShippedDate ?? OrderDate;
                    DeliveredDate = DeliveredDate ?? ShippedDate ?? OrderDate;
                    break;
            }

            NotifyPropertyChanged(nameof(StatusDesc));
            NotifyPropertyChanged(nameof(CanEditPayment));
            NotifyPropertyChanged(nameof(CanEditShipping));
            NotifyPropertyChanged(nameof(CanEditDelivery));
        }

        public override void Merge(ModelBase source)
        {
            if (source is OrderModel model)
            {
                Merge(model);
            }
        }

        public void Merge(OrderModel source)
        {
            if (source != null)
            {
                OrderID = source.OrderID;
                CustomerID = source.CustomerID;
                OrderDate = source.OrderDate;
                ShippedDate = source.ShippedDate;
                DeliveredDate = source.DeliveredDate;
                Status = source.Status;
                PaymentType = source.PaymentType;
                TrackingNumber = source.TrackingNumber;
                ShipVia = source.ShipVia;
                ShipAddress = source.ShipAddress;
                ShipCity = source.ShipCity;
                ShipRegion = source.ShipRegion;
                ShipCountryCode = source.ShipCountryCode;
                ShipPostalCode = source.ShipPostalCode;
                ShipPhone = source.ShipPhone;
                Customer = source.Customer;
            }
        }

        public override string ToString()
        {
            return OrderID.ToString();
        }
    }
}
