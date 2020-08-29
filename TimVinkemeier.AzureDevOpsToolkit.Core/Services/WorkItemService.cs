﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.TeamFoundation.WorkItemTracking.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.WebApi.Patch.Json;

namespace TimVinkemeier.AzureDevOpsToolkit.Core.Services
{
    public class WorkItemService : IWorkItemService
    {
        private readonly ISettingsService _settingsService;

        public WorkItemService(ISettingsService settingsService)
        {
            _settingsService = settingsService;
        }

        public async Task<WorkItem> GetWorkItemAsync(int id)
        {
            var baseUrl = await _settingsService.GetSettingAsync<string>(Setting.OrganisationBaseUrl).ConfigureAwait(false);
            var accessToken = await _settingsService.GetSettingAsync<string>(Setting.AzureDevOpsToken).ConfigureAwait(false);
            var credentials = new VssBasicCredential(string.Empty, accessToken);
            var uri = new Uri(baseUrl);
            var client = new WorkItemTrackingHttpClient(uri, credentials);
            return await client.GetWorkItemAsync(id).ConfigureAwait(false);
        }

        public async Task<IReadOnlyList<WorkItemField>> GetWorkItemFieldsAsync()
        {
            var baseUrl = await _settingsService.GetSettingAsync<string>(Setting.OrganisationBaseUrl).ConfigureAwait(false);
            var accessToken = await _settingsService.GetSettingAsync<string>(Setting.AzureDevOpsToken).ConfigureAwait(false);
            var projectName = await _settingsService.GetSettingAsync<string>(Setting.ProjectName).ConfigureAwait(false);
            var credentials = new VssBasicCredential(string.Empty, accessToken);
            var uri = new Uri(baseUrl);
            var client = new WorkItemTrackingHttpClient(uri, credentials);
            return await client.GetFieldsAsync(projectName).ConfigureAwait(false);
        }

        public async Task<IReadOnlyList<WorkItem>> GetWorkItemsForWiqlQueryAsync(string query)
        {
            var baseUrl = await _settingsService.GetSettingAsync<string>(Setting.OrganisationBaseUrl).ConfigureAwait(false);
            var accessToken = await _settingsService.GetSettingAsync<string>(Setting.AzureDevOpsToken).ConfigureAwait(false);
            var credentials = new VssBasicCredential(string.Empty, accessToken);
            var uri = new Uri(baseUrl);
            var client = new WorkItemTrackingHttpClient(uri, credentials);
            var wiqlQuery = new Wiql
            {
                Query = query
            };
            var queryResult = await client.QueryByWiqlAsync(wiqlQuery).ConfigureAwait(true);
            IEnumerable<WorkItem> searchResults = new List<WorkItem>();
            foreach (var workItemReferenceBatch in queryResult.WorkItems.Batch(200))
            {
                var workItems = await client.GetWorkItemsAsync(workItemReferenceBatch.Select(reference => reference.Id)).ConfigureAwait(true);
                searchResults = searchResults.Concat(workItems);
            }

            return searchResults.OrderBy(s => s.Id).ToList();
        }

        public async Task<IReadOnlyList<WorkItemType>> GetWorkItemTypesAsync()
        {
            var baseUrl = await _settingsService.GetSettingAsync<string>(Setting.OrganisationBaseUrl).ConfigureAwait(false);
            var accessToken = await _settingsService.GetSettingAsync<string>(Setting.AzureDevOpsToken).ConfigureAwait(false);
            var projectName = await _settingsService.GetSettingAsync<string>(Setting.ProjectName).ConfigureAwait(false);
            var credentials = new VssBasicCredential(string.Empty, accessToken);
            var uri = new Uri(baseUrl);
            var client = new WorkItemTrackingHttpClient(uri, credentials);
            return await client.GetWorkItemTypesAsync(projectName).ConfigureAwait(false);
        }

        public async Task UpdateWorkItemAsync(int workItemId, JsonPatchDocument patchDocument, CancellationToken cancellationToken)
        {
            var baseUrl = await _settingsService.GetSettingAsync<string>(Setting.OrganisationBaseUrl).ConfigureAwait(false);
            var accessToken = await _settingsService.GetSettingAsync<string>(Setting.AzureDevOpsToken).ConfigureAwait(false);
            var credentials = new VssBasicCredential(string.Empty, accessToken);
            var uri = new Uri(baseUrl);
            var client = new WorkItemTrackingHttpClient(uri, credentials);
            var workItem = await client.GetWorkItemAsync(workItemId, cancellationToken: cancellationToken).ConfigureAwait(false);
            if (cancellationToken.IsCancellationRequested)
            {
                return;
            }

            await client.UpdateWorkItemAsync(patchDocument, (int)workItem.Id, false, false, false, null, cancellationToken).ConfigureAwait(false);
        }
    }
}