namespace Services.Extensions.Ui.Modules
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Net;
    using Chains.Play.Web;
    using Chains.Play.Web.HttpListener;
    using Newtonsoft.Json;
    using Services.Communication.Protocol;
    using Services.Extensions.Contexts.ServiceStarter;
    using Services.Extensions.Ui.Admin;
    using Services.Extensions.Web;
    using Services.Management.Administration.Server;
    using Services.Management.Administration.Worker;

    internal sealed class AdministratorRestRequestHandler : HttpRestHandler
    {
        private readonly WorkUnitContext workUnitContext;

        private readonly PersistentServiceStarterContext persistentServiceStarterContext;

        public const string AdminRestUriPath = "/administration/data";

        public AdministratorRestRequestHandler(
            HttpServer httpServer,
            WorkUnitContext workUnitContext,
            PersistentServiceStarterContext persistentServiceStarterContext)
            : base(httpServer, AdminRestUriPath)
        {
            this.workUnitContext = workUnitContext;
            this.persistentServiceStarterContext = persistentServiceStarterContext;
        }

        public override void Get(HttpListenerContext context)
        {
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
                    FormattedUptime = x.Value.Uptime.ToString(),
                    IsAutoStarting = autoStartingServices.Any(y => y == x.Value.StartData.Id)
                });

            new HttpResultContext(JsonConvert.SerializeObject(serviceReports), "text/json")
                .AccessControlAllowOriginAll()
                .CompressRequest()
                .ApplyOutputToHttpContext(context);
        }

        public override void Post(HttpListenerContext context)
        {
            var httpContextInfo = new HttpContextInfo(context);
            var id = httpContextInfo.Form["serviceId"];

            if (string.IsNullOrWhiteSpace(id))
            {
                HttpResultContext.NotFound().ApplyOutputToHttpContext(context);
                return;
            }

            id = Uri.UnescapeDataString(Uri.UnescapeDataString(id));

            var reportedData = workUnitContext.AdminServer.Do(new Send<GetReportedDataReturnData>(new GetReportedData()));

            if (persistentServiceStarterContext.Data.ServicesToStart.Any(x => x.Id == id))
            {
                persistentServiceStarterContext.Data.ServicesToStart.Remove(
                    persistentServiceStarterContext.Data.ServicesToStart.First(x => x.Id == id));
            }
            else
            {
                persistentServiceStarterContext.Data.ServicesToStart.Add(
                    reportedData.Reports.First(x => x.Value.StartData.Id == id).Value.StartData);
            }

            persistentServiceStarterContext.Do(
                new UpdatePersistentStarterData(persistentServiceStarterContext.Data.ServicesToStart));

            HttpResultContext.NoContent().ApplyOutputToHttpContext(context);
        }

        public override void Put(HttpListenerContext context)
        {
            throw new NotImplementedException();
        }

        public override void Delete(HttpListenerContext context)
        {
            throw new NotImplementedException();
        }
    }
}
