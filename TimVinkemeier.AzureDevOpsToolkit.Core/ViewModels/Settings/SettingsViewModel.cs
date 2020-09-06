using System;
using System.Threading.Tasks;

using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

using TimVinkemeier.AzureDevOpsToolkit.Core.Services;

namespace TimVinkemeier.AzureDevOpsToolkit.Core.ViewModels.Settings
{
    public class SettingsViewModel : ContentViewBaseViewModel
    {
        private readonly ISettingsService _settingsService;
        private readonly IWorkItemService _workItemService;
        private string _baseUrl;
        private bool _hasChanges = false;
        private string _projectName;
        private string _saveErrorMessage;
        private string _token;

        public SettingsViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService, ISettingsService settingsService, IWorkItemService workItemService)
            : base(logProvider, navigationService)
        {
            _settingsService = settingsService;
            SaveCommand = new MvxAsyncCommand(SaveSettingsAsync, () => _hasChanges);
            _workItemService = workItemService;
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

        public string SaveErrorMessage
        {
            get => _saveErrorMessage;
            set => SetProperty(ref _saveErrorMessage, value);
        }

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
            SaveErrorMessage = null;

            try
            {
                BusyTask = MvxNotifyTask.Create(_workItemService.TestSettingsAsync(BaseUrl, ProjectName, Token));
                await BusyTask.Task.ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                SaveErrorMessage = ex.Message;
                _hasChanges = false;
                SaveCommand?.RaiseCanExecuteChanged();
                return;
            }

            BusyTask = MvxNotifyTask.Create(Task.WhenAll(
                _settingsService.SetSettingAsync(Setting.AzureDevOpsToken, Token),
                _settingsService.SetSettingAsync(Setting.OrganisationBaseUrl, BaseUrl),
                _settingsService.SetSettingAsync(Setting.ProjectName, ProjectName)));

            await BusyTask.Task.ConfigureAwait(false);
            _hasChanges = false;
            SaveCommand.RaiseCanExecuteChanged();
        }
    }
}