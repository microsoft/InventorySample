using System;
using System.ComponentModel;

namespace Inventory
{
    public interface INotifyExpressionChanged : INotifyPropertyChanged
    {
        void NotifyPropertyChanged(string propertyName);
    }
}
