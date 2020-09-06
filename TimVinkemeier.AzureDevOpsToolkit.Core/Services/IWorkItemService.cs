using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using Microsoft.VisualStudio.Services.WebApi.Patch.Json;

namespace TimVinkemeier.AzureDevOpsToolkit.Core.Services
{
    public interface IWorkItemService
    {
        Task<WorkItem> GetWorkItemAsync(int id);

        Task<IReadOnlyList<WorkItemField>> GetWorkItemFieldsAsync();

        Task<IReadOnlyList<WorkItem>> GetWorkItemsForWiqlQueryAsync(string query);

        Task<IReadOnlyList<WorkItemType>> GetWorkItemTypesAsync();

        Task TestSettingsAsync(string baseUrl, string projectName, string accessToken);

        Task UpdateWorkItemAsync(int workItemId, JsonPatchDocument patchDocument, CancellationToken cancellationToken);
    }
}