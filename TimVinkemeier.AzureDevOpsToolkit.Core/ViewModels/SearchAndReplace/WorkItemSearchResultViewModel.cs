using MvvmCross.ViewModels;

using TimVinkemeier.AzureDevOpsToolkit.Core.Messages.SearchAndReplace;
using TimVinkemeier.AzureDevOpsToolkit.Core.Services;

namespace TimVinkemeier.AzureDevOpsToolkit.Core.ViewModels.SearchAndReplace
{
    public class WorkItemSearchResultViewModel : MvxViewModel
    {
        private readonly IMessenger _messenger;

        private bool _isSelected;

        private string _replaceResult;

        public WorkItemSearchResultViewModel(IMessenger messenger)
        {
            _messenger = messenger;
        }

        public string FieldDisplayName { get; set; }

        public string FieldName { get; set; }

        public string FindHighlight { get; set; }

        public string FindHighlightPostfix { get; set; }

        public string FindHighlightPrefix { get; set; }

        public int HitCount { get; set; }

        public int Id { get; set; }

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                SetProperty(ref _isSelected, value);
                _messenger.Publish(new SearchResultIsSelectedChangedMessage(this));
            }
        }

        public string ReplaceResult
        {
            get => _replaceResult;
            set => SetProperty(ref _replaceResult, value);
        }

        public string SearchValue { get; set; }

        public string Title { get; set; }
    }
}