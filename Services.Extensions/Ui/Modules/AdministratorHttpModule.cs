namespace Services.Extensions.Ui.Modules
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Xml;
    using Chains;
    using Chains.Play.Web;
    using Chains.Play.Web.HttpListener;
    using Services.Management.Administration.Server;
    using xLibrary;
    using xLibrary.Actions;

    internal sealed class AdministratorHttpRequestHandler : Chain<AdministratorHttpRequestHandler>, IHttpRequestHandler
    {
        public const string AdminHomepageUriPath = "/administration/";

        public const string AdminContentPath = "/ui/content/";

        private readonly string PathToContent = String.Format("{0}{1}Ui{1}Content{1}", AppDomain.CurrentDomain.BaseDirectory, Path.DirectorySeparatorChar);

        private readonly AdministrationContext administrationContext;

        public AdministratorHttpRequestHandler(HttpServer httpServer, AdministrationContext administrationContext)
        {
            this.administrationContext = administrationContext;

            httpServer.AddPath(AdminHomepageUriPath);

            httpServer.Modules.Add(this);
        }

        public bool ResolveRequest(HttpListenerContext context)
        {
            var libraryDocument = new XmlDocument();
            libraryDocument.Load(string.Format("{0}{1}", PathToContent, "administration.xml"));

            if (context.Request.Url.AbsolutePath.ToLowerInvariant() == AdminHomepageUriPath)
            {
                // Check security with administrationContext

                // Admin status + service status, performance graph (s), 

                var xContext = new xContext(new HttpContextInfo(context)).Do(new LoadLibrary(libraryDocument));

                xContext.DoFirstNotNull(
                    xcontext =>
                        xcontext.Do(new CreateTag("Administration.HomePage.Body.Status.Rows"))
                                .Do(new CheckIfRestRequest(onGet: StatusOnGet)),
                    xcontext => xcontext.Do(new CreateTag("Administration.HomePage")).Do(new RenderHtml()))
                        .ApplyOutputToHttpContext();

                return true;
            }

            return false;
        }

        private void StatusOnGet(xTag xTag, bool isAjax)
        {
            new xTagContext(xTag.xContext, xTag).Do(new Databind(administrationContext.ReportData.Select(x => x.Value)));
        }
    }
}
