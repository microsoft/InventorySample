# Responsive Design

UWP applications can run on a variety of form factors, from tablets to PCs, even inside IoT devices (like Windows IoT Core). Thus, interfaces should take this into account, as part of the Fluent Design principles.

Users can run your application in a 8-inch display on a Tablet, or inside a multi-display configuration using monitors of 27 inches.

Moreover, the user can decide to rotate the device, switching from portrait to landscape and your UI should be usable and adapt smoothly.

You can take as reference the ever rising number of web sites that depending on the device where you are browsing, show one or another layout, that could be completely diferrent, or just the same, with the same elements, but with arranged in a completely different way.

To help users design the UI, the Universal Windows Platform comes with handy mechanisms that helps developers adapt their user interfaces to any situation.

## Adaptive Triggers
This mechanism has been part of UWP from the very beginning. You may already know them, but in case you don't, here you can find an introduction a few samples that you'll get you ready to use them.

### Triggers
A trigger is a declarative rule that will connect a cause with its consequence. In our case, the cause will be a special condition that, when it met, will *trigger* a change.

In the case of Adaptive Triggers, the condition can be that a given Window size has been reached. The consequence can be that a button can be repositioned to another column/row in a grid, for instance.

Let's illustrate it with some code:

```xml
<VisualState x:Name="Big">
	<VisualState.StateTriggers>
		<AdaptiveTrigger MinWindowWidth="600"/>  
	</VisualState.StateTriggers> 
	<VisualState.Setters> 
		<Setter Target="Button1.(Grid.Row)" Value="2"/> 
	</VisualState.Setters> 
</VisualState>
```

This will make the `Button` called "Button1" be inside the Row 2 of its hosting grid **while the Window is 600 DIPs or wider**. 

(**DIP**, Device Independent, is a measurement unit)

Using adaptive triggers, you could perform transformations in a UI to adapt to any form factor.

One common way to do it is using a Grid using columns for the elements in the view when it's landscape and rows when in portrait.

Here a complete example:

```xml
<Page
    x:Class="Composi.MainPage"
    xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
    xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
    xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
    mc:Ignorable="d">


    <Grid>

        <VisualStateManager.VisualStateGroups>
            <VisualStateGroup>
                <VisualState x:Name="Lanscape">
                    <VisualState.StateTriggers>
                        <AdaptiveTrigger MinWindowWidth="600" />
                    </VisualState.StateTriggers>

                    <VisualState.Setters>
                        <Setter Target="Border2.(Grid.Row)" Value="0" />
                        <Setter Target="Border2.(Grid.Column)" Value="1" />
                        <Setter Target="ColumnDefinition.Width" Value="*" />
                        <Setter Target="RowDefinition.Height" Value="Auto" />
                    </VisualState.Setters>

                </VisualState>

            </VisualStateGroup>
        </VisualStateManager.VisualStateGroups>

        <Grid.ColumnDefinitions>
            <ColumnDefinition />
            <ColumnDefinition x:Name="ColumnDefinition" Width="Auto" />
        </Grid.ColumnDefinitions>

        <Grid.RowDefinitions>
            <RowDefinition />
            <RowDefinition x:Name="RowDefinition" />
        </Grid.RowDefinitions>

        <Border x:Name="Border1" Background="LawnGreen" />
        <Border x:Name="Border2" Grid.Row="1" Grid.Column="0" Background="Green" />
    </Grid>
</Page>
```

There is a single Adaptive Trigger in the Page that will active when the Width of the page is 600 or wider.

Upon this change, the Border called "Border2" will switch from row 1 to row 0, and from column 0, to column 1. Also, The width of the second column definition and row definition are switched values (from "Auto" to "*" and viceversa), so they will collapse automatically (otherwise, there would be an empty espace in both rows and columns (50% of the space, to be exact).

As said before, this is a common technique, but it's not always necessary, because there a controls that encapsulate this adaptive behavior themselves.

For instance, we have the NavigationView, that adapts itself to the available space, hiding or showing the navigatable side panel.

### Master-Detail

In other scenarios, like master-detail scenarios, it's common to show both views side by side when there is enough space, and to hide one of them when there isn't. In this case we should handle the use of the back button, or create a way to go back from the details view to the master view.

In these scenarios we have to keep track of the current selected item. 

A common way to do it is checking whether the selected item in the master list is null. If this happens, the detail view should have `Visibility=Collapsed`. When the user selected one item, the Detail should be turned Visible (`Visibility=Visible`) and the Back Button should be shown (so the user can go back).

This situation is so common that there are already controls that encapsulate the whole behavior, like the [MasterDetailsView](https://docs.microsoft.com/es-es/windows/uwpcommunitytoolkit/controls/masterdetailsview) of [UWP Community Toolkit](https://github.com/Microsoft/UWPCommunityToolkit) 