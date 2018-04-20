## Navigation Service

Since we are using MVVM, and the [ViewModel-First](../mvvm.md#view-first-vs-viewmodel-first) approach, we will use a Navigation Service abstraction to facilitate the ViewModel-based navigation. 

This kind of navigation, oposed to View-based navigation, is the navigation that uses a ViewModel as the subject that determines the navigation. The View isn't specified explicitly. Instead, there is a mechanism to associate each ViewModel with its corresponding View. This is where our Navigation Service comes in. It will perform the navigation itself, but also the will glue the ViewModel with its View.

### INavigationService

This *contract* in charge of the Navigation of the app. The service is agnostic of the platform that is going to use it and that's why is located in the **Inventory.ViewModels** project. 

Let's take a look to the interface:
```csharp
public interface INavigationService
{
    bool IsMainView { get; }

    bool CanGoBack { get; }

    void Initialize(object frame);

    bool Navigate<TViewModel>(object parameter = null);
    bool Navigate(Type viewModelType, object parameter = null);

    Task<int> CreateNewViewAsync<TViewModel>(object parameter = null);
    Task<int> CreateNewViewAsync(Type viewModelType, object parameter = null);

    void GoBack();

    Task CloseViewAsync();
}
```

Let's review each one of these methods:

| Method |	Usage |
| ------ | ------ |
| Initialize | This method is needed in order to initialize the Navigation Service |
| Navigate | There are 2 overloads of this Method. Both the `Type` of the destination ViewModel is needed plus some arguments |
| CreateNewViewAsync | The inventory app example, allow us to create a View in a new Window. There are also 2 overloads of this method, and both need the target ViewModel to load |
| GoBack | Navigate back in the stack |
| CloseViewAsync | Close the actual Window |  

The Navigation Service sits between the View and the ViewModel. As the Navigate method takes the type of the ViewModel, our implementation will have to find the View that is associated with it. Let's review now how this interface is implemented.

### INavigationService implementation

The interface is implemented in the **Inventory.App** project by the `NavigationService` class. 

#### View Lookup
The Navigation Service will need a mechanism to associate Views to ViewModels. In our implementation, this is done using an internal dictionary.

Whenever the `Navigate` method is called, this dictionary will be queried for the Type of the View that corresponds to the ViewModel.

Let's take a look to the implementation of the `Navigate` method:

```csharp
public bool Navigate(Type viewModelType, object parameter = null)
{
    if (Frame == null)
    {
        throw new InvalidOperationException("Navigation frame not initialized.");
    }

    return Frame.Navigate(GetView(viewModelType), parameter);
}
```
In the first place, we're checking whether the `Frame` is null. The Frame property is the object that will perform the navigation at the UI side. It's usually set at the very beginning of the execution. In our application, inside the `ShellView.InitializeNavigation()` method, where the service is resolved.

In the second place, where are telling the `Frame` to navigate to the View associated to `viewModelType`. The lookup is done in `GetView`:
```csharp
static public Type GetView(Type viewModel)
{
    if (_viewModelMap.TryGetValue(viewModel, out Type view))
    {
        return view;
    }
    throw new InvalidOperationException($"View not registered for ViewModel '{viewModel.FullName}'");
}
```

#### View Registration

In order to know the association between a ViewModel and its View, some kind of registration is needed. For this effect, we will expose a method in our implementation:

```csharp
static public void Register<TViewModel, TView>() where TView : Page
{
    if (!_viewModelMap.TryAdd(typeof(TViewModel), typeof(TView)))
    {
        throw new InvalidOperationException($"ViewModel already registered '{typeof(TViewModel).FullName}'");
    }
}
```

It just adds the type of the ViewModel and the type of the View in a common dictionary.

The registration is usually done at the beginning of the execution, so all entries are available as soon as possible. In our application, you can locate the registration in the `Setup` class, inside the `ConfigureNavigation` method.

```csharp
private static void ConfigureNavigation()
{
    NavigationService.Register<ShellViewModel, ShellView>();
    NavigationService.Register<MainShellViewModel, MainShellView>();

    NavigationService.Register<DashboardViewModel, DashboardView>();
	...
}
```

### Additional functionalities

In our implementation, there are a few additional properties and methods to enable more advanced navigation scenarios, like to pop a new Window or close it programmatically. 

#### Opening new Windows

In our application, there are scenarios where we will feature multitasking by opening new Windows. It's provided by the `CreateNewViewAsync` methods.

To know more go to the [dedicated section](../fluent-design/multiple-windows).

Also, we made the functionality symmetrical by exposing the `CloseViewAsync` method, that is very easy to implement thanks to the `ApplicationViewSwitcher` static class.

```csharp
public async Task CloseViewAsync()
{
    int currentId = ApplicationView.GetForCurrentView().Id;
    await ApplicationViewSwitcher.SwitchAsync(MainViewId, currentId, ApplicationViewSwitchingOptions.ConsolidateViews);
}
```