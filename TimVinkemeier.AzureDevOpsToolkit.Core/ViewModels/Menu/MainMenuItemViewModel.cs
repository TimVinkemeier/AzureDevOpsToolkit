using System;

using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace TimVinkemeier.AzureDevOpsToolkit.Core.ViewModels.Menu
{
    public class MainMenuItemViewModel : MvxNavigationViewModel
    {
        private bool _isSelected;

        public MainMenuItemViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService)
            : base(logProvider, navigationService)
        {
            NavigationService.BeforeNavigate += OnNavigating;
        }

        public string DisplayName { get; set; }

        public bool IsSelected
        {
            get => _isSelected;
            set => SetProperty(ref _isSelected, value);
        }

        public Type TargetViewModelType { get; set; }

        private void OnNavigating(object sender, MvvmCross.Navigation.EventArguments.IMvxNavigateEventArgs e)
        {
            IsSelected = TargetViewModelType == e.ViewModel.GetType();
        }
    }
}