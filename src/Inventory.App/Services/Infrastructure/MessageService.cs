#region copyright
// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************
#endregion

using System;
using System.Linq;
using System.Collections.Generic;

namespace Inventory.Services
{
    public class MessageService : IMessageService
    {
        private object _sync = new Object();

        private List<Subscriber> _subscribers = new List<Subscriber>();

        public void Subscribe<TSender>(object target, Action<TSender, string, object> action) where TSender : class
        {
            Subscribe<TSender, Object>(target, action);
        }
        public void Subscribe<TSender, TArgs>(object target, Action<TSender, string, TArgs> action) where TSender : class
        {
            if (target == null)
                throw new ArgumentNullException(nameof(target));
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            lock (_sync)
            {
                var subscriber = _subscribers.Where(r => r.Target == target).FirstOrDefault();
                if (subscriber == null)
                {
                    subscriber = new Subscriber(target);
                    _subscribers.Add(subscriber);
                }
                subscriber.AddSubscription<TSender, TArgs>(action);
            }
        }

        public void Unsubscribe<TSender>(object target) where TSender : class
        {
            if (target == null)
                throw new ArgumentNullException(nameof(target));

            lock (_sync)
            {
                var subscriber = _subscribers.Where(r => r.Target == target).FirstOrDefault();
                if (subscriber != null)
                {
                    subscriber.RemoveSubscription<TSender>();
                    if (subscriber.IsEmpty)
                    {
                        _subscribers.Remove(subscriber);
                    }
                }
            }
        }
        public void Unsubscribe<TSender, TArgs>(object target) where TSender : class
        {
            if (target == null)
                throw new ArgumentNullException(nameof(target));

            lock (_sync)
            {
                var subscriber = _subscribers.Where(r => r.Target == target).FirstOrDefault();
                if (subscriber != null)
                {
                    subscriber.RemoveSubscription<TSender, TArgs>();
                    if (subscriber.IsEmpty)
                    {
                        _subscribers.Remove(subscriber);
                    }
                }
            }
        }
        public void Unsubscribe(object target)
        {
            if (target == null)
                throw new ArgumentNullException(nameof(target));

            lock (_sync)
            {
                var subscriber = _subscribers.Where(r => r.Target == target).FirstOrDefault();
                if (subscriber != null)
                {
                    _subscribers.Remove(subscriber);
                }
            }
        }

        public void Send<TSender, TArgs>(TSender sender, string message, TArgs args) where TSender : class
        {
            if (sender == null)
                throw new ArgumentNullException(nameof(sender));

            foreach (var subscriber in GetSubscribersSnapshot())
            {
                // Avoid sending message to self
                if (subscriber.Target != sender)
                {
                    subscriber.TryInvoke(sender, message, args);
                }
            }
        }

        private Subscriber[] GetSubscribersSnapshot()
        {
            lock (_sync)
            {
                return _subscribers.ToArray();
            }
        }

        class Subscriber
        {
            private WeakReference _reference = null;

            private Dictionary<Type, Subscriptions> _subscriptions;

            public Subscriber(object target)
            {
                _reference = new WeakReference(target);
                _subscriptions = new Dictionary<Type, Subscriptions>();
            }

            public object Target => _reference.Target;

            public bool IsEmpty => _subscriptions.Count == 0;

            public void AddSubscription<TSender, TArgs>(Action<TSender, string, TArgs> action)
            {
                if (!_subscriptions.TryGetValue(typeof(TSender), out Subscriptions subscriptions))
                {
                    subscriptions = new Subscriptions();
                    _subscriptions.Add(typeof(TSender), subscriptions);
                }
                subscriptions.AddSubscription(action);
            }

            public void RemoveSubscription<TSender>()
            {
                _subscriptions.Remove(typeof(TSender));
            }
            public void RemoveSubscription<TSender, TArgs>()
            {
                if (_subscriptions.TryGetValue(typeof(TSender), out Subscriptions subscriptions))
                {
                    subscriptions.RemoveSubscription<TArgs>();
                    if (subscriptions.IsEmpty)
                    {
                        _subscriptions.Remove(typeof(TSender));
                    }
                }
            }

            public void TryInvoke<TArgs>(object sender, string message, TArgs args)
            {
                var target = _reference.Target;
                if (_reference.IsAlive)
                {
                    var senderType = sender.GetType();
                    foreach (var keyValue in _subscriptions.Where(r => r.Key.IsAssignableFrom(senderType)))
                    {
                        var subscriptions = keyValue.Value;
                        subscriptions.TryInvoke(sender, message, args);
                    }
                }
            }
        }

        class Subscriptions
        {
            private Dictionary<Type, Delegate> _subscriptions = null;

            public Subscriptions()
            {
                _subscriptions = new Dictionary<Type, Delegate>();
            }

            public bool IsEmpty => _subscriptions.Count == 0;

            public void AddSubscription<TSender, TArgs>(Action<TSender, string, TArgs> action)
            {
                _subscriptions.Add(typeof(TArgs), action);
            }

            public void RemoveSubscription<TArgs>()
            {
                _subscriptions.Remove(typeof(TArgs));
            }

            public void TryInvoke<TArgs>(object sender, string message, TArgs args)
            {
                var argsType = typeof(TArgs);
                foreach (var keyValue in _subscriptions.Where(r => r.Key.IsAssignableFrom(argsType)))
                {
                    var action = keyValue.Value;
                    action?.DynamicInvoke(sender, message, args);
                }
            }
        }
    }
}
