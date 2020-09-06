using MvvmCross.Platforms.Wpf.Views;
using MvvmCross.ViewModels;

using TimVinkemeier.AzureDevOpsToolkit.Core.Services;
using TimVinkemeier.AzureDevOpsToolkit.Core.ViewModels;

namespace TimVinkemeier.AzureDevOpsToolkit.Views
{
    /// <summary>
    /// Interaction logic for StartPageView.xaml
    /// </summary>
    [MvxViewFor(typeof(StartPageViewModel))]
    [OutletBasedPresentation(OutletIdentifier = "RouterOutlet")]
    public partial class StartPageView : MvxWpfView
    {
        public StartPageView()
        {
            InitializeComponent();
        }
    }
}