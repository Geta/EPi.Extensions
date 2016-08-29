using System;
using EPiServer.Core;

namespace Geta.EPi.Extensions.MenuList
{
    /// <summary>
    ///     Generic model for one menu item.
    ///     It is used as a model in Menu item template.
    /// </summary>
    public class MenuItem<T> where T : IContent
    {
        /// <summary>
        ///     Menu item's content.
        /// </summary>
        public virtual T Content { get; set; }

        /// <summary>
        ///     Mark if menu item is selected.
        /// </summary>
        public bool Selected { get; set; }

        /// <summary>
        ///     Mark if menu item has child items.
        /// </summary>
        public Lazy<bool> HasChildren { get; set; }

        /// <summary>
        ///     Mark if menu item has a child page that is marked as selected.
        /// </summary>
        public virtual Lazy<bool> HasSelectedChildContent { get; set; }
    }

    /// <summary>
    ///     Model for one menu item.
    ///     It is used as a model in Menu item template.
    /// </summary>
    public class MenuItem : MenuItem<PageData>
    {
        /// <summary>
        ///     Menu item's page.
        /// </summary>
        public PageData Page => Content;

        /// <summary>
        ///     Mark if menu item has a child page that is marked as selected.
        /// </summary>
        public Lazy<bool> HasSelectedChildPage => HasSelectedChildContent;
    }
}