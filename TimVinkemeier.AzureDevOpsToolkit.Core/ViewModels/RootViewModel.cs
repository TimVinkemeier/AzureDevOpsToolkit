using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

using TimVinkemeier.AzureDevOpsToolkit.Core.ViewModels.SearchAndReplace;
using TimVinkemeier.AzureDevOpsToolkit.Core.ViewModels.Settings;

namespace TimVinkemeier.AzureDevOpsToolkit.Core.ViewModels
{
    public class RootViewModel : MvxNavigationViewModel
    {
        public RootViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService)
            : base(logProvider, navigationService)
        {
            GoToSearchAndReplaceCommand = new MvxAsyncCommand(() => NavigationService.Navigate<SearchAndReplaceViewModel>());

            GoToSettingsCommand = new MvxAsyncCommand(() => NavigationService.Navigate<SettingsViewModel>());
        }

        public IMvxAsyncCommand GoToSearchAndReplaceCommand { get; }

        public IMvxAsyncCommand GoToSettingsCommand { get; }
    }
}