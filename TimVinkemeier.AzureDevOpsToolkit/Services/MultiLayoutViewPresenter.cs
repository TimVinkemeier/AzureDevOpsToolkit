using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

using MvvmCross.Platforms.Wpf.Presenters;
using MvvmCross.Platforms.Wpf.Views;
using MvvmCross.Presenters;
using MvvmCross.ViewModels;

using TimVinkemeier.AzureDevOpsToolkit.Core.Services;
using TimVinkemeier.AzureDevOpsToolkit.Views;

namespace TimVinkemeier.AzureDevOpsToolkit.Services
{
    public class MultiLayoutViewPresenter : MvxWpfViewPresenter
    {
        private readonly ContentControl _contentControl;

        public MultiLayoutViewPresenter(ContentControl contentControl)
        {
            _contentControl = contentControl;
            FrameworkElementsDictionary.Add(contentControl, new Stack<FrameworkElement>());
            Instance = this;
        }

        internal static MultiLayoutViewPresenter Instance { get; private set; }

        public override void RegisterAttributeTypes()
        {
            base.RegisterAttributeTypes();
            AttributeTypesToActionsDictionary.Add(typeof(OutletBasedPresentationAttribute), new MvxPresentationAttributeAction
            {
                ShowAction = (viewType, attribute, request) =>
                {
                    var view = WpfViewLoader.CreateView(request);
                    return ShowInLayoutView(view, (OutletBasedPresentationAttribute)attribute, request);
                },
                CloseAction = (viewModel, attribute) => CloseLayoutView(viewModel, (OutletBasedPresentationAttribute)attribute)
            });
        }

        internal void RegisterOutlet(ContentControl outlet)
        {
            FrameworkElementsDictionary.Add(outlet, new Stack<FrameworkElement>());
        }

        private Task<bool> CloseLayoutView(IMvxViewModel viewModel, OutletBasedPresentationAttribute attribute)
        {
            var item = FrameworkElementsDictionary.FirstOrDefault(i => i.Value.Count > 0 && (i.Value.Peek() as IMvxWpfView)?.ViewModel == viewModel);
            var viewOutlet = item.Key as IIdentifiableViewOutlet;
            var elements = item.Value;

            if (elements.Count > 0)
                elements.Pop(); // Pop closing view

            if (elements.Count > 0)
            {
                viewOutlet.CloseView(viewModel, elements.Peek());
                return Task.FromResult(true);
            }

            return Task.FromResult(false);
        }

        private Task<bool> ShowInLayoutView(FrameworkElement view, OutletBasedPresentationAttribute attribute, MvxViewModelRequest request)
        {
            var viewOutlet = FrameworkElementsDictionary.Keys.FirstOrDefault(w => (w as IIdentifiableViewOutlet)?.Identifier == attribute.OutletIdentifier) as IIdentifiableViewOutlet;

            if (viewOutlet is null)
            {
                throw new ArgumentException($"Could not find view outlet with identifier '{attribute.OutletIdentifier}'.");
            }

            FrameworkElementsDictionary[viewOutlet as ContentControl].Push(view);
            viewOutlet.ShowView(view);
            return Task.FromResult(true);
        }
    }
}