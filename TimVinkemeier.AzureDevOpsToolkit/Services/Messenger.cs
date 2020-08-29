using System;
using System.Collections.Concurrent;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Windows.Threading;

using MvvmCross.Plugin.Messenger;

using TimVinkemeier.AzureDevOpsToolkit.Core.Services;

namespace TimVinkemeier.AzureDevOpsToolkit.Services
{
    public class Messenger : IMessenger
    {
        private readonly ConcurrentDictionary<Type, object> _subjects = new ConcurrentDictionary<Type, object>();

        public void Publish<TMessage>(TMessage message) where TMessage : MvxMessage
        {
            var subject = _subjects.GetOrAdd(typeof(TMessage), _ => new Subject<TMessage>());
            (subject as IObserver<TMessage>).OnNext(message);
        }

        public IDisposable Subscribe<TMessage>(Action<TMessage> callback) where TMessage : MvxMessage
        {
            var subject = _subjects.GetOrAdd(typeof(TMessage), _ => new Subject<TMessage>());
            return (subject as IObservable<TMessage>).Subscribe(m => Dispatcher.CurrentDispatcher.Invoke(() => callback(m)), ex => throw new Exception("ERROR", ex), () => throw new Exception("COMPLETED"));
        }
    }
}