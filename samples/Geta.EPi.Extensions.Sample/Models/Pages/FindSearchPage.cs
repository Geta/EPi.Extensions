using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Geta.EPi.Extensions.Sample.Models;
using Geta.EPi.Extensions.Sample.Models.Pages;
using EPiServer.DataAnnotations;
using EPiServer.DataAbstraction;
using EPiServer.Core;
using Geta.EPi.Extensions.Sample.Models.Blocks;

namespace Geta.EPi.Extensions.Sample.Models.Pages
{
    /// <summary>
    /// Used to provide on-site search
    /// </summary>
    [SiteContentType(
        GUID = "AAC25733-1D21-4F82-B031-11E626C91E30",
        GroupName = Geta.EPi.Extensions.Sample.Global.GroupNames.Specialized,
        DisplayName = "SearchPage")]
    [SiteImageUrl]
    public class FindSearchPage : SitePageData, IHasRelatedContent, ISearchPage
    {
        public override void SetDefaultValues(ContentType contentType)
        {
            PageSize = 20;
            ExcerptLength = 200;
			HitImagesHeight = 30;
            base.SetDefaultValues(contentType);
        }

        [Display(
            GroupName = SystemTabNames.Content,
            Order = 310)]
        [CultureSpecific]
        [AllowedTypes(new[] { typeof(IContentData) }, new[] { typeof(JumbotronBlock) })]
        public virtual ContentArea RelatedContentArea { get; set; }

        /// <summary>
        /// Allow editors to control how many hits should be displayed
        /// on each search result listing when paging.
        /// </summary>
        [Range(1, 100)]
        [DefaultValue(20)]
        [Required]
        public virtual int PageSize { get; set; }

        /// <summary>
        /// Allow editors to control wether matching keywords in 
        /// search hit titles should be highlighted.
        /// </summary>
        public virtual bool HighlightTitles { get; set; }

        /// <summary>
        /// Allow editors to control wether matching keywords in 
        /// excerpt texts for search hits should be highlighted.
        /// If false the beginning of the search text will be retrieved.
        /// </summary>
        public virtual bool HighlightExcerpts { get; set; }

        /// <summary>
        /// Allow editors to specify the hight of hit images.
        /// </summary>
        [Range(0, 300)]
		[DefaultValue(30)]
		[Required]
        public virtual int HitImagesHeight { get; set; }

        /// <summary>
        /// Allow editors to specify length of excerpt text to 
        /// retrieve and show for each search hit.
        /// </summary>
        [Range(0, 1000)]
        [DefaultValue(200)]
        [Required]
        public virtual int ExcerptLength { get; set; }

        /// <summary>
        /// Allow search query to combine multiple search terms with AND
        /// </summary>
        public virtual bool UseAndForMultipleSearchTerms { get; set; }
    }
}
