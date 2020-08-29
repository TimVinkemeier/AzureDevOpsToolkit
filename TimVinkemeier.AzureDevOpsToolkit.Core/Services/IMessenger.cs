using System;

using MvvmCross.Plugin.Messenger;

namespace TimVinkemeier.AzureDevOpsToolkit.Core.Services
{
    public interface IMessenger
    {
        void Publish<TMessage>(TMessage message) where TMessage : MvxMessage;

        IDisposable Subscribe<TMessage>(Action<TMessage> callback) where TMessage : MvxMessage;
    }
}