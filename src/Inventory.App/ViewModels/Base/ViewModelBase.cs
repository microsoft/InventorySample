using System;
using System.Linq;
using System.Collections.Generic;

using Windows.ApplicationModel.Core;

namespace Inventory.ViewModels
{
    public class ViewModelBase : ModelBase
    {
        public bool IsMainView => CoreApplication.GetCurrentView().IsMain;

        virtual public string Title => String.Empty;
    }

    public class ViewModelBase<T> : ViewModelBase where T : ModelBase
    {
        virtual protected IEnumerable<IValidationConstraint<T>> ValidationConstraints => Enumerable.Empty<IValidationConstraint<T>>();

        public Result Validate(T model)
        {
            foreach (var constraint in ValidationConstraints)
            {
                if (!constraint.Validate(model))
                {
                    return Result.Error("Validation Error", $"{constraint.Message} Please, correct the error and try again.");
                }
            }
            return Result.Ok();
        }
    }
}
