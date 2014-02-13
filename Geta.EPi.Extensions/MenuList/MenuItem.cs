using System;
using EPiServer.Core;

namespace Geta.EPi.Extensions.MenuList
{
    /// <summary>
    ///     Model for one menu item.
    ///     It is used as a model in Menu item template.
    /// </summary>
    public class MenuItem
    {
        /// <summary>
        ///     Menu item's page.
        /// </summary>
        public PageData Page { get; set; }

        /// <summary>
        ///     Mark if menu item is selected.
        /// </summary>
        public bool Selected { get; set; }

        /// <summary>
        ///     Mark if menu item has child items.
        /// </summary>
        public Lazy<bool> HasChildren { get; set; }
    }
}