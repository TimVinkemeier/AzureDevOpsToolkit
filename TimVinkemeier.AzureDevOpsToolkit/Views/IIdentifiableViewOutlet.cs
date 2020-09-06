using System.Windows;

using MvvmCross.Platforms.Wpf.Views;
using MvvmCross.ViewModels;

namespace TimVinkemeier.AzureDevOpsToolkit.Views
{
    public interface IIdentifiableViewOutlet : IMvxWpfView
    {
        string Identifier { get; }

        void CloseView(IMvxViewModel viewModel, FrameworkElement previousStackView);

        void ShowView(FrameworkElement view);
    }
}