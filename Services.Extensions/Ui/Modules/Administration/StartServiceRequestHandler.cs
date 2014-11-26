namespace Services.Extensions.Ui.Modules.Administration
{
    using System;
    using Chains.Play;
    using Chains.Play.Web.HttpListener;
    using Services.Communication.Protocol;
    using Services.Extensions.Web.xTags;
    using Services.Management.Administration.Server;
    using Services.Management.Administration.Worker;
    using xLibrary;

    public sealed class StartServiceRequestHandler : xTagHttpRestHandler
    {
        private readonly WorkUnitContext workUnitContext;

        private const string startServiceButtonUriPath = "/administration/buttons/start-service";

        public StartServiceRequestHandler(HttpServer httpServer,
            WorkUnitContext workUnitContext)
            : base(
                httpServer,
                startServiceButtonUriPath,
                AdministratorHttpRequestHandler.PathToContent,
                "Administration.HomePage.Buttons.StartService",
                true)
        {
            this.workUnitContext = workUnitContext;
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

            var serviceId = xTagContext.xTag.Data["serviceId"];

            StartWorkerData data = null;
            if (!xTagContext.xTag.Data.ContainsKey("serviceSpecification"))
            {
                var reportedData =
                    workUnitContext.AdminServer.Do(
                        new Send<GetReportedDataReturnData>(
                            new GetReportedData()
                            {
                                ApiKey = xTagContext.xTag.ApiKey,
                                Session = xTagContext.xTag.Session,
                            }));

                data = reportedData.Reports[serviceId].StartData;
            }
            else
            {
                // Only starts service that already was started before.
                return;
            }

            workUnitContext.AdminServer.Do(
                new Send(
                    new StartWorkerProcess(data)
                    {
                        ApiKey = xTagContext.xTag.ApiKey,
                        Session = xTagContext.xTag.Session,
                    }));
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
