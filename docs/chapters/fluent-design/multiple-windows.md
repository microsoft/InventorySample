# Multiple Windows

There has been a trend in the last years to cut down the need to pop-up new windows in modern applications. This has to do with the huge grow of mobile applications, that, due to the restricted size of most smartphone screens, had to reduce the amount of data displayed, and resorted to page navigation instead of creating new windows. Those pages occupied to whole screen, and IU flows started to be linear. 

Motivated by this new approach and the convergence between the mobile and the classic Desktop worlds, the Single Page Application paradigm was on the rise. Soon, most applications started to integrate every element of the User Interface into the same `Window`, and Pages got the main role while separate windows became less and less used to the point that their use is discouraged, favoring navigation.

Almost any application can be rethought as SPA, with little to no impact in the user experience. Navigation does a great job in the 99% of cases.

## Enterprise applications usually need more Windows

In the context of a LOB application, it's rather frequent that the user needs to switch contexts. Frequently, they need to query or edit data from different parts of the application, or mabye to hold data entry until other processes finish. There is a wide range of scenarios that would benefit from having separate windows to allow multitasking.

The Universal Windows Platform offers us the flexibility to choose when we need to give this extra multitasking facility.

## How to create a new Window

This is a small recipe method to create a new `Window` in UWP:

```csharp
public async Task<int> CreateNewViewAsync(object viewModel)
{
    int viewId = 0;

    var newView = CoreApplication.CreateNewView();
    await newView.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
    {
        viewId = ApplicationView.GetForCurrentView().Id;

        var frame = new Frame();
        `Window`.Current.Content = frame;
        `Window`.Current.Activate();
        frame.DataContext = viewModel;
    });

    if (await ApplicationViewSwitcher.TryShowAsStandaloneAsync(viewId))
    {
        return viewId;
    }

    return 0;
}
```

Basically, where are:
1. Creating the `Window` and 
2. Telling its Dispatcher to execute the setup code for the `Window`
   1. Getting the **managed `Window` Id** (`viewId`) of the `Window`
   2. Set its Content (here, a `Frame`)
   3. Activate it
   4. Setting the `DataContext` (really useful to apply MVVM)
3. Trying to show it
4. Returning the **managed `Window` Id**, that is a handle to identify the `Window` since some calls need it.

Notice that there is no reference to the word "Window" in any method, but in fact, a View in UWP in this context refers to what we commonly know as `Window`. 

So, this is all. Executing this code will get a new `Window` with its DataContext set. Of course, you can set the content of the `Window` to any `Page` to customize what is rendered inside the `Window`.

## Guidelines to Multiple Windows

- Important: Enable new windows in an app **for scenarios that enhance productivity and enable multitasking**.
- Provide a way for the user to navigate from a secondary `Window` back to the main `Window`.
- Provide a clear way for the user to open a new `Window`. For example, add a `Button` to the app bar for opening a new `Window`. 
- Make sure the title of the new `Window` reflects the contents of that `Window`. The user should be able to differentiate between the windows of an app based on the title.
- Subscribe to the [Consolidated event](https://msdn.microsoft.com/en-us/library/windows/apps/windows.ui.viewmanagement.applicationview.consolidated.aspx) and, when the event fires, close the `Window`'s contents. The consolidated event occurs when the `Window` is removed from the list of recently used apps or if the user executes a close gesture on it.
- If the new `Window` replaces the original app `Window`, provide custom animation when the windows switch.
- Design new windows that allow users to accomplish tasks entirely within the `Window`.
- Don't automatically open a new `Window` when a user navigates to a different part of the app.  
- Don't require the user to open a new `Window` to complete flows in the application.