namespace Services.Extensions.Ui.Modules.Administration
{
    using System;
    using System.IO;
    using System.Linq;
    using Chains.Play.Web.HttpListener;
    using Newtonsoft.Json;
    using Services.Communication.Protocol;
    using Services.Extensions.Contexts.ServiceStarter;
    using Services.Extensions.Ui.Admin;
    using Services.Extensions.Web.xTags;
    using Services.Management.Administration.Server;
    using Services.Management.Administration.Worker;
    using xLibrary;
    using xLibrary.Actions;

    internal sealed class AdministratorHttpRequestHandler : xTagHttpHtmlAndRestHandler
    {
        private readonly WorkUnitContext workUnitContext;

        private readonly PersistentServiceStarterContext persistentServiceStarterContext;

        public const string AdminHomepageUriPath = "/administration/";

        public static readonly string PathToContent = string.Format("{0}{1}Ui{1}Admin{1}Content{1}{2}", AppDomain.CurrentDomain.BaseDirectory, Path.DirectorySeparatorChar, "administration.xml");

        public AdministratorHttpRequestHandler(
            HttpServer httpServer,
            WorkUnitContext workUnitContext,
            PersistentServiceStarterContext persistentServiceStarterContext)
            : base(
                httpServer,
                AdminHomepageUriPath,
                PathToContent,
                "Administration.HomePage",
                "Administration.HomePage.Body.Status.Rows")
        {
            this.workUnitContext = workUnitContext;
            this.persistentServiceStarterContext = persistentServiceStarterContext;
        }

        public override void Get(xTagContext xTagContext, bool isAjax)
        {
            if (!isAjax)
            {
                return;
            }

            var reportedData = workUnitContext.AdminServer.Do(new Send<GetReportedDataReturnData>(new GetReportedData()));

            var autoStartingServices =
                reportedData.Reports.Where(
                    x => persistentServiceStarterContext.Data.ServicesToStart.Any(y => y.Id == x.Value.StartData.Id))
                    .Select(x => x.Value.StartData.Id);

            var serviceReports = reportedData.Reports.Select(
                x => new ServiceReportItem
                {
                    Id = x.Value.StartData.Id,
                    ServiceName = x.Value.StartData.ServiceName,
                    FormattedUptime = x.Value.Uptime.ToString(@"dd\.hh\:mm\:ss"),
                    IsAutoStarting = autoStartingServices.Any(y => y == x.Value.StartData.Id),
                    State = x.Value.WorkerState.ToString()
                });

            xTagContext.xTag.Data["runtimeApplicationData"] = JsonConvert.SerializeObject(serviceReports);
        }

        public override void Post(xTagContext xTagContext, bool isAjax)
        {
            new NotImplementedException();
        }

        public override void Put(xTagContext xTagContext, bool isAjax)
        {
            throw new NotImplementedException();
        }

        public override void Delete(xTagContext xTagContext, bool isAjax)
        {
            throw new NotImplementedException();
        }

        private static void Databind(
            xTagContext xTagContext,
            GetReportedDataReturnData reportedData,
            PersistentServiceStarterContext persistentServiceStarterContext)
        {
            var autoStartingServices =
                reportedData.Reports.Where(
                    x => persistentServiceStarterContext.Data.ServicesToStart.Any(y => y.Id == x.Value.StartData.Id))
                    .Select(x => x.Value.StartData.Id);

            var serviceReports = reportedData.Reports.Select(
                x => new ServiceReportItem
                {
                    Id = x.Value.StartData.Id,
                    ServiceName = x.Value.StartData.ServiceName,
                    FormattedUptime = x.Value.Uptime.ToString(),
                    IsAutoStarting = autoStartingServices.Any(y => y == x.Value.StartData.Id)
                });

            xTagContext.Do(new Databind(serviceReports));
        }
    }
}
