using System.Windows;

using MvvmCross.Platforms.Wpf.Views;
using MvvmCross.ViewModels;

using TimVinkemeier.AzureDevOpsToolkit.Services;

namespace TimVinkemeier.AzureDevOpsToolkit.Views
{
    /// <summary>
    /// Interaction logic for ViewOutlet.xaml
    /// </summary>
    public partial class ViewOutlet : MvxWpfView, IIdentifiableViewOutlet
    {
        public ViewOutlet()
        {
            InitializeComponent();
            MultiLayoutViewPresenter.Instance.RegisterOutlet(this);
        }

        public string Identifier { get; set; }

        public void CloseView(IMvxViewModel viewModel, FrameworkElement previousStackView)
        {
            Content = previousStackView;
        }

        public void ShowView(FrameworkElement view)
        {
            Content = view;
        }
    }
}