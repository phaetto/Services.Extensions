namespace Services.Extensions.Ui.Modules.Administration
{
    using System;
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

    internal sealed class RowsHttpRequestHandler : xTagHttpRestHandler
    {
        private readonly WorkUnitContext workUnitContext;

        private readonly PersistentServiceStarterContext persistentServiceStarterContext;

        public const string RowsHomepageUriPath = "/administration/dashboard/rows";

        public RowsHttpRequestHandler(
            HttpServer httpServer,
            WorkUnitContext workUnitContext,
            PersistentServiceStarterContext persistentServiceStarterContext)
            : base(
                httpServer,
                RowsHomepageUriPath,
                AdministratorHttpRequestHandler.PathToContent,
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
            throw new NotSupportedException();
        }

        public override void Put(xTagContext xTagContext, bool isAjax)
        {
            throw new NotSupportedException();
        }

        public override void Delete(xTagContext xTagContext, bool isAjax)
        {
            throw new NotSupportedException();
        }
    }
}
