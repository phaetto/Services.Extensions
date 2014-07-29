namespace Services.Extensions.Ui
{
    using System;
    using Chains;
    using Chains.Play.Web;
    using Chains.Play.Web.HttpListener;
    using Services.Communication.Protocol;
    using Services.Extensions.Ui.Actions;
    using Services.Extensions.Ui.Data;
    using Services.Extensions.Ui.Modules;
    using Services.Management.Administration.Worker;

    public sealed class WorkUnitServer : Chain<WorkUnitServer>
    {
        private const int HttpServerThreads = 3;

        private readonly HttpServer httpServer;

        public WorkUnitServer(WorkUnitContext workUnitContext)
        {
            try
            {
                var adminAndHostReturnData =
                    workUnitContext.AdminServer.Do(
                        new Send<GetAdminAndHostReturnData>(new GetAdminHostAndPortForUiServer()));

                httpServer =
                    new ServerHost(adminAndHostReturnData.Host, adminAndHostReturnData.Port).Do(
                        new StartHttpServer(new string[0], HttpServerThreads));

                httpServer.Modules.Add(new WorkUnitHttpModule(httpServer, workUnitContext));

                httpServer.Modules.Add(new FileSystemModule(httpServer));
            }
            catch (NotSupportedException)
            {
            }
        }
    }
}
