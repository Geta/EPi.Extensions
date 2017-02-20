# Extensions and helpers library for EPiServer CMS

![](http://tc.geta.no/app/rest/builds/buildType:(id:TeamFrederik_ExtensionsDebug)/statusIcon)
[![Platform](https://img.shields.io/badge/Platform-.NET 4.5.2-blue.svg?style=flat)](https://msdn.microsoft.com/en-us/library/w0x726c2%28v=vs.110%29.aspx)
[![Platform](https://img.shields.io/badge/Episerver-%2010-orange.svg?style=flat)](http://world.episerver.com/cms/)

## What is EPi.Extensions?

EPi.Extensions is library with useful extension methods and helpers for EPiServer.

## How to get started?

Start by installing NuGet package (use [EPiServer NuGet](http://nuget.episerver.com/)):

    Install-Package Geta.EPi.Extensions

See [reference](http://geta.github.io/EPi.Extensions/) and examples below.

# Examples

## Basics

You can use the GetChildren and GetPage extension methods to easily fetch pages. They also have generic overloads.

    var startPage = ContentReference.StartPage.GetPage<StartPage>();
    var sections = ContentReference.StartPage.GetChildren<SectionPage>();

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

Markup would look something like this:

    <form action="" class="form xform" method="post">
        <div id="id_matrix" class="xform-table">
            <div class="xform-body">
                <div class="xform-row">
                    <div class="xform-col">
                        <label for="Input_54330581">
                            Navn
                        </label>
                        <input id="Input_54330581" name="Navn" size="70" type="text" value="" />
                    </div>
                </div><div class="xform-row">
                    <div class="xform-col">
                        <label for="Input_54330581">
                            Epost
                        </label>
                        <input id="Input_54330581" name="Epost" size="70" type="text" value="" />
    
                    </div>
                </div>
            </div>
        </div>
    </form>

## QueryStringBuilder

Here we have an example of using _QueryStringBuilder_ to build a filter URL. This can be useful for lists that have filter functionality or sort functionality. To instantiate _QueryStringBuilder_ _UrlHelper_ extensions are used.

    <a href="@Url.QueryBuilder(Request.RawUrl).Toggle("sort", "alphabet")">A-Å</a>

Output when URL is: /list

    <a href="/list?sort=alphabet">A-Å</a>

Output when URL is: /list?sort=alphabet

    <a href="/list">A-Å</a>
    
Here is an example of using _QueryStringBuilder_ to add a segment to a EPiServer page URL. This can be useful for forms if you want to post to a page controller action.

    <form action="@Url.QueryBuilder(Model.CurrentPage.ContentLink).AddSegment("MyActionName")"></form>
    
Output when page URL is: /about-us

    <form action="/about-us/MyActionName"></form>

## Validation

We have included a simple validation helper for validating email address' using .NET's built in email validation (which updates together with newer versions/patches for .NET).

    bool isValidEmail = ValidationHelper.IsValidEmail("test@example.com");

## Enum properties

If you have enum values you want to use in your content types you can use the EnumAttribute to decorate your properties. The values can also be localized.

    [BackingType(typeof(PropertyNumber))]
    [EnumAttribute(typeof(Priority))]
    public virtual Priority Priority { get; set; }

Credits: http://world.episerver.com/Blogs/Linus-Ekstrom/Dates/2014/5/Enum-properties-for-EPiServer-75/

## Categories

You can easily get the child categories of any root category you like (as long as you have it's ID).

    IEnumerable<Category> categories = Category.GetRoot().ID.GetChildCategories();

When you have a CategoryList and want to get strongly typed Category objects back you can use the GetFullCategories() method.

    IEnumerable<Category> categories = CurrentPage.Category.GetFullCategories();

If you need to check if the CategoryList has that category you can use the Contains() method.

    bool hasBikes = CurrentPage.Category.Contains("bikes");

## External/friendly URL

This can be useful when used together with sharing widgets.

    string fullUrl = CurrentPage.GetFriendlyUrl();

## Singleton page

Allows easily load the page which is a single instance of a type.

Loading the singleton page of a type by a parent _ContentReference_.

    var testPage1 = ContentReference.StartPage.GetSingletonPage<TestPage>();

Loading the singleton page of a type by a parent page.

    var startPage = _contentLoader.Get<StartPage>(ContentReference.StartPage);
    var testPage2 = startPage.GetSingletonPage<TestPage>();

## Content editor user experience helpers/extensions

Set of extension methods and HTML helpers to improve user experience for content editors. 
The goal is to reduce the need for "All properties view" in the Episerver edit interface.

### EditButton attribute

Attribute to use on properties that you want to have editable in On Page edit mode. Typical usage is for settings properties or other properties that are normally not rendered in your view. 

	[EditButton(ButtonLabel = "My property")] // If you don't supply ButtonLabel, display name of the property will be used instead.
	public virtual string MyProperty { get; set; }

The edit button is then rendered in the view, only when page is in edit mode, with EditButtonFor helper:

	@Html.EditButtonFor(m => m.MyProperty)

You can also use the EditButtonsGroup helper to render buttons for all properties marked with the EditButtonAttribute:

	@Html.EditButtonsGroup() // If view model is IContentData 
	@Html.EditButtonsGroup(m => m.CurrentPage) // If view model is page view model

Note: EditButtonsGroup accepts an argument named includeBuiltInProperties (defaults to false) which, if true, also renders buttons for the following built-in properties:
	
	Category
	Simple address
	URLSegment
	Display in navigation
	Published
	Update modified date

### EditorHelp attribute

Attribute to use on properties you might want an extended help text for in edit mode.

	[EditorHelp("This is the main content area for blocks. The following block types are supported: My block 1, My block 2.")]
	public virtual ContentArea MainContentArea { get;set; }

The help text is rendered in the view (as ul element) with EditorHelpFor helper:

	@Html.EditorHelpFor(m => m.MainContentArea)
	@Html.PropertyFor(m => m.MainContentArea)

You can also render a help summary for all properties marked with the EditorHelpAttribute:

	@Html.EditorHelpSummary() // If view model is IContentData
	@Html.EditorHelpSummary(m => m.CurrentPage) // If view model is page view model

Please note that the buttons and help texts are not styled with any CSS in this package. You will have to do that yourself.

### Example screenshot of EditButton and EditorHelp helpers

![ScreenShot](/docs/content-editor-enhancements.png)