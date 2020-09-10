using Microsoft.VisualStudio.Services.Common;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

using TimVinkemeier.AzureDevOpsToolkit.Core.Services;

namespace TimVinkemeier.AzureDevOpsToolkit.Services
{
    public class SettingsService : ISettingsService
    {
        private IDictionary<string, string> _allSettings;

        public async Task<T> GetSettingAsync<T>(Setting setting)
        {
            if (_allSettings is null)
            {
                await ReadSettingsAsync().ConfigureAwait(false);
            }

            var key = Enum.GetName(setting);
            return _allSettings.TryGetValue(key, out var value)
                ? JsonConvert.DeserializeObject<T>(value)
                : default;
        }

        public async Task SetSettingAsync<T>(Setting setting, T value)
        {
            await ReadSettingsAsync().ConfigureAwait(false);

            var key = Enum.GetName(setting);
            var json = JsonConvert.SerializeObject(value);
            _allSettings.AddOrUpdate(key, json, (_, _) => json);

            await SaveSettingsAsync().ConfigureAwait(false);
            await ReadSettingsAsync().ConfigureAwait(false);
        }

        private static FileInfo GetSettingsFileInfo()
            => new FileInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "TimVinkemeier.AzureDevOps", "settings.json"));

        private async Task ReadSettingsAsync()
        {
            var file = GetSettingsFileInfo();

            if (!file.Exists)
            {
                _allSettings = new Dictionary<string, string>();
                await SaveSettingsAsync().ConfigureAwait(false);
                return;
            }

            var fullJson = await File.ReadAllTextAsync(file.FullName).ConfigureAwait(false);
            _allSettings = JsonConvert.DeserializeObject<IDictionary<string, string>>(fullJson);
        }

        private async Task SaveSettingsAsync()
        {
            var file = GetSettingsFileInfo();

            Directory.CreateDirectory(file.DirectoryName);

            var fullJson = JsonConvert.SerializeObject(_allSettings);
            await File.WriteAllTextAsync(file.FullName, fullJson).ConfigureAwait(false);
        }
    }
}