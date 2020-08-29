using System.Threading.Tasks;

using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.Navigation;

using TimVinkemeier.AzureDevOpsToolkit.Core.Services;

namespace TimVinkemeier.AzureDevOpsToolkit.Core.ViewModels.Settings
{
    public class SettingsViewModel : ContentViewBaseViewModel
    {
        private readonly ISettingsService _settingsService;
        private string _baseUrl;
        private bool _hasChanges = false;
        private string _projectName;
        private string _token;

        public SettingsViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService, ISettingsService settingsService)
                    : base(logProvider, navigationService)
        {
            _settingsService = settingsService;
            SaveCommand = new MvxAsyncCommand(SaveSettingsAsync, () => _hasChanges);
        }

        public string BaseUrl
        {
            get => _baseUrl;
            set
            {
                SetProperty(ref _baseUrl, value);
                _hasChanges = true;
                SaveCommand.RaiseCanExecuteChanged();
            }
        }

        public string ProjectName
        {
            get => _projectName;
            set
            {
                SetProperty(ref _projectName, value);
                _hasChanges = true;
                SaveCommand.RaiseCanExecuteChanged();
            }
        }

        public MvxAsyncCommand SaveCommand { get; }

        public string Token
        {
            get => _token;
            set
            {
                SetProperty(ref _token, value);
                _hasChanges = true;
                SaveCommand.RaiseCanExecuteChanged();
            }
        }

        public override async Task Initialize()
        {
            _token = await _settingsService.GetSettingAsync<string>(Setting.AzureDevOpsToken).ConfigureAwait(false);
            _baseUrl = await _settingsService.GetSettingAsync<string>(Setting.OrganisationBaseUrl).ConfigureAwait(false);
            _projectName = await _settingsService.GetSettingAsync<string>(Setting.ProjectName).ConfigureAwait(false);
        }

        private async Task SaveSettingsAsync()
        {
            await _settingsService.SetSettingAsync(Setting.AzureDevOpsToken, Token).ConfigureAwait(false);
            await _settingsService.SetSettingAsync(Setting.OrganisationBaseUrl, BaseUrl).ConfigureAwait(false);
            await _settingsService.SetSettingAsync(Setting.ProjectName, ProjectName).ConfigureAwait(false);
            _hasChanges = false;
            SaveCommand.RaiseCanExecuteChanged();
        }
    }
}