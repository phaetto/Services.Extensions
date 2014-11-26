namespace Services.Extensions.Ui.Modules.Administration
{
    using System;
    using System.Linq;
    using Chains.Play.Web.HttpListener;
    using Services.Communication.Protocol;
    using Services.Extensions.Contexts.ServiceStarter;
    using Services.Extensions.Web.xTags;
    using Services.Management.Administration.Server;
    using Services.Management.Administration.Worker;
    using xLibrary;

    internal sealed class AutoStartServiceRequestHandler : xTagHttpRestHandler
    {
        private readonly WorkUnitContext workUnitContext;

        private readonly PersistentServiceStarterContext persistentServiceStarterContext;

        private const string autoStartServiceButtonUriPath = "/administration/buttons/auto-start-service";

        public AutoStartServiceRequestHandler(HttpServer httpServer,
            WorkUnitContext workUnitContext,
            PersistentServiceStarterContext persistentServiceStarterContext)
            : base(
                httpServer,
                autoStartServiceButtonUriPath,
                AdministratorHttpRequestHandler.PathToContent,
                "Administration.HomePage.Buttons.AutoStartService",
                true)
        {
            this.workUnitContext = workUnitContext;
            this.persistentServiceStarterContext = persistentServiceStarterContext;
        }

        public override void Get(xTagContext xTagContext, bool isAjax)
        {
            throw new NotSupportedException();
        }

        public override void Post(xTagContext xTagContext, bool isAjax)
        {
            if (!isAjax)
            {
                return;
            }

            if (!xTagContext.xTag.Data.ContainsKey("serviceId"))
            {
                return;
            }

            var id = xTagContext.xTag.Data["serviceId"];
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
        }

        public override void Put(xTagContext xTagContext, bool isAjax)
        {
            throw new System.NotSupportedException();
        }

        public override void Delete(xTagContext xTagContext, bool isAjax)
        {
            throw new System.NotSupportedException();
        }
    }
}
