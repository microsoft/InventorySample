## Inventory App initial setup

Every app or web, with complex patterns involved, normally requires an initial setup to put evething in motion. In this section we will review the initial app setup required to make it works.

### The Startup class

As you may know, the App class (App.xaml file in the Inventory.App project), is the fisrt class to be executed when we launch the application. In the override method `OnLaunched` of this class, we are running the intial setup and the first page to load:

```csharp
protected override async void OnLaunched(LaunchActivatedEventArgs e)
{
    var frame = Window.Current.Content as Frame;

    if (frame == null)
    {
        frame = new Frame();
        Window.Current.Content = frame;
    }

    if (e.PrelaunchActivated == false)
    {
        await Startup.ConfigureAsync();

        if (frame.Content == null)
        {
            var args = new ShellArgs
            {
                ViewModel = EntryViewModel,
                Parameter = EntryArgs
            };
            frame.Navigate(typeof(MainShellView), args);
        }
        Window.Current.Activate();
    }

    ApplicationView.GetForCurrentView().SetPreferredMinSize(new Size(500, 500));
}
```  

Calling the method `ConfigureAsync` of the `Startup` class, we are initializing everything we need to start working with the app:
 ```csharp
 static public async Task ConfigureAsync()
{
    ServiceLocator.Configure(_serviceCollection);

    ConfigureNavigation();

    await EnsureDatabaseAsync();
    await ConfigureDataHelper();
}
 ```

We can distinguish the following setups:

- `ServiceLocator.Configure(_serviceCollection)`: Initialize [Dependency Injection and the Service Locator](#Dependency-Injection-and-ServiceLocator).
- `ConfigureNavigation`: This method registers the ViewModels and the Views in order to make our *Navigation Service* works. You can check more about the *Navigation Service* [here](navigation-service.md#INavigationService-implementation).
- `EnsureDatabaseAsync`: This method makes sure that the database exists or created if there`s no one.
- `ConfigureDataHelper`: Once the database is created, we are caching the setup tables values in a `ILookupTables` service to avoid continuous access to the database. If you want to know more about the Data Services, please check [this section](dataaccess.md#Accessing-the-data-from-the-app).


## Dependency Injection

### What is Dependency Injection

Dependency Injection (DI) is a design pattern to create loosely coupled classes. It's also the fifth statement of the *SOLID* principles:

> One should "depend upon abstractions, [not] concretions

The best way to understand it is by a simple example of how to use it. The following code uses DI to implement loose coupling:

```c#
public interface IClass2 
{
}

public class Class2 : IClass2
{
}

public class Class1
{
    public readonly IClass2 _class2;
 
    public Class1():this(DependencyFactory.Resolve<IClass2>())
    {
    }
 
    public Class1(IClass2 class2)
    {
        _class2 = class2;
    }
} 
```

This is what we know when we have a look a little closer to this class:

- `Class1` needs an *interface* `IClass2` to work.
- `Class1` doesn't know which class is implementing the *interface* `IClass2` and how it was initialized.
- If we would want to test `Class1`, we don't need the Class2 that implements the interface `IClass2`, we just need a *mock* of this interface.

### Advantages of using DI

There are 2 important reasons to use *DI*:

- **Unit Testing**: DI enables you to replace complex dependencies, such as databases, with mocked implementations of those dependencies. This allows you to completely isolate the code that is being testing.
- **Validation/Exception Management**: DI allows you to inject additional code between the dependencies. For example, it is possible to inject exception management logic or validation logic, which means that the developer no longer needs to write this logic for every class.

### Dependency Injection and ServiceLocator

We are using *DI* to decoupled our services from the ViewModels and encourage unit testing best practices at the same time. More info about DI [here](../dependency-injection.md#dependency-injection). The DI framework we are using in our app is the new `Microsoft.Extensions.DependencyInjection` created by Microsoft for `dotnet core`. We also have a section explining it in detail [here](../dependency-injection.md#Microsoft.Extensions.DependencyInjection).

Because of the app allows to open a new window and allow the user to work with it separately from the rest of the app, we have to be careful when we define the registration of our Services or ViewModels. We have created the `ServiceLocator` class to solve this problem:

- **Multiple Service Locators**: We need multiple service locators when we open a new window, to avoid re-utilization of the same services in different contexts.
```csharp
static private readonly ConcurrentDictionary<int, ServiceLocator> _serviceLocators = new ConcurrentDictionary<int, ServiceLocator>();

static public ServiceLocator Current
{
    get
    {
        int currentViewId = ApplicationView.GetForCurrentView().Id;
        return _serviceLocators.GetOrAdd(currentViewId, key => new ServiceLocator());
    }
}
```

- **Register Services and ViewModels**: This is done in the `Configure` method. We can divide the registration depending on the way the service will be resolve.  

```csharp
static public void Configure(IServiceCollection serviceCollection)
{
    serviceCollection.AddSingleton<IDataServiceFactory, DataServiceFactory>();
    serviceCollection.AddSingleton<ILookupTables, LookupTables>();
    serviceCollection.AddSingleton<ICustomerService, CustomerService>();
    serviceCollection.AddSingleton<IOrderService, OrderService>();
    ...
    
    serviceCollection.AddScoped<IContextService, ContextService>();
    serviceCollection.AddScoped<INavigationService, NavigationService>();
    serviceCollection.AddScoped<IDialogService, DialogService>();
    serviceCollection.AddScoped<ICommonServices, CommonServices>();

    serviceCollection.AddTransient<ShellViewModel>();
    serviceCollection.AddTransient<MainShellViewModel>();
    ...
    ...

    _rootServiceProvider = serviceCollection.BuildServiceProvider();
}
```

| Resolved as | Services |
| ----------- | -------- |
| **Singleton**: Same service every time we resolve it | Example of these type of services are: IDataServiceFactory, ILookupTables, IMessageService, ILogService, etc |
| **Scoped**: A new instance of the service will ve returned if we are in a different scope, i.e. when a new window is opened | Example of these type of services are: IContextService, INavigationService, IDialogService and ICommonServices
| **Transient**: A new version of the service is returned every time we request for it | All the ViewModels of the app are registered as Transient |



