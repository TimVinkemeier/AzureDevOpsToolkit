using MvvmCross.Plugin.Messenger;

namespace TimVinkemeier.AzureDevOpsToolkit.Core.Messages.SearchAndReplace
{
    public class SearchResultIsSelectedChangedMessage : MvxMessage
    {
        public SearchResultIsSelectedChangedMessage(object sender)
            : base(sender)
        {
        }
    }
}