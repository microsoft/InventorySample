# View Models

In a [MVVM](../mvvm.md#mvvm) architecture, and addionally applying *ViewModel first* development, the ViewModels turns into the main core of the app. 

Also, because of the choice of ViewModel first approach, the ViewModels doesn't need to know about Views and therefore, the whole app could be defined using ViewModels instead coupling the User Interface to the development.

That's why, it's important to define the principal ViewModels that will be the base of the implementation.

## The Base ViewModels

We can distinguish between 2 types of base ViewModels in the Inventory Sample app. The ones related with *infrastructure* and the ones related with a *common data structure* like a List of elements or the detail of a domain model:

### Infrastructure base ViewModels

There are 2 infrastructure base ViewModels: `ModelBase` and `ViewModelBase`.

#### ModelBase

This ViewModel is basically the `INotifyPropertyChanged` implementation plus the `Set` generic method to inform that a property has been changed:

```csharp
public class ModelBase : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    public bool IsEmpty { get; set; }

    virtual public void Merge(ModelBase source) { }

    protected bool Set<T>(ref T field, T newValue = default(T), [CallerMemberName] string propertyName = null)
    {
        if (!EqualityComparer<T>.Default.Equals(field, newValue))
        {
            field = newValue;
            NotifyPropertyChanged(propertyName);
            return true;
        }
        return false;
    }

    public void NotifyPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public void NotifyChanges()
    {
        // Notify all properties
        NotifyPropertyChanged("");
    }
}
```

#### ViewModelBase

This ViewModel inherits from the prevoius `ModelBase` and has the extra commun funcionality of:
- Resolve the common services that every ViewModel in the app uses, which are: `IContextService`, `INavigationService`, `IMessageService`, `IDialogService` and the `ILogService`. 
- It handles the Log common operations for all ViewModels in the app.
- It's responsable of communicate the status of the ViewModel using the [Message Service](message-service.md#message-service).

```csharp
public class ViewModelBase : ModelBase
{
    private Stopwatch _stopwatch = new Stopwatch();

    public ViewModelBase(ICommonServices commonServices)
    {
        ContextService = commonServices.ContextService;
        NavigationService = commonServices.NavigationService;
        MessageService = commonServices.MessageService;
        DialogService = commonServices.DialogService;
        LogService = commonServices.LogService;
    }

    public IContextService ContextService { get; }
    public INavigationService NavigationService { get; }
    public IMessageService MessageService { get; }
    public IDialogService DialogService { get; }
    public ILogService LogService { get; }

    public bool IsMainView => ContextService.IsMainView;

    virtual public string Title => String.Empty;

    public async void LogInformation(string source, string action, string message, string description)
    {
        await LogService.WriteAsync(LogType.Information, source, action, message, description);
    }

    public async void LogWarning(string source, string action, string message, string description)
    {
        await LogService.WriteAsync(LogType.Warning, source, action, message, description);
    }

    public void LogException(string source, string action, Exception exception)
    {
        LogError(source, action, exception.Message, exception.ToString());
    }
    public async void LogError(string source, string action, string message, string description)
    {
        await LogService.WriteAsync(LogType.Error, source, action, message, description);
    }

    public void StartStatusMessage(string message)
    {
        StatusMessage(message);
        _stopwatch.Reset();
        _stopwatch.Start();
    }
    public void EndStatusMessage(string message)
    {
        _stopwatch.Stop();
        StatusMessage($"{message} ({_stopwatch.Elapsed.TotalSeconds:#0.000} seconds)");
    }

    public void StatusReady()
    {
        MessageService.Send(this, "StatusMessage", "Ready");
    }
    public void StatusMessage(string message)
    {
        MessageService.Send(this, "StatusMessage", message);
    }
    public void StatusError(string message)
    {
        MessageService.Send(this, "StatusError", message);
    }
}
```

### Domain Base ViewModels

We have defined 2 base ViewModels representing a common data structure to be displayed in then Inventory Sample app: List and Detail generic ViewModels.

#### GenericListViewModel

```csharp
class GenericListViewModel<TModel> : ViewModelBase where TModel : ModelBase
```
This base ViewModel handles the common operations of a List, like:
- **Filter** the list.
- **Item selected** and comminicate the event to possible subscribers.
- **Multiple Items** selection.
- **`OnNew`**, **`OnRefresh`** and **`OnDeleteSelection`** abstract methods to be implemented in the ViewModels that inherit from this base ViewModel.

#### GenericDetailsViewModel

 ```csharp
 class GenericDetailsViewModel<TModel> : ViewModelBase where TModel : ModelBase, new()
 ```
This generic ViewModel puts focus on:

- Display the detail of the model as **ReadOnly** or **Edit** mode.
- **Validation** item check when Save changes.
- **`GetValidationConstraints`** abstract method to be implemented to provide the specific validation constraint for the *Domain Model*.
- Additional abstract methods to implement: **`SaveItemAsync`**, **`DeleteItemAsync`** and **`ConfirmDeleteAsync`**.
