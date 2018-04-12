# Dependency Injection

## What is Dependency Injection

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

## Advantages of using DI

There are 2 important reasons to use *DI*:

- **Unit Testing**: DI enables you to replace complex dependencies, such as databases, with mocked implementations of those dependencies. This allows you to completely isolate the code that is being testing.
- **Validation/Exception Management**: DI allows you to inject additional code between the dependencies. For example, it is possible to inject exception management logic or validation logic, which means that the developer no longer needs to write this logic for every class.

## Microsoft.Extensions.DependencyInjection

With *dotnet core* Microsoft has released a *Dependency Injection APIs* that will facilitate the integration of this pattern in the development of our apps or webs.

This is the tool we are using to add *DI* into the Van Arsdell Invetory app.

### How to use it

The first thing we need to do is add the Nuget package *Microsoft.Extensions.DependencyInjection* to our project.

This library exposes the *interface* `IServiceCollection` 

