namespace Services.Extensions.Ui.Modules.Administration
{
    using System;
    using Chains.Play.Web.HttpListener;
    using Services.Communication.Protocol;
    using Services.Extensions.Web.xTags;
    using Services.Management.Administration.Server;
    using Services.Management.Administration.Worker;
    using xLibrary;

    public sealed class StopServiceRequestHandler : xTagHttpRestHandler
    {
        private readonly WorkUnitContext workUnitContext;

        private const string stopServiceButtonUriPath = "/administration/buttons/stop-service";

        public StopServiceRequestHandler(HttpServer httpServer,
            WorkUnitContext workUnitContext)
            : base(
                httpServer,
                stopServiceButtonUriPath,
                AdministratorHttpRequestHandler.PathToContent,
                "Administration.HomePage.Buttons.StopService",
                true)
        {
            this.workUnitContext = workUnitContext;
        }

        public override void Get(xTagContext xTagContext, bool isAjax)
        {
            throw new NotImplementedException();
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

            workUnitContext.AdminServer.Do(new Send(new StopWorkerProcess(serviceId)
                                                    {
                                                        ApiKey = xTagContext.xTag.ApiKey,
                                                        Session = xTagContext.xTag.Session,
                                                    }));
        }

        public override void Put(xTagContext xTagContext, bool isAjax)
        {
            throw new System.NotImplementedException();
        }

        public override void Delete(xTagContext xTagContext, bool isAjax)
        {
            throw new System.NotImplementedException();
        }
    }
}
