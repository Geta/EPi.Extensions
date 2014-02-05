using EPiServer;

namespace Geta.EPi.Cms.MarineHarvest
{
    public static class EPiStringExtensions
    {
        public static Url ToUrl(this string target)
        {
            return new Url(target);
        }
    }
}
