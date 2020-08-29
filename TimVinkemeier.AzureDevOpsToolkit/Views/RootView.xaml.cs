using MvvmCross.Platforms.Wpf.Views;
using MvvmCross.ViewModels;

using TimVinkemeier.AzureDevOpsToolkit.Core.ViewModels;

namespace TimVinkemeier.AzureDevOpsToolkit.Views
{
    /// <summary>
    /// Interaction logic for RootView.xaml
    /// </summary>
    [MvxViewFor(typeof(RootViewModel))]
    public partial class RootView : MvxWpfView
    {
        public RootView()
        {
            InitializeComponent();
        }
    }
}