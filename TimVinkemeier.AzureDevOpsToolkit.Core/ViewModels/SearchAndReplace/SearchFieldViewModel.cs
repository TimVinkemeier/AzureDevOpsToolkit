using MvvmCross.ViewModels;

using TimVinkemeier.AzureDevOpsToolkit.Core.Messages.SearchAndReplace;
using TimVinkemeier.AzureDevOpsToolkit.Core.Services;

namespace TimVinkemeier.AzureDevOpsToolkit.Core.ViewModels.SearchAndReplace
{
    public class SearchFieldViewModel : MvxViewModel
    {
        private readonly IMessenger _messenger;

        private bool _isSelected;

        public SearchFieldViewModel(IMessenger messenger)
        {
            _messenger = messenger;
        }

        public string AzureDevOpsFieldName { get; set; }

        public string DisplayName { get; set; }

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                SetProperty(ref _isSelected, value);
                _messenger.Publish(new SearchFieldIsSelectedChangedMessage(this));
            }
        }

        public string SupportedContainsOperation { get; set; }
    }
}