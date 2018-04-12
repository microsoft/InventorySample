using System;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Inventory.Data.Services
{
    public interface IDataSource : IDisposable
    {
        DbSet<Category> Categories { get; }
        DbSet<CountryCode> CountryCodes { get; }
        DbSet<OrderStatus> OrderStatus { get; }
        DbSet<PaymentType> PaymentTypes { get; }
        DbSet<Shipper> Shippers { get; }
        DbSet<TaxType> TaxTypes { get; }

        DbSet<Customer> Customers { get; }
        DbSet<Order> Orders { get; }
        DbSet<OrderItem> OrderItems { get; }
        DbSet<Product> Products { get; }

        EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}
