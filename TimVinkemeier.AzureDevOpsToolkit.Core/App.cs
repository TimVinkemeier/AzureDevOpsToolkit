using MvvmCross;
using MvvmCross.IoC;
using MvvmCross.Plugin.Messenger;
using MvvmCross.ViewModels;

using TimVinkemeier.AzureDevOpsToolkit.Core.ViewModels;

namespace TimVinkemeier.AzureDevOpsToolkit.Core
{
    public class App : MvxApplication
    {
        public override void Initialize()
        {
            CreatableTypes()
                .EndingWith("Service")
                .AsInterfaces()
                .RegisterAsLazySingleton();

            Mvx.IoCProvider.RegisterSingleton<IMvxMessenger>(new MvxMessengerHub());

            RegisterAppStart<RootViewModel>();
        }
    }
}