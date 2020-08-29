using MvvmCross;
using MvvmCross.Platforms.Wpf.Core;

using TimVinkemeier.AzureDevOpsToolkit.Core.Services;
using TimVinkemeier.AzureDevOpsToolkit.Services;

namespace TimVinkemeier.AzureDevOpsToolkit
{
    public class Setup : MvxWpfSetup<Core.App>
    {
        protected override void InitializeFirstChance()
        {
            base.InitializeFirstChance();

            Mvx.IoCProvider.RegisterType<ISettingsService, SettingsService>();
            Mvx.IoCProvider.RegisterSingleton<IMessenger>(new Messenger());
        }
    }
}