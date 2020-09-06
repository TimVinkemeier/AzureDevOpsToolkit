using System.Threading.Tasks;

using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace TimVinkemeier.AzureDevOpsToolkit.Core.ViewModels
{
    public abstract class ContentViewBaseViewModel : MvxNavigationViewModel
    {
        private MvxNotifyTask _busyTask;

        protected ContentViewBaseViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService)
            : base(logProvider, navigationService)
        {
            BusyTask = MvxNotifyTask.Create(Task.CompletedTask);
        }

        public MvxNotifyTask BusyTask
        {
            get => _busyTask;
            set => SetProperty(ref _busyTask, value);
        }
    }
}