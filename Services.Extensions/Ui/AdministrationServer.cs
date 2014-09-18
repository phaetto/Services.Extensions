namespace Services.Extensions.Ui
{
    using System;
    using System.IO;
    using Chains;
    using Chains.Persistence;
    using Chains.Play.Web;
    using Chains.Play.Web.HttpListener;
    using Services.Extensions.Contexts.ServiceAutoManager;
    using Services.Extensions.Contexts.ServiceStarter;
    using Services.Extensions.Ui.Modules;
    using Services.Management.Administration.Worker;

    public sealed class AdministrationServer : Chain<AdministrationServer>, IDisposable
    {
        private const int HttpServerThreads = 3;

        private readonly HttpServer httpServer;

        private readonly PersistentServiceStarterContext persistentServiceStarterContext;

        private readonly PersistentAutoServiceContext persistentAutoServiceContext;

        public AdministrationServer(string hostname, int port, WorkUnitContext workUnitContext)
        {
            persistentServiceStarterContext = new PersistentServiceStarterContext(
                new PersistentServiceStarterData
                {
                    Id = PersistentServiceStarterContext.DefaultIdForStartingOptions
                },
                new FilePersistentStore<PersistentServiceStarterData>(
                    string.Format(PersistentServiceStarterContext.DataFolderUnformatted, Path.DirectorySeparatorChar)));

            persistentAutoServiceContext =
                new PersistentAutoServiceContext(
                    new FilePersistentStoreWithMemorySnapshotCache<PersistentAutoServiceData>(
                        string.Format(PersistentAutoServiceContext.DataFolderUnformatted, Path.DirectorySeparatorChar)));

            httpServer =
                new ServerHost(hostname, port, AdministratorHttpRequestHandler.AdminHomepageUriPath).Do(
                    new StartHttpServer(new string[0], HttpServerThreads));

            new AdministratorHttpRequestHandler(httpServer, workUnitContext, persistentServiceStarterContext);

            // string.Format("{0}Ui{1}Admin{1}Content{1}", AppDomain.CurrentDomain.BaseDirectory, Path.DirectorySeparatorChar)
            new FileSystemHttpRequestHandler(httpServer);
        }

        public void Dispose()
        {
            try
            {
                httpServer.Stop();
            }
            catch (Exception)
            {
            }
        }
    }
}
