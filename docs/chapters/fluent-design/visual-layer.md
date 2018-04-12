# Visual Layer

## What's Visual Layer?

The Visual Layer is a layer between the Windows UWP XAML layer and DirectX that allows indirect access to drawing primitives. This gives developers great flexibility when it comes to rendering custom controls that require complex render features or animations, while keeping the same philosophy that XAML provides in terms of simplicity and high-level APIs.

## Visual Layer

In order to work with the Visual Layer we have to get a Visual. A Visual is an abstraction of anything that can be rendered on the screen. Every UIElement has an underlying Visual that can be retrieved using a helper class from the `Windows.UI.Composition` namespace, called `ElementCompositionPreview`.

For instance, given a Canvas in a XAML view (the class Canvas control) like this:

```xml
<Canvas x:Name="Canvas">
```

We can get its visual calling

```csharp
Visual hostVisual = ElementCompositionPreview.GetElementVisual(Canvas);
```

In order to be able to create a composition to render on the Canvas, we must then create a `Composer`. The `Composer` class is a factory that will construct the objects that will take part into a composition.

How do we get the Composer? From the Compositor property of the Visual.

```csharp
Compositor compositor = hostVisual.Compositor;
```

Once we get it, we can add figures to the composition using methods like `Compositor.CreateSpriteVisual()`

For example, in this code, we're creating a rotated rectangle:
  
```csharp
 var rectangle = compositor.CreateSpriteVisual();

rectangle.Size = new Vector2(6.0f, 5F);
rectangle.Brush = compositor.CreateColorBrush(Colors.Blue);
rectangle.Offset = new Vector3(100f, 0.0f, 0);
rectangle.CenterPoint = new Vector3(3.0f, 100.0f, 0);
rectangle.RotationAngleInDegrees = 45;

// Add the shadow as a child of the host in the visual tree
ElementCompositionPreview.SetElementChildVisual(Canvas, rectangle);     
```

Notice the last line. It adds the composed rectangle as the last child of the element. This way, the rectangle will be rendered over every other element that the `Canvas` would have.

This is a pretty basic usage of the Visual Layer that will let you understand what's going on under the hood.

## Advanced scenarios

Compositions can be are complex as you wish. 

There are other features like
- Animations
- Effects
- Spotlights
- Transparencies
- Brushes

You can refer to the [Visual Layer Documentation](https://docs.microsoft.com/en-us/windows/uwp/composition/visual-layer) to get a full specification of the Composition API.