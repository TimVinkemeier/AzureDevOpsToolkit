using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace TimVinkemeier.AzureDevOpsToolkit.Core.ViewModels
{
    public abstract class ContentViewBaseViewModel : MvxNavigationViewModel
    {
        protected ContentViewBaseViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService)
            : base(logProvider, navigationService)
        {
            BackToRootCommand = new MvxAsyncCommand(() => NavigationService.Navigate<RootViewModel>());
        }

        public IMvxAsyncCommand BackToRootCommand { get; }
    }
}