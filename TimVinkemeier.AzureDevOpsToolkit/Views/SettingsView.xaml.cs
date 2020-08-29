using MvvmCross.Platforms.Wpf.Presenters.Attributes;
using MvvmCross.Platforms.Wpf.Views;

namespace TimVinkemeier.AzureDevOpsToolkit.Views
{
    /// <summary>
    /// Interaction logic for SettingsView.xaml
    /// </summary>
    [MvxContentPresentation]
    public partial class SettingsView : MvxWpfView
    {
        public SettingsView()
        {
            InitializeComponent();
        }
    }
}