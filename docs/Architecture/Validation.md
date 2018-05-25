# Validation

Any app that accept input from users must validate the data supplied in order to maintain data integrity and prevent the application to fail. Validation enforces business rules and prevents an attacker from injecting malicious data.

In the context of the Model-ViewModel-Model (MVVM) pattern, a view model or model will often be required to perform data validation and signal any validation errors to the view so that the user can correct them.

In this application the user can edit / add information when the detail of a *Domain Model* is displayed. Therefore, the validation rules are defined at `GenericDetailsViewModel<TModel>` base class.

The idea is to define the *Validation rules* for each property of a Detail Model. Then these validation rules will be executed before saving the data. In case of any validation rules fail, we will notify the UI the error.

## IValidationConstraint

The interface `IValidationConstraint` defines a validation over a property to be made.

```csharp
public interface IValidationConstraint<T>
{
    Func<T, bool> Validate { get; }
    string Message { get; }
}
```

On every valitation requeriement, we need to supply the function to execute and the message to return in case of error. There are several IValidationConstraint implementations already created to be used:

| Constraint | Description |
| ---------- | ----------- |
| `RequiredConstraint` | To use when a field is mandatory |
| `RequiredGreaterThanZeroConstraint` | Mandatory and greater than zero |
| `PositiveConstraint` | Greater than zero |
| `NonZeroConstraint` | Value different from zero |
| `GreaterThanConstraint` | Greater than a specific value |
| `NonGreaterThanConstraint` | Under a specific value |
| `LessThanConstraint` | Less than a specific value |

Once we know how to define validation constraints, we need to define them for a Detail ViewModel.

## Detail ViewModel validation

The validations will be made for us by the base class `GenericDetailsViewModel<TModel>`, but at least we need to implement the virtual method `GetValidationConstraints`. We will provide through this method the list of constraints of the Model. For example, in the case of a Customer, we have this implementation in the `CustomerDetailViewModel`:

```csharp
override protected IEnumerable<IValidationConstraint<CustomerModel>> GetValidationConstraints(CustomerModel model)
{
    yield return new RequiredConstraint<CustomerModel>("First Name", m => m.FirstName);
    yield return new RequiredConstraint<CustomerModel>("Last Name", m => m.LastName);
    yield return new RequiredConstraint<CustomerModel>("Email Address", m => m.EmailAddress);
    yield return new RequiredConstraint<CustomerModel>("Address Line 1", m => m.AddressLine1);
    yield return new RequiredConstraint<CustomerModel>("City", m => m.City);
    yield return new RequiredConstraint<CustomerModel>("Region", m => m.Region);
    yield return new RequiredConstraint<CustomerModel>("Postal Code", m => m.PostalCode);
    yield return new RequiredConstraint<CustomerModel>("Country", m => m.CountryCode);
}
```

Then the `GenericDetailViewModel` class will execute the Save action like so:

```csharp
public ICommand SaveCommand => new RelayCommand(OnSave);
virtual protected async void OnSave()
{
    StatusReady();
    var result = Validate(EditableItem);
    if (result.IsOk)
    {
        await SaveAsync();
    }
    else
    {
        await DialogService.ShowAsync(result.Message, $"{result.Description} Please, correct the error and try again.");
    }
}
```
And the `Validate` function will be one to check the validation constraints of the Model:

```csharp
virtual public Result Validate(TModel model)
{
    foreach (var constraint in GetValidationConstraints(model))
    {
        if (!constraint.Validate(model))
        {
            return Result.Error("Validation Error", constraint.Message);
        }
    }
    return Result.Ok();
}
```

## Summary

We have seen how simple it's to provide the validation contraints of our Models just overriding the function `GetValidationConstraints` for all the ViewModels inheriting from `GenericDetailsViewModel`.
