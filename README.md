EPi.Extensions
==============

[Reference](http://geta.github.io/EPi.Extensions/)

# Examples

## Filters

You can use _FilterForDisplay_ to easily filter out pages that the user shouldn't see. Here is an example of how to filter child pages of start page.

    var childPages = ContentReference.StartPage.GetChildren().FilterForDisplay();


## MenuList extension for HtmlHelper

_MenuList_ extension method helps to build menus. Extension method requires two parameters - _ContentReference_ of the menu root page and _@helper_ which generates menu item. _MenuList_ uses _FilterForDisplay_ extension method to filter out pages that the user doesn't have access to, are not published and are not visible in menu and that don't have a template.

_MenuList_ extension method has three optional parameters:
- _includeRoot_ - flag which indicates if root page should be included in menu (default is false)
- _requireVisibleInMenu_ - flag which indicates if pages should be included only when their property _VisibleInMenu_ is true (default is true)
- _requirePageTemplate_ - flag which indicates if pages should have templates

Below is an example how menu can be created for pages under start page.

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

_MenuList_ creates the list only for one level. For multiple menu levels use _MenuList_ extension in menu item template to generate sub-levels.

    @helper SubMenuItemTemplate(MenuItem item)
    {
        <li class="@(item.Selected ? "active" : null)">
            @Html.PageLink(item.Page)
        </li>
    }

    @helper MenuItemTemplate(MenuItem item)
    {
        <li class="@(item.Selected ? "active" : null)">
            @Html.PageLink(item.Page)
            <ul>
                @Html.MenuList(item.Page.ContentLink, SubMenuItemTemplate)
            </ul>
        </li>
    }

    <nav id="main-nav" class="navigation" role="navigation">
        <ul class="nav">
            @Html.MenuList(ContentReference.StartPage, MenuItemTemplate)
        </ul>
    </nav>

## XFormHelper

Example of a custom DisplayTemplate for XForm using the _XFormHelper_ to clean up the markup.

    @using EPiServer.HtmlParsing
    @using EPiServer.Web.Mvc.Html
    @using Geta.EPi.Cms.Helpers
    @model EPiServer.XForms.XForm
    @if (ViewData["XFormActionResult"] is EPiServer.Web.Mvc.XForms.XFormSuccessActionResult)
    {
        <strong>Form posted.</strong>
    }
    else
    {
        using (Html.BeginXForm(Model, htmlAttributes: new { @class = "form xform" }))
        {
            if (Model != null)
            {
                foreach (HtmlFragment fragment in (IEnumerable<HtmlFragment>)ViewData["XFormFragments"] ?? Model.CreateHtmlFragments())
                {
                    @Html.Fragment(XFormHelper.CleanupXFormHtmlMarkup(fragment))
                }
            }
        }
    }

## QueryStringBuilder

Here we have an example of using _QueryStringBuilder_ to build a filter URL. This can be useful for lists that have filter functionality or sort functionality. To instantiate _QueryStringBuilder_ _UrlHelper_ extensions are used.

    <a href="@Url.QueryBuilder(Request.RawUrl).Toggle("sort", "alphabet")">A-Å</a>

Output when URL is: /list

    <a href="/list?sort=alphabet">A-Å</a>

Output when URL is: /list?sort=alphabet

    <a href="/list">A-Å</a>
