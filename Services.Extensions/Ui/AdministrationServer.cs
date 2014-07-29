namespace Services.Extensions.Ui
{
    using Chains;
    using Chains.Play.Web;
    using Chains.Play.Web.HttpListener;
    using Services.Extensions.Ui.Modules;
    using Services.Management.Administration.Server;

    public sealed class AdministrationServer : Chain<AdministrationServer>
    {
        public readonly string Hostname;

        public readonly int Port;

        private const int HttpServerThreads = 3;

        private readonly HttpServer httpServer;

        public AdministrationServer(AdministrationContext administrationContext)
            : this("localhost", 80, administrationContext)
        {
        }

        public AdministrationServer(string hostname, int port, AdministrationContext administrationContext)
        {
            this.Hostname = hostname;
            this.Port = port;

            httpServer =
                new ServerHost(hostname, port, AdministratorHttpRequestHandler.AdminHomepageUriPath).Do(
                    new StartHttpServer(new string[0], HttpServerThreads));

            new AdministratorHttpRequestHandler(httpServer, administrationContext);
            new FileSystemModule(httpServer);
        }
    }
}
