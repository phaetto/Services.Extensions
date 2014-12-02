namespace Services.Extensions.Ui
{
    using System;
    using System.Linq;
    using System.Threading;
    using Chains;
    using Chains.Play.Web;
    using Chains.Play.Web.HttpListener;
    using Services.Communication.Protocol;
    using Services.Extensions.Ui.Modules;
    using Services.Management.Administration.Server;
    using Services.Management.Administration.Worker;

    public sealed class WorkUnitServer : Chain<WorkUnitServer>, IDisposable
    {
        private const int HttpServerThreads = 3;

        private readonly HttpServer httpServer;

        public const string ServiceHomepageUriPath = "{0}services/{1}/";

        public WorkUnitServer(WorkUnitContext workUnitContext)
        {
            try
            {
                ServerHost serverData;
                while(true)
                {
                    var reportedData = workUnitContext.AdminServer.Do(new Send<GetReportedDataReturnData>(new GetReportedData()));

                    if (reportedData.Reports.Any(x => x.Value.StartData.ContextType != null
                        && x.Value.StartData.ContextType.IndexOf(typeof(AdministrationServer).FullName) != -1))
                    {
                        var adminData =
                            reportedData.Reports.First(
                                x => x.Value.StartData.ContextType.IndexOf(typeof(AdministrationServer).FullName) != -1)
                                .Value.StartData;


                        serverData = new ServerHost(adminData.ContextServerHost, adminData.ContextServerPort, adminData.ContextHttpData.Path);

                        break;
                    }

                    Thread.Sleep(2000);
                }

                var currentServiceUriPath =
                    Uri.EscapeUriString(
                        string.Format(ServiceHomepageUriPath, serverData.Parent.Path, workUnitContext.WorkerData.Id));

                // Get the options that have started the server, and use them
                httpServer =
                    new ServerHost(serverData.Parent.Hostname, serverData.Parent.Port)
                        .Do(new StartHttpServer(new string[0], HttpServerThreads));

                new WorkUnitHttpRequestHandler(httpServer, workUnitContext, currentServiceUriPath);

                //new FileSystemHttpRequestHandler(httpServer); // Needs a pathPrefix or change of the root folder
            }
            catch (NotSupportedException)
            {
            }
        }

        public void Dispose()
        {
            try
            {
                httpServer.Stop();
            }
            catch
            {
            }
        }
    }
}
