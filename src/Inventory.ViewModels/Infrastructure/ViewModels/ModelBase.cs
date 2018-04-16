using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.ComponentModel;

namespace Inventory.Models
{
    public class ModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public bool IsEmpty { get; set; }

        public ModelBase Clone()
        {
            return MemberwiseClone() as ModelBase;
        }

        virtual public void Merge(ModelBase source) { }

        protected bool Set<T>(ref T field, T newValue = default(T), [CallerMemberName] string propertyName = null)
        {
            if (!EqualityComparer<T>.Default.Equals(field, newValue))
            {
                field = newValue;
                NotifyPropertyChanged(propertyName);
                return true;
            }
            return false;
        }

        public void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void NotifyChanges()
        {
            // Notify all properties
            NotifyPropertyChanged("");
        }
    }
}
