using System.Threading.Tasks;

namespace TimVinkemeier.AzureDevOpsToolkit.Core.Services
{
    public interface ISettingsService
    {
        Task<T> GetSettingAsync<T>(Setting setting);

        Task SetSettingAsync<T>(Setting setting, T value);
    }
}