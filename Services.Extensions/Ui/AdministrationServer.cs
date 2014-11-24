namespace Services.Extensions.Ui
{
    using System.IO;
    using Chains;
    using Chains.Persistence;
    using Chains.Play.Web.HttpListener;
    using Services.Extensions.Contexts.ServiceStarter;
    using Services.Extensions.Ui.Modules;
    using Services.Extensions.Ui.Modules.Administration;
    using Services.Management.Administration.Worker;

    public sealed class AdministrationServer : Chain<AdministrationServer>
    {
        private readonly PersistentServiceStarterContext persistentServiceStarterContext;

        public AdministrationServer(WorkUnitContext workUnitContext)
        {
            persistentServiceStarterContext = new PersistentServiceStarterContext(
                new PersistentServiceStarterData
                {
                    Id = PersistentServiceStarterContext.DefaultIdForStartingOptions
                },
                new FilePersistentStore<PersistentServiceStarterData>(
                    string.Format(PersistentServiceStarterContext.DataFolderUnformatted, Path.DirectorySeparatorChar)));

            var httpServer = workUnitContext.ContextServer.ServerProtocolStack.ServerProvider as HttpServer;

            if (httpServer != null)
            {
                new AdministratorHttpRequestHandler(httpServer, workUnitContext, persistentServiceStarterContext);

                new StartServiceRequestHandler(httpServer, workUnitContext);
                new StopServiceRequestHandler(httpServer, workUnitContext);
                new DeleteServiceRequestHandler(httpServer, workUnitContext);
                new AutoStartServiceRequestHandler(httpServer, workUnitContext, persistentServiceStarterContext);

                // string.Format("{0}Ui{1}Admin{1}Content{1}", AppDomain.CurrentDomain.BaseDirectory, Path.DirectorySeparatorChar)
                new FileSystemHttpRequestHandler(httpServer);
            }
            else
            {
                workUnitContext.LogLine("This module only works when http sharing is on. Nothing to do... Closing...");
                workUnitContext.ReportToAdmin();
                workUnitContext.Stop();
            }
        }
    }
}
