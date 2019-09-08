using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace Geta.EPi.Extensions
{
    /// <summary>
    /// HtmlHelper extensions.
    /// </summary>
    public static class HtmlHelperExtensions
    {
        /// <summary>
        /// Renders the link to the action if has permissions.
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="linkText"></param>
        /// <param name="actionName"></param>
        /// <param name="controllerName"></param>
        /// <param name="routeValues"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static MvcHtmlString AuthorizedActionLink(
            this HtmlHelper helper, 
            string linkText, 
            string actionName, 
            string controllerName, 
            object routeValues, 
            object htmlAttributes)
        {
            if (HasActionPermission(helper, actionName, controllerName))
            {
                return helper.ActionLink(linkText, actionName, controllerName, routeValues, htmlAttributes);
            }

            return MvcHtmlString.Empty;
        }

        /// <summary>
        /// Renders the link to the action if has permissions.
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="linkText"></param>
        /// <param name="actionName"></param>
        /// <param name="controllerName"></param>
        /// <returns></returns>
        public static MvcHtmlString AuthorizedActionLink(
            this HtmlHelper helper, 
            string linkText, 
            string actionName, 
            string controllerName)
        {
            if (HasActionPermission(helper, actionName, controllerName))
            {
                return helper.ActionLink(linkText, actionName, controllerName);
            }

            return MvcHtmlString.Empty;
        }

        /// <summary>
        /// Renders the link to the action if has permissions.
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="linkText"></param>
        /// <param name="actionName"></param>
        /// <param name="controllerName"></param>
        /// <param name="routeValues"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static MvcHtmlString AuthorizedActionLink(
            this HtmlHelper helper, 
            string linkText, 
            string actionName, 
            string controllerName, 
            RouteValueDictionary routeValues, 
            IDictionary<string, object> htmlAttributes)
        {
            if (HasActionPermission(helper, actionName, controllerName))
            {
                return helper.ActionLink(linkText, actionName, controllerName, routeValues, htmlAttributes);
            }

            return MvcHtmlString.Empty;
        }

        /// <summary>
        /// Checks if a user has permission to the controller's action.
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="actionName"></param>
        /// <param name="controllerName"></param>
        /// <returns></returns>
        public static bool HasActionPermission(this HtmlHelper htmlHelper, string actionName, string controllerName)
        {
            var controllerToLinkTo = string.IsNullOrEmpty(controllerName)
                ? htmlHelper.ViewContext.Controller
                : GetControllerByName(htmlHelper, controllerName);

            var controllerContext = new ControllerContext(htmlHelper.ViewContext.RequestContext, controllerToLinkTo);

            var controllerDescriptor = new ReflectedControllerDescriptor(controllerToLinkTo.GetType());
            var actionDescriptor = controllerDescriptor.FindAction(controllerContext, actionName);

            return ActionIsAuthorized(controllerContext, actionDescriptor);
        }

        private static bool ActionIsAuthorized(ControllerContext controllerContext, ActionDescriptor actionDescriptor)
        {
            if (actionDescriptor == null)
                return false;

            var authContext = new AuthorizationContext(controllerContext, actionDescriptor);
            foreach (var authFilter in FilterProviders.Providers.GetFilters(authContext, actionDescriptor))
            {
                if (authFilter.Instance is AuthorizeAttribute)
                {
                    ((IAuthorizationFilter)authFilter.Instance).OnAuthorization(authContext);

                    if (authContext.Result != null)
                        return false;
                }
            }

            return true;
        }

        private static ControllerBase GetControllerByName(HtmlHelper helper, string controllerName)
        {
            var factory = ControllerBuilder.Current.GetControllerFactory();
            var controller = factory.CreateController(helper.ViewContext.RequestContext, controllerName);

            if (controller == null)
            {
                throw new InvalidOperationException(
                    string.Format(
                        CultureInfo.CurrentUICulture,
                        "Controller factory {0} controller {1} returned null",
                        factory.GetType(),
                        controllerName));
            }

            return (ControllerBase)controller;
        }
    }
}