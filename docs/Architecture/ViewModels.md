# ViewModels

In a MVVM architecture, the view-models turns into the main core of the app.

Views depend on view-models to present the information to the users and execute user actions, but view-models don’t need to know anything about the views. Applying this principle permits reusing the same view-model in different view implementations for different platforms.

In general, many of the properties and methods are common for all the different view-models so we use inheritance to reuse functionality.

## ViewModelBase

This view-model is the base class for all view-models in the application and offers the following funcionality:

- Resolves and exposes the common services that every view-model in the app uses, which are: `IContextService`, `INavigationService`, `IMessageService`, `IDialogService` and the `ILogService`. 
- It handles the Log common operations for all view-models in the app.
- It's responsable of communicate the status of the view-model using the Message Service.

The following code is a simplified implementation of the ViewModelBase.

```csharp
public class ViewModelBase : ObservableObject
{
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

    virtual public string Title => String.Empty;

    public async void LogInformation(string source, string action, string message, string description)
    {
        await LogService.WriteAsync(LogType.Information, source, action, message, description);
    }

    public async void LogWarning(string source, string action, string message, string description)
    {
        await LogService.WriteAsync(LogType.Warning, source, action, message, description);
    }

    public async void LogError(string source, string action, string message, string description)
    {
        await LogService.WriteAsync(LogType.Error, source, action, message, description);
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

## Domain Base ViewModels

We have defined 2 base view-models representing a common data structure to be displayed in then Inventory Sample app: List and Detail generic view-models.

### GenericListViewModel

```csharp
class GenericListViewModel<TModel> : ViewModelBase where TModel : ObservableObject
```
This base view-model handles the common operations of a List, like:
- Filter the list of items.
- Single and multiple item selection handling.
- Refresh, New and Delete operations over the list of items.

This class contains abstract methods to be implemented by the concrete clasess like `OnRefresh`, `OnNew` or `OnDeleteSelection`.

### GenericDetailsViewModel

 ```csharp
 class GenericDetailsViewModel<TModel> : ViewModelBase where TModel : ObservableObject, new()
 ```
This generic view-model puts focus on:

- Display the detail of the model as *ReadOnly* or *Edit* mode.
- Validates user input before saving changes.
- Delete current item.

This class contains abstract methods to be implemented by the concrete clasess like `SaveItem`, `DeleteItem` or `GetValidationConstraints`.
