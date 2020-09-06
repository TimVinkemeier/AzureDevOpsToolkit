using MvvmCross.Platforms.Wpf.Views;
using MvvmCross.ViewModels;

using TimVinkemeier.AzureDevOpsToolkit.Core.Services;
using TimVinkemeier.AzureDevOpsToolkit.Core.ViewModels.SearchAndReplace;

namespace TimVinkemeier.AzureDevOpsToolkit.Views
{
    /// <summary>
    /// Interaction logic for SearchAndReplaceView.xaml
    /// </summary>
    [OutletBasedPresentation(OutletIdentifier = "RouterOutlet")]
    [MvxViewFor(typeof(SearchAndReplaceViewModel))]
    public partial class SearchAndReplaceView : MvxWpfView
    {
        public SearchAndReplaceView()
        {
            InitializeComponent();
        }
    }
}