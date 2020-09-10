using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using Microsoft.VisualStudio.Services.WebApi.Patch;
using Microsoft.VisualStudio.Services.WebApi.Patch.Json;

using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.Navigation;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

using TimVinkemeier.AzureDevOpsToolkit.Core.Extensions;
using TimVinkemeier.AzureDevOpsToolkit.Core.Messages.SearchAndReplace;
using TimVinkemeier.AzureDevOpsToolkit.Core.Services;

namespace TimVinkemeier.AzureDevOpsToolkit.Core.ViewModels.SearchAndReplace
{
    public class SearchAndReplaceViewModel : ContentViewBaseViewModel
    {
        private readonly IMessenger _messenger;
        private readonly ISettingsService _settingsService;
        private readonly IWorkItemService _workItemService;
        private int _currentReplacementCount;
        private bool _isRunningReplace;
        private bool _isRunningSearch;
        private string _replaceText;
        private ObservableCollection<SearchFieldViewModel> _searchFields = new ObservableCollection<SearchFieldViewModel>();
        private ObservableCollection<WorkItemTypeViewModel> _searchItemTypes = new ObservableCollection<WorkItemTypeViewModel>();
        private ObservableCollection<WorkItemSearchResultViewModel> _searchResults = new ObservableCollection<WorkItemSearchResultViewModel>();
        private string _searchText;

        private int _totalReplacementCount;

        public SearchAndReplaceViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService, IWorkItemService workItemService, IMessenger messenger, ISettingsService settingsService)
            : base(logProvider, navigationService)
        {
            SearchCommand = new MvxAsyncCommand(
                () => SearchAsync(SearchText, SearchItemTypes.Where(f => f.IsSelected).ToList(), SearchFields.Where(f => f.IsSelected).ToList()),
                () => !string.IsNullOrWhiteSpace(SearchText) && !IsRunningReplace && SearchItemTypes.Count(f => f.IsSelected) > 0 && SearchFields.Count(f => f.IsSelected) > 0);
            ReplaceCommand = new MvxAsyncCommand(ReplaceAsync, () => TotalReplacementCount > 0 && !string.IsNullOrWhiteSpace(ReplaceText) && !IsRunningSearch);
            OpenWorkItemCommand = new MvxAsyncCommand<int>(OpenWorkItemAsync, id => id > 0);
            _workItemService = workItemService;
            _messenger = messenger;
            _settingsService = settingsService;
        }

        public bool AreAllResultsSelected
        {
            get { return SearchResults?.Count > 0 && (SearchResults?.All(r => r.IsSelected) ?? false); }
            set
            {
                foreach (var result in SearchResults)
                {
                    result.IsSelected = value;
                }

                RaisePropertyChanged();
            }
        }

        public int CurrentReplacementCount
        {
            get => _currentReplacementCount;
            set => SetProperty(ref _currentReplacementCount, value);
        }

        public bool IsRunningReplace
        {
            get => _isRunningReplace;
            set
            {
                SetProperty(ref _isRunningReplace, value);
                SearchCommand?.RaiseCanExecuteChanged();
            }
        }

        public bool IsRunningSearch
        {
            get => _isRunningSearch;
            set
            {
                SetProperty(ref _isRunningSearch, value);
                ReplaceCommand?.RaiseCanExecuteChanged();
            }
        }

        public IMvxAsyncCommand<int> OpenWorkItemCommand { get; }

        public IMvxAsyncCommand ReplaceCommand { get; }

        public string ReplaceText
        {
            get => _replaceText;
            set
            {
                SetProperty(ref _replaceText, value);
                OnSelectionChanged();
            }
        }

        public IMvxAsyncCommand SearchCommand { get; }

        public ObservableCollection<SearchFieldViewModel> SearchFields
        {
            get => _searchFields;
            set => SetProperty(ref _searchFields, value);
        }

        public ObservableCollection<WorkItemTypeViewModel> SearchItemTypes
        {
            get => _searchItemTypes;
            set => SetProperty(ref _searchItemTypes, value);
        }

        public ObservableCollection<WorkItemSearchResultViewModel> SearchResults
        {
            get => _searchResults;
            set
            {
                SetProperty(ref _searchResults, value);
                OnSelectionChanged();
            }
        }

        public string SearchText
        {
            get => _searchText;
            set
            {
                SetProperty(ref _searchText, value);
                SearchCommand?.RaiseCanExecuteChanged();
            }
        }

        public int TotalReplacementCount
        {
            get => _totalReplacementCount;
            set => SetProperty(ref _totalReplacementCount, value);
        }

        public override async Task Initialize()
        {
            _messenger.Subscribe<SearchResultIsSelectedChangedMessage>(_ => OnSelectionChanged());
            _messenger.Subscribe<SearchFieldIsSelectedChangedMessage>(_ => OnSelectionChanged());
            await PopulateSearchFieldsAsync().ConfigureAwait(false);
        }

        private WorkItemSearchResultViewModel BuildSearchResultViewModelForWorkItem(string searchText, WorkItem wi, SearchFieldViewModel selectedField)
        {
            const int highlightRange = 25;
            var fieldValue = wi.Fields[selectedField.AzureDevOpsFieldName]?.ToString() ?? string.Empty;
            fieldValue = fieldValue.ToLowerInvariant();

            var parts = fieldValue.Split(new[] { searchText }, StringSplitOptions.RemoveEmptyEntries);

            var prefix = parts.Length == 0
                ? string.Empty
                : (parts[0].Length > highlightRange ? "..." : string.Empty) + new string(parts[0].Reverse().Take(highlightRange).Reverse().ToArray());
            var postfix = parts.Length > 1
                ? new string(parts[1].Take(highlightRange).ToArray()) + (parts[1].Length > highlightRange ? "..." : string.Empty)
                : string.Empty;
            var findHighlight = searchText;

            var hitCount = fieldValue.CountOccurencesOf(searchText);
            return new WorkItemSearchResultViewModel(_messenger)
            {
                Id = wi.Id ?? 0,
                Title = wi.Fields["System.Title"].ToString() ?? string.Empty,
                FindHighlight = findHighlight ?? string.Empty,
                FindHighlightPrefix = prefix ?? string.Empty,
                FindHighlightPostfix = postfix ?? string.Empty,
                HitCount = hitCount,
                FieldName = selectedField.AzureDevOpsFieldName,
                FieldDisplayName = selectedField.DisplayName,
                SearchValue = searchText
            };
        }

        private void OnSelectionChanged()
        {
            TotalReplacementCount = SearchResults?.Count(r => r.IsSelected) ?? 0;
            SearchCommand?.RaiseCanExecuteChanged();
            ReplaceCommand?.RaiseCanExecuteChanged();

            RaisePropertyChanged(nameof(AreAllResultsSelected));
        }

        private async Task OpenWorkItemAsync(int workItemId)
        {
            var baseUrl = await _settingsService.GetSettingAsync<string>(Setting.OrganisationBaseUrl).ConfigureAwait(false);
            var projectName = await _settingsService.GetSettingAsync<string>(Setting.ProjectName).ConfigureAwait(false);
            Process.Start(new ProcessStartInfo
            {
                UseShellExecute = true,
                FileName = $"{baseUrl}/{projectName}/_workitems/edit/{workItemId}/"
            });
        }

        private async Task PopulateSearchFieldsAsync()
        {
            var workItemTypes = await _workItemService.GetWorkItemTypesAsync().ConfigureAwait(false);
            var workItemFields = await _workItemService.GetWorkItemFieldsAsync().ConfigureAwait(false);
            var types = new List<WorkItemTypeViewModel>();
            var fields = new List<SearchFieldViewModel>();
            foreach (var workItemType in workItemTypes)
            {
                types.Add(new WorkItemTypeViewModel(_messenger)
                {
                    DisplayName = workItemType.Name,
                    AzureDevOpsTypeName = workItemType.ReferenceName,
                    IsSelected = true
                });
            }

            foreach (var field in workItemFields.Where(f => workItemTypes.Any(wit => wit.Fields.Any(witf => witf.ReferenceName == f.ReferenceName))))
            {
                var supportsContains = field.SupportedOperations.Any(o => o.ReferenceName == "SupportedOperations.Contains");
                var supportsContainsWords = field.SupportedOperations.Any(o => o.ReferenceName == "SupportedOperations.ContainsWords");
                var isReadOnly = field.ReadOnly;
                if (!supportsContains && !supportsContainsWords && !isReadOnly)
                {
                    continue;
                }

                fields.Add(new SearchFieldViewModel(_messenger)
                {
                    DisplayName = field.Name,
                    AzureDevOpsFieldName = field.ReferenceName,
                    SupportedContainsOperation = supportsContains ? "Contains" : "Contains Words"
                });
            }

            SearchFields = new ObservableCollection<SearchFieldViewModel>(fields.OrderBy(f => f.DisplayName));
            SearchItemTypes = new ObservableCollection<WorkItemTypeViewModel>(types.OrderBy(f => f.DisplayName));
        }

        private async Task ReplaceAsync()
        {
            IsRunningReplace = true;
            var replaceText = ReplaceText;
            var itemsToUpdate = SearchResults.Where(r => r.IsSelected).ToList();

            CurrentReplacementCount = 0;
            TotalReplacementCount = itemsToUpdate.Count;
            foreach (var workItemVm in itemsToUpdate)
            {
                try
                {
                    var workItem = await _workItemService.GetWorkItemAsync(workItemVm.Id).ConfigureAwait(false);
                    var oldValue = workItem.Fields[workItemVm.FieldName]?.ToString() ?? string.Empty;
                    var newValue = oldValue?.Replace(workItemVm.SearchValue, replaceText);
                    var patchDocument = new JsonPatchDocument
                    {
                        new JsonPatchOperation
                        {
                            Operation = Operation.Replace,
                            Path = $"/fields/{workItemVm.FieldName}",
                            Value = newValue
                        }
                    };
                    await _workItemService.UpdateWorkItemAsync(workItemVm.Id, patchDocument, default).ConfigureAwait(false);
                    workItemVm.ReplaceResult = "Replacement succeeded";
                }
                catch (Exception ex)
                {
                    workItemVm.ReplaceResult = $"Replacement failed ({ex.Message})";
                }

                workItemVm.IsSelected = false;
                CurrentReplacementCount++;
            }

            CurrentReplacementCount = 0;
            IsRunningReplace = false;
        }

        private async Task SearchAsync(string searchText, IReadOnlyList<WorkItemTypeViewModel> selectedWorkItemTypes, IReadOnlyList<SearchFieldViewModel> selectedFields)
        {
            IsRunningSearch = true;
            searchText = searchText.ToLowerInvariant();
            var joinedTypes = string.Join(",", selectedWorkItemTypes.Select(wit => $"'{wit.DisplayName}'"));
            IEnumerable<WorkItemSearchResultViewModel> vms = new List<WorkItemSearchResultViewModel>();
            foreach (var selectedField in selectedFields)
            {
                var query = $"Select[Id] From WorkItems Where [Work Item Type] IN ({joinedTypes}) And [{selectedField.AzureDevOpsFieldName}] {selectedField.SupportedContainsOperation} '{searchText}'";
                var results = await _workItemService.GetWorkItemsForWiqlQueryAsync(query).ConfigureAwait(false);
                vms = vms.Concat(results.Select(wi => BuildSearchResultViewModelForWorkItem(searchText, wi, selectedField)));
            }
            SearchResults = new ObservableCollection<WorkItemSearchResultViewModel>(vms);
            IsRunningSearch = false;
        }
    }
}