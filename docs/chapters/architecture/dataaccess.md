## Data Access

One of the most important parts of the *Van Arsdel Arquitecture* is how we access to the different Data Sources and how this logic is decoupled from the rest of the app.

We will review in detail how to use the existing the different *data sources* as *Data Providers*, and how we use *Entity Framework Core* for databases as SQLite or SQL server.

### Data Providers

In the *Van Arsdel Inventory* app we have different data sources that we could use to extract the data:

- *SQLite database*
- *SQL Server database*
- *Web API REST*

The data we are going to display and manipulate in the app is manage for the contract `IDataProvider` and each *data source* has to implement this *interface*:

```c#
public interface IDataProvider : IDisposable
{
    Task<IList<CountryCodeModel>> GetCountryCodesAsync();
    Task<IList<OrderStatusModel>> GetOrderStatusAsync();
    Task<IList<PaymentTypeModel>> GetPaymentTypesAsync();
    Task<IList<ShipperModel>> GetShippersAsync();
    Task<IList<TaxTypeModel>> GetTaxTypesAsync();

    Task<PageResult<CustomerModel>> GetCustomersAsync(PageRequest<Customer> request);
    Task<CustomerModel> GetCustomerAsync(long id);
    Task<int> UpdateCustomerAsync(CustomerModel model);
    Task<int> DeleteCustomerAsync(CustomerModel model);

    Task<PageResult<OrderModel>> GetOrdersAsync(PageRequest<Order> request);
    Task<OrderModel> GetOrderAsync(long id);
    Task<OrderModel> CreateNewOrderAsync(long customerID);
    Task<int> UpdateOrderAsync(OrderModel model);
    Task<int> DeleteOrderAsync(OrderModel model);

    Task<PageResult<OrderItemModel>> GetOrderItemsAsync(PageRequest<OrderItem> request);
    Task<OrderItemModel> GetOrderItemAsync(long customerID, int orderLine);
    Task<int> UpdateOrderItemAsync(OrderItemModel model);
    Task<int> DeleteOrderItemAsync(OrderItemModel model);

    Task<PageResult<ProductModel>> GetProductsAsync(PageRequest<Product> request);
    Task<ProductModel> GetProductAsync(string id);
    Task<int> UpdateProductAsync(ProductModel model);
    Task<int> DeleteProductAsync(ProductModel model);
}
```

The interface `IDataProvider` allows us:

- To introduce new data sources of your prefference.
- It will act as the *Mapper* between the data source *entity models* and the *models* that our app will actually use. 

The way to change the default data source that we are going to use in the app is declaring the corresponding *Data Provider* in the following class:

```c#
public class DataProviderFactory : IDataProviderFactory
{
    public IDataProvider CreateDataProvider()
    {
        // TODO: Return selected DataProvider in configuration
        return new SQLiteDataProvider(AppSettings.SQLiteConnectionString);
        //return new SQLServerDataProvider(AppSettings.SQLServerConnectionString);
    }
}
```

### Data Services

When our *data source* is a relational database, we are using [Entity Framework Core](dataaccess.md) to manipulate the data and expose it to our *Data Provider*. Due that we have more than one *data source* with this characteristics, there's an abstraction to use databases implemented in the **VanArsdel.Data** project.

We will have a class implementing `IDataSource` for each *context* representing a database.

```c#
public interface IDataSource : IDisposable
{
    DbSet<CountryCode> CountryCodes { get; }
    DbSet<PaymentType> PaymentTypes { get; }
    DbSet<TaxType> TaxTypes { get; }
    DbSet<OrderStatus> OrderStatus { get; }
    DbSet<Shipper> Shippers { get; }

    DbSet<Customer> Customers { get; }
    DbSet<Order> Orders { get; }
    DbSet<OrderItem> OrderItems { get; }
    DbSet<Product> Products { get; }

    EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
}
``` 

Let's see how the SQLite context is implemented:

```c#
public class SQLiteDb : DbContext, IDataSource
{
    private string _connectionString = null;

    public SQLiteDb(string connectionString)
    {
        _connectionString = connectionString;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite(_connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SubCategory>().HasKey(e => new { e.CategoryID, e.SubCategoryID });
        modelBuilder.Entity<OrderItem>().HasKey(e => new { e.OrderID, e.OrderLine });
    }

    public DbSet<Customer> Customers { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }

    public DbSet<Category> Categories { get; set; }
    public DbSet<SubCategory> SubCategories { get; set; }

    public DbSet<CountryCode> CountryCodes { get; set; }
    public DbSet<PaymentType> PaymentTypes { get; set; }
    public DbSet<TaxType> TaxTypes { get; set; }
    public DbSet<OrderStatus> OrderStatus { get; set; }
    public DbSet<Shipper> Shippers { get; set; }
}
```

There's also one last thing to note. The real diference between this *SQLite context* and the *SQL Server context* is the parameter *connectionString* that we are passing in the constructor of the class. Therefore the data will be delivered exactly with the same implementation for both *contexts*. We will have a base class `DataServiceBase` and two derived classes representing each *data source*:

```c#
public class SQLiteDataService : DataServiceBase
{
    public SQLiteDataService(string connectionString)
        : base(new SQLiteDb(connectionString))
    {
    }
}
```

Once the we have a commun way to expose the data for both *contexts*, we will use it in our implementations of `IDataProvider`.

```c#
public class SQLiteDataProvider : SQLBaseProvider
{
    public SQLiteDataProvider(string connectionString)
        : base(new SQLiteDataService(connectionString))
    {
    }
}
```