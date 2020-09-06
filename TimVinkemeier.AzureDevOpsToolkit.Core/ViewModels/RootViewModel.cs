using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

using TimVinkemeier.AzureDevOpsToolkit.Core.ViewModels.Menu;

namespace TimVinkemeier.AzureDevOpsToolkit.Core.ViewModels
{
    public class RootViewModel : MvxNavigationViewModel
    {
        public RootViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService)
            : base(logProvider, navigationService)
        {
        }

        public override void ViewAppearing()
        {
            NavigationService.Navigate<MainMenuViewModel>();
            NavigationService.Navigate<StartPageViewModel>();
        }
    }
}