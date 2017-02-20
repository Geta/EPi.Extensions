using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;
using EPiServer.Web;

namespace Geta.EPi.Extensions
{
    /// <summary>
    ///     Initialization module for extensions package.
    /// </summary>
    [InitializableModule]
    [ModuleDependency(typeof(EPiServer.Web.InitializationModule))]
    public class ExtensionsInitializationModule : IInitializableModule
    {
        /// <summary>
        ///     Attach <see cref="TemplateResolver"/>.TemplateResolved event.
        ///     The reason is so we can keep track of if a block is rendered through block preview template.
        ///     Used in EditButtonFor and EditorHelpFor HTML helpers.
        /// </summary>
        /// <param name="context"></param>
        public void Initialize(InitializationEngine context)
        {
            context.Locate.TemplateResolver().TemplateResolved += OnTemplateResolved;
        }

        /// <summary>
        ///     Detach <see cref="TemplateResolver"/>.TemplateResolved event.
        /// </summary>
        /// <param name="context"></param>
        public void Uninitialize(InitializationEngine context)
        {
            ServiceLocator.Current.GetInstance<TemplateResolver>().TemplateResolved -= OnTemplateResolved;
        }

        private void OnTemplateResolved(object sender, TemplateResolverEventArgs args)
        {
            if (args.SelectedTemplate == null) return;

            if (args.SelectedTemplate.IsBlockPreviewTemplate())
            {
                args.WebContext.Items["IsBlockPreviewTemplate"] = true;
            }
        }
    }
}
