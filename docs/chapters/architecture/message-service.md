## Message Service

Another service that it's important to know is the one represented by the interface `IMessageService`. The mission of this service is to send messages between ViewModels or Views, and once the subscriber receive the message, react to it executing some action. This pattern is known as the **Event Agregator Pattern**:

![event aggregator](../img/eventaggregator.png)

The `IMessageService` exposes the following methods:
```csharp
public interface IMessageService
{
    void Subscribe<TSender>(object target, Action<TSender, string, object> action) where TSender : class;
    void Subscribe<TSender, TArgs>(object target, Action<TSender, string, TArgs> action) where TSender : class;

    void Unsubscribe(object target);
    void Unsubscribe<TSender>(object target) where TSender : class;
    void Unsubscribe<TSender, TArgs>(object target) where TSender : class;

    void Send<TSender, TArgs>(TSender sender, string message, TArgs args) where TSender : class;
}
```

| Methods | Description |
| ------ | ------ |
| Subscribe | This is used to subscribe your class to a specific event. When we subscribe to an event, we need to indicate the sender (`TSender`), the target object, and the action to execute when the message is received |
| Unsubscribe | Unsubscribe an event already registered. It's important to unsubscribe events in order to avoid memory leaks |
| Send | Communicate to subscriber that a specific event has occurred. When we send a message we are passing the sender (`TSender`), the message that identifies the event occurred (`message`) and additional arguments when necessary |


### How to use it

The best thing to understand how it works is to check how it's being used in the *Inventory App*. For example, lets see how the `CustomersViewModel` loads the detail of a Customer, when it is selected from the List of Customers. In this process the following actors are involved:

- `CustomerListViewModel`: When an element of the list is selected, this ViewModel will use the `IMessageService` to send to the Customer selected to the possible subscribers.

```csharp
private TModel _selectedItem = default(TModel);
public TModel SelectedItem
{
    get => _selectedItem;
    set
    {
        if (Set(ref _selectedItem, value))
        {
            if (!IsMultipleSelection)
            {
                MessageService.Send(this, "ItemSelected", _selectedItem);
            }
        }
    }
}
```

- `CustomersViewModel`: This ViewModel will *Subcribe* to the Customers list *ItemSelected* event. It will also unsubscribe from this event when necessary. 
```csharp
public void Subscribe()
{
    MessageService.Subscribe<CustomerListViewModel>(this, OnMessage);
    CustomerList.Subscribe();
    CustomerDetails.Subscribe();
    CustomerOrders.Subscribe();
}

private async void OnMessage(CustomerListViewModel viewModel, string message, object args)
{
    if (viewModel == CustomerList && message == "ItemSelected")
    {
        await ContextService.RunAsync(() =>
        {
            OnItemSelected();
        });
    }
}

private async void OnItemSelected()
{
    if (CustomerDetails.IsEditMode)
    {
        StatusReady();
        CustomerDetails.CancelEdit();
    }
    CustomerOrders.IsMultipleSelection = false;
    var selected = CustomerList.SelectedItem;
    if (!CustomerList.IsMultipleSelection)
    {
        if (selected != null && !selected.IsEmpty)
        {
            await PopulateDetails(selected);
            await PopulateOrders(selected);
        }
    }
    CustomerDetails.Item = selected;
}
```

We are executing the `OnItemSelected` method when we received from the `CustomerListViewModel` a message `ItemSelected`.

Finally, as we previously mentioned, it's important to control the subscriptions and unsubcriptions of the events to prevent memory leaks. We will do this overriding the events `OnNavigatedTo` and `OnNavigatingFrom`methods of our Views:
```csharp
protected override async void OnNavigatedTo(NavigationEventArgs e)
{
    ViewModel.Subscribe();
    await ViewModel.LoadAsync(e.Parameter as CustomerListArgs);
}

protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
{
    ViewModel.Unload();
    ViewModel.Unsubscribe();
}
```

### Message Service Implementation

The purpouse of this section is to show how an *Event Aggregator Pattern* can be implemented. Lets review the main methods of the `IMessageService`:

#### Subscribe

```csharp
public void Subscribe<TSender, TArgs>(object target, Action<TSender, string, TArgs> action) where TSender : class
{
    if (target == null)
        throw new ArgumentNullException(nameof(target));
    if (action == null)
        throw new ArgumentNullException(nameof(action));

    lock (_sync)
    {
        var subscriber = _subscribers.Where(r => r.Target == target).FirstOrDefault();
        if (subscriber == null)
        {
            subscriber = new Subscriber(target);
            _subscribers.Add(subscriber);
        }
        subscriber.AddSubscription<TSender, TArgs>(action);
    }
}
```

