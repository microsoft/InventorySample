# Application Navigation

In this application, views are hosted in master pages. These master pages contains a frame used to contain the view and to enable navigation between views.

There are two kind of master pages: the ShellView and the MainShellView.

## MainShellView 
The MainShellView is composed by a lateral menu, a main `Frame`, and a status bar on the bottom:

![main shell view](img/mainshellview.png)

This is the XAML representation of the view:

```xml
<Grid Background="{ThemeResource ApplicationPageBackgroundThemeBrush}">
    <Grid.RowDefinitions>
        <RowDefinition Height="*"/>
        <RowDefinition Height="Auto"/>
    </Grid.RowDefinitions>

    <NavigationView x:Name="navigationView" MenuItemsSource="{x:Bind ViewModel.Items}" MenuItemTemplate="{StaticResource NavigationViewItem}"
                    SelectedItem="{x:Bind ViewModel.SelectedItem, Mode=TwoWay}" SelectionChanged="OnSelectionChanged"
                    IsPaneOpen="{x:Bind ViewModel.IsPaneOpen, Mode=TwoWay}" AlwaysShowHeader="False">

        <Grid>
            <ProgressRing IsActive="{x:Bind ViewModel.IsBusy}" />
            <Frame x:Name="frame">
                <Frame.ContentTransitions>
                    <TransitionCollection>
                        <NavigationThemeTransition/>
                    </TransitionCollection>
                </Frame.ContentTransitions>
            </Frame>
        </Grid>
    </NavigationView>

    <Grid Grid.Row="1" Background="{ThemeResource SystemControlAccentAcrylicElementAccentMediumHighBrush}">
        <Rectangle Fill="IndianRed" Visibility="{x:Bind ViewModel.IsError, Mode=OneWay}" />
        <TextBlock Margin="6,4" Text="{x:Bind ViewModel.Message, Mode=OneWay}" Foreground="White" FontSize="12" />
    </Grid>
</Grid>
```

The `NavigationView` control represents the menu, and we can identify inside it the main Frame where our content will be changed when we select a new menú option. At the end of the main Grid we can identify the *Status Bar* where we will display the status of the user actions when interacts with the app.

## ShellView

It has the same structure of the `MainShellView` but without the Lateral Menu:

![shell view](img/shellview.png)

The XMAL markup for this shell is:

```xml
<Grid Background="{ThemeResource ApplicationPageBackgroundThemeBrush}">
    <Grid.RowDefinitions>
        <RowDefinition Height="*"/>
        <RowDefinition Height="Auto"/>
    </Grid.RowDefinitions>

    <Grid Grid.Row="0">
        <ProgressRing IsActive="{x:Bind ViewModel.IsBusy}" />
        <Frame x:Name="frame">
            <Frame.ContentTransitions>
                <TransitionCollection>
                    <NavigationThemeTransition/>
                </TransitionCollection>
            </Frame.ContentTransitions>
        </Frame>
    </Grid>

    <Grid Grid.Row="1" Background="{ThemeResource SystemControlAccentAcrylicElementAccentMediumHighBrush}">
        <Rectangle Fill="IndianRed" Visibility="{x:Bind ViewModel.IsError, Mode=OneWay}" />
        <TextBlock Margin="6,4" Text="{x:Bind ViewModel.Message, Mode=OneWay}" Foreground="White" FontSize="12" />
    </Grid>
</Grid>
```
The difference with the main shell is the lack of a `NavigationView` control. The ShellView is the master page that we will use when a new window is displayed, i.e. calling the `CreateNewViewAsync` of our *NavigationService*.

It's important to have clear the Shells that we are going to display in the app in order to understand how the *Navigation* of the app works.

## Frame Navigation

The *Main Frame* defined in the prevoius Shells is the reponsable for the navigation in the app. We can check how the Frame control is used for navigate [here](navigation-service.md#INavigationService-implementation).

When the MainShellView or the ShellView is loaded the Navigation Service has to be initialiaze in order to use the Frame as navigation control:

```csharp
private void InitializeNavigation()
{
    _navigationService = ServiceLocator.Current.GetService<INavigationService>();
    _navigationService.Initialize(frame);
    frame.Navigated += OnFrameNavigated;
    CurrentView.BackRequested += OnBackRequested;
}
```

Once the Navigation Service is initialized from the Shell views. We are ready to navigate using the `INavigationService` in our ViewModels to navigate.

## Saving the state of the view when Navigate

As you may know, in UWP apps the pages are recreated everytime we navigate to them, unless we want to preserve them in memory setting the property `NavigationCacheMode` to `Required` or `Enabled`.

With the purpose of having the best performance we can, we have decided not to *cache* the pages and save the state of them in the Navigation process. This page state will be passed as an argument when we navigate to a page, and retreived when we navigate back.

So, the first thing to do is defined an argument per ViewModel. Let's take for example the `CustomerListViewModel`. We will have a class defined for the arguments of this `CustomerListArgs`:

```csharp
public class CustomerListArgs
{
    static public CustomerListArgs CreateEmpty() => new CustomerListArgs { IsEmpty = true };

    public CustomerListArgs()
    {
        OrderBy = r => r.FirstName;
    }

    public bool IsEmpty { get; set; }

    public string Query { get; set; }

    public Expression<Func<Customer, object>> OrderBy { get; set; }
    public Expression<Func<Customer, object>> OrderByDesc { get; set; }
}
```

As we can see, we are saving the possible values of the actions that the user can do over a list.

### Passing the state as an argument in the navigation

If we have a look at the `Navigate` method implemented of the *Navigation Service*, we have a nullable `parameter` in addition of the type of the ViewModel we want to navigate to. 
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

We will pass the in this `parameter` the `CustomerListArgs` previously defined: 
```csharp
NavigationService.Navigate(viewModel, new CustomerListArgs());
```

Now we need to reflect the passed state in the navigation into the View. To accomplish that, we need to overrride the View method `OnNavigatedTo` and get the state in the `Parameter` property of the `NavigationEventArgs` received:
```csharp
protected override async void OnNavigatedTo(NavigationEventArgs e)
{
    ViewModel.Subscribe();
    await ViewModel.LoadAsync(e.Parameter as CustomerListArgs);
}
```

For those ViewModels we want to save the state, we are creating a public method in them to be called from our Views, the `LoadAsync` method. This method, just simple receive the status saved in the `CustomerListArgs` object, and load it in the ViewModel:
```csharp
public async Task LoadAsync(CustomerListArgs args)
{
    ViewModelArgs = args ?? CustomerListArgs.CreateEmpty();
    Query = args.Query;

    StartStatusMessage("Loading customers...");
    await RefreshAsync();
    EndStatusMessage("Customers loaded");
}
```

