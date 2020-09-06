using MvvmCross.Platforms.Wpf.Views;

using TimVinkemeier.AzureDevOpsToolkit.Core.Services;

namespace TimVinkemeier.AzureDevOpsToolkit.Views
{
    /// <summary>
    /// Interaction logic for MainMenuView.xaml
    /// </summary>
    [OutletBasedPresentation(OutletIdentifier = "MainMenuOutlet")]
    public partial class MainMenuView : MvxWpfView
    {
        public MainMenuView()
        {
            InitializeComponent();
        }
    }
}