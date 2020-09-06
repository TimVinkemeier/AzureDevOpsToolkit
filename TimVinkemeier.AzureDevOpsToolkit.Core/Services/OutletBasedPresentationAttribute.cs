using MvvmCross.Presenters.Attributes;

namespace TimVinkemeier.AzureDevOpsToolkit.Core.Services
{
    public class OutletBasedPresentationAttribute : MvxBasePresentationAttribute
    {
        public string OutletIdentifier { get; set; }
    }
}