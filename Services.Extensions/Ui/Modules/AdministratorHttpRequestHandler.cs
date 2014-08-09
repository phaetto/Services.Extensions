namespace Services.Extensions.Ui.Modules
{
    using System;
    using System.IO;
    using Chains.Play.Web.HttpListener;
    using Services.Extensions.Web.xTags;
    using xLibrary;

    internal sealed class AdministratorHttpRequestHandler : xTagHttpHtmlAndRestHandler
    {
        public const string AdminHomepageUriPath = "/administration/";

        private static string PathToContent = string.Format("{0}{1}Ui{1}Admin{1}Content{1}{2}", AppDomain.CurrentDomain.BaseDirectory, Path.DirectorySeparatorChar, "administration.xml");

        public AdministratorHttpRequestHandler(HttpServer httpServer) :
            base(AdminHomepageUriPath, PathToContent, "Administration.HomePage", "Administration.HomePage.Body.Status.Rows")
        {
            httpServer.AddPath(AdminHomepageUriPath);

            httpServer.Modules.Add(this);
        }

        public override void Get(xTag xtag, bool isAjax)
        {
            // new xTagContext(xtag.xContext, xtag).Do(new Databind(administrationContext.ReportData.Select(x => x.Value)));
            throw new NotImplementedException();
        }

        public override void Post(xTag xtag, bool isAjax)
        {
            throw new NotImplementedException();
        }

        public override void Put(xTag xtag, bool isAjax)
        {
            throw new NotImplementedException();
        }

        public override void Delete(xTag xtag, bool isAjax)
        {
            throw new NotImplementedException();
        }
    }
}
