namespace Services.Extensions.Ui
{
    using Chains;
    using Chains.Play.Web;
    using Chains.Play.Web.HttpListener;
    using Services.Extensions.Contexts.ServiceStarter;
    using Services.Extensions.Ui.Modules;
    using Services.Management.Administration.Worker;

    public sealed class AdministrationServer : Chain<AdministrationServer>
    {
        private const int HttpServerThreads = 3;

        private readonly HttpServer httpServer;

        private readonly ServiceStarterContext serviceStarterContext = new ServiceStarterContext();

        public AdministrationServer(string hostname, int port, WorkUnitContext workUnitContext)
        {
            httpServer =
                new ServerHost(hostname, port, AdministratorHttpRequestHandler.AdminHomepageUriPath).Do(
                    new StartHttpServer(new string[0], HttpServerThreads));

            new AdministratorHttpRequestHandler(httpServer, workUnitContext);

            // string.Format("{0}Ui{1}Admin{1}Content{1}", AppDomain.CurrentDomain.BaseDirectory, Path.DirectorySeparatorChar)
            new FileSystemHttpRequestHandler(httpServer);
        }
    }
}
