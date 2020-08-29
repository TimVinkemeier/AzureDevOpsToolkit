using MvvmCross.Plugin.Messenger;

namespace TimVinkemeier.AzureDevOpsToolkit.Core.Messages.SearchAndReplace
{
    public class SearchFieldIsSelectedChangedMessage : MvxMessage
    {
        public SearchFieldIsSelectedChangedMessage(object sender) : base(sender)
        {
        }
    }
}