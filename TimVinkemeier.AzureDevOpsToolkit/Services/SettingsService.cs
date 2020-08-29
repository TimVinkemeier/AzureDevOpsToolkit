using System;
using System.Threading.Tasks;

using Newtonsoft.Json;

using TimVinkemeier.AzureDevOpsToolkit.Core.Services;

namespace TimVinkemeier.AzureDevOpsToolkit.Services
{
    public class SettingsService : ISettingsService
    {
        public Task<T> GetSettingAsync<T>(Setting setting)
        {
            var value = setting switch
            {
                Setting.AzureDevOpsToken => Properties.Settings.Default.AzureDevOpsToken,
                Setting.OrganisationBaseUrl => Properties.Settings.Default.OrganisationBaseUrl,
                Setting.ProjectName => Properties.Settings.Default.ProjectName,
                _ => throw new ArgumentException(null, nameof(setting))
            };

            return Task.FromResult(JsonConvert.DeserializeObject<T>(value));
        }

        public Task SetSettingAsync<T>(Setting setting, T value)
        {
            switch (setting)
            {
                case Setting.AzureDevOpsToken:
                    Properties.Settings.Default.AzureDevOpsToken = JsonConvert.SerializeObject(value);
                    break;

                case Setting.OrganisationBaseUrl:
                    Properties.Settings.Default.OrganisationBaseUrl = JsonConvert.SerializeObject(value);
                    break;

                case Setting.ProjectName:
                    Properties.Settings.Default.ProjectName = JsonConvert.SerializeObject(value);
                    break;

                default:
                    throw new ArgumentException(null, nameof(setting));
            };

            Properties.Settings.Default.Save();

            return Task.CompletedTask;
        }
    }
}