using System.Threading.Tasks;

using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

using TimVinkemeier.AzureDevOpsToolkit.Core.ViewModels.SearchAndReplace;
using TimVinkemeier.AzureDevOpsToolkit.Core.ViewModels.Settings;

namespace TimVinkemeier.AzureDevOpsToolkit.Core.ViewModels.Menu
{
    public class MainMenuViewModel : MvxNavigationViewModel
    {
        public MainMenuViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService)
            : base(logProvider, navigationService)
        {
            NavigateCommand = new MvxCommand<MainMenuItemViewModel>(vm => NavigationService.Navigate(vm.TargetViewModelType));
        }

        public MvxObservableCollection<MainMenuItemViewModel> Items { get; } = new MvxObservableCollection<MainMenuItemViewModel>();

        public IMvxCommand<MainMenuItemViewModel> NavigateCommand { get; }

        public MvxObservableCollection<MainMenuItemViewModel> SecondaryItems { get; } = new MvxObservableCollection<MainMenuItemViewModel>();

        public override Task Initialize()
        {
            Items.Add(new MainMenuItemViewModel(LogProvider, NavigationService)
            {
                DisplayName = "Find and Replace",
                TargetViewModelType = typeof(SearchAndReplaceViewModel)
            });

            SecondaryItems.Add(new MainMenuItemViewModel(LogProvider, NavigationService)
            {
                DisplayName = "Settings",
                TargetViewModelType = typeof(SettingsViewModel)
            });

            return Task.CompletedTask;
        }
    }
}