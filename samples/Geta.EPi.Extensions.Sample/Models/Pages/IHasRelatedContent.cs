using EPiServer.Core;

namespace Geta.EPi.Extensions.Sample.Models.Pages
{
    public interface IHasRelatedContent
    {
        ContentArea RelatedContentArea { get; }
    }
}
