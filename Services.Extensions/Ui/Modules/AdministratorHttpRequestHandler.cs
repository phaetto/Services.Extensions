namespace Services.Extensions.Ui.Modules
{
    using System;
    using System.IO;
    using System.Linq;
    using Chains.Play.Web.HttpListener;
    using Services.Communication.Protocol;
    using Services.Extensions.Ui.Admin;
    using Services.Extensions.Web.xTags;
    using Services.Management.Administration.Server;
    using Services.Management.Administration.Worker;
    using xLibrary;
    using xLibrary.Actions;

    internal sealed class AdministratorHttpRequestHandler : xTagHttpHtmlAndRestHandler
    {
        private readonly WorkUnitContext workUnitContext;

        public const string AdminHomepageUriPath = "/administration/";

        private static string PathToContent = string.Format("{0}{1}Ui{1}Admin{1}Content{1}{2}", AppDomain.CurrentDomain.BaseDirectory, Path.DirectorySeparatorChar, "administration.xml");

        public AdministratorHttpRequestHandler(HttpServer httpServer, WorkUnitContext workUnitContext) :
            base(AdminHomepageUriPath, PathToContent, "Administration.HomePage", "Administration.HomePage.Body.Status.Rows")
        {
            this.workUnitContext = workUnitContext;
            httpServer.AddPath(AdminHomepageUriPath);

            httpServer.Modules.Add(this);
        }

        public override void Get(xTagContext xTagContext, bool isAjax)
        {
            var reportedData = workUnitContext.AdminServer.Do(new Send<GetReportedDataReturnData>(new GetReportedData()));
            var serviceReports = reportedData.Reports.Select(
                x => new ServiceReportItem
                     {
                         Id = x.Value.StartData.Id,
                         ServiceName = x.Value.StartData.ServiceName,
                         FormattedUptime = x.Value.Uptime.ToString(),
                     });

            xTagContext.Do(new Databind(serviceReports));
        }

        public override void Post(xTagContext xTagContext, bool isAjax)
        {
            throw new NotImplementedException();
        }

        public override void Put(xTagContext xTagContext, bool isAjax)
        {
            throw new NotImplementedException();
        }

        public override void Delete(xTagContext xTagContext, bool isAjax)
        {
            throw new NotImplementedException();
        }
    }
}
