using System;

namespace Inventory.Services
{
    public interface IMessageService
    {
        void Subscribe<TSender>(object target, Action<TSender, string, object> action) where TSender : class;
        void Subscribe<TSender, TArgs>(object target, Action<TSender, string, TArgs> action) where TSender : class;

        void Unsubscribe(object target);
        void Unsubscribe<TSender>(object target) where TSender : class;
        void Unsubscribe<TSender, TArgs>(object target) where TSender : class;

        void Send<TSender, TArgs>(TSender sender, string message, TArgs args) where TSender : class;
    }
}
