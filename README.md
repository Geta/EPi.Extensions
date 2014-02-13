EPi.Extensions
==============

[Reference](http://geta.github.io/EPi.Extensions/)

## MenuList extension for HtmlHelper

_MenuList_ extension method helps to build menus. Extension method requires two parameters - _ContentReference_ of the menu root page and _@helper_ which generates menu item. Below is an example how menu can be created for pages under start page.

    @helper MenuItemTemplate(MenuItem item)
    {
        <li class="@(item.Selected ? "active" : null)">
            @Html.PageLink(item.Page)
        </li>
    }

    <nav id="main-nav" class="navigation" role="navigation">
        <ul class="nav">
            @Html.MenuList(ContentReference.StartPage, MenuItemTemplate)
        </ul>
    </nav>

_MenuList_ extension method has three optional parameters:
- _includeRoot_ - flag which indicates if root page should be included in menu (default is false)
- _requireVisibleInMenu_ - flag which indicates if pages should be included only when their property _VisibleInMenu_ is true (default is true)
- _requirePageTemplate_ - flag which indicates if pages should have templates (default is true)

_MenuList_ creates the list only for one level. For multiple menu levels use _MenuList_ extension in menu item template to generate sub-levels.
