using System.Windows.Controls;

using MvvmCross;
using MvvmCross.Platforms.Wpf.Core;
using MvvmCross.Platforms.Wpf.Presenters;

using TimVinkemeier.AzureDevOpsToolkit.Core.Services;
using TimVinkemeier.AzureDevOpsToolkit.Services;

namespace TimVinkemeier.AzureDevOpsToolkit
{
    public class Setup : MvxWpfSetup<Core.App>
    {
        protected override IMvxWpfViewPresenter CreateViewPresenter(ContentControl root)
        {
            return new MultiLayoutViewPresenter(root);
        }

        protected override void InitializeFirstChance()
        {
            base.InitializeFirstChance();

            Mvx.IoCProvider.RegisterType<ISettingsService, SettingsService>();
            Mvx.IoCProvider.RegisterSingleton<IMessenger>(new Messenger());
        }
    }
}