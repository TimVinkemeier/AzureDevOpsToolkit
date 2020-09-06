![Toolkit for Azure DevOps Logo](https://github.com/TimVinkemeier/AzureDevOpsToolkit/blob/master/TimVinkemeier.AzureDevOpsToolkit.Package/Images/StoreLogo.scale-100.png)

# Toolkit for Azure DevOps

A toolkit application for Azure DevOps power users.
Currently in very early state...

Toolkit for Azure DevOps uses [MSIX packages](https://docs.microsoft.com/en-us/windows/msix/overview) for distribution.
You can find them linked in the [Releases](https://github.com/TimVinkemeier/AzureDevOpsToolkit/releases), however, the easier path is using the [dedicated website](https://toolkitforazuredevops.z16.web.core.windows.net/) that is also used to provide automatic updates to the toolkit.

## Getting Started

1. Install the publisher certificate from [here](https://toolkitforazuredevops.z16.web.core.windows.net/TimVinkemeier.AzureDevOpsToolkit.Package_0.1.0.0_Test/TimVinkemeier.AzureDevOpsToolkit.Package_0.1.0.0_x86_x64.cer) into the certificate store "Local Computer" > "Trusted Persons" as directed in [Microsoft's MSIX docs](https://docs.microsoft.com/en-us/windows/msix/app-installer/troubleshoot-appinstaller-issues#trusted-certificates). This step is necessary since only a self-signed certificate is currently used for signing (code signing certificates cost money, so... :shrug:)
1. Install the toolkit itself by clicking **Get the App** on [the toolkit's website](https://toolkitforazuredevops.z16.web.core.windows.net/).
1. Start the toolkit.
1. Go to **Settings**.
1. Enter the **base URL** of your Azure DevOps organisation, i.e. `https://dev.azure.com/yourorganizationname`
1. Enter the **name of the DevOps project** to search in
1. Go to Azure DevOps and create a **Personal Access Token** (PAT) with at least the permissions "Test Management" (Read & Write) and "Work Items" (Read & Write) and paste it into the settings field. (for more information, see [Microsoft's docs](https://docs.microsoft.com/en-us/azure/devops/organizations/accounts/use-personal-access-tokens-to-authenticate?view=azure-devops&tabs=preview-page#create-a-pat))
1. Click **Save**
1. You are now ready to use any of the features below - enjoy :smile:

## Features

### Work Item Find-and-Replace

Azure DevOps currently does not offer a convenient way to search and replace text over a large number of work items.
This may be necessary if features get renamed or labels in test cases need to be adjusted.

The toolkit offers a convenient way to search for text occurrences in configurable item types and fields and replace these occurrences with another text.

#### How to use

1. Click on **Find and Replace** in the main menu on the left
1. The toolkit will populate the available fields and item types with information from your configured Azure DevOps project.
1. Enter your desired search text in the **find** field
1. Select the **work item types** that you want to search
1. Select the **work item fields** that you want to search in
1. Click **Search**

The toolkit will query Azure DevOps for all items matching the specified criteria and return a list of hits in the grid.

1. **Select the results** that you want to be replaced. *Note: A search result always represents an entire field. If you do a replacement, all occurrences of the search text in that field will be replaced, not only the Example Occurrence!*
1. Specify a **replacement text**
1. Click **Replace**

All selected work item fields will be updated with all occurrences of the search text replaced with the replacement text. Since the toolkit is using a personal access token (PAT) to access Azure DevOps, the edits will show up in the work item history as being made by the user to which the PAT belongs.

 > TODO screenshots
