namespace Services.Extensions.Contexts.ServiceStarter
{
    using System.IO;
    using System.Threading;
    using Chains;
    using Chains.Persistence;
    using Services.Communication.Protocol;
    using Services.Management.Administration.Server;
    using Services.Management.Administration.Worker;

    public sealed class ServiceStarterContext : Chain<ServiceStarterContext>
    {
        private readonly WorkUnitContext workUnitContext;

        internal readonly PersistentServiceStarterContext PersistentServiceStarterContext;

        public ServiceStarterContext(WorkUnitContext workUnitContext)
        {
            this.workUnitContext = workUnitContext;
            PersistentServiceStarterContext = new PersistentServiceStarterContext(
                new PersistentServiceStarterData
                {
                    Id = PersistentServiceStarterContext.DefaultIdForStartingOptions
                },
                new FilePersistentStoreWithMemorySnapshotCache<PersistentServiceStarterData>(
                    string.Format(PersistentServiceStarterContext.DataFolderUnformatted, Path.DirectorySeparatorChar)));

            new Thread(StartOtherServices).Start();
        }

        public ServiceStarterContext()
        {
        }

        private void StartOtherServices()
        {
            foreach (var serviceEntry in PersistentServiceStarterContext.Data.ServicesToStart)
            {
                workUnitContext.AdminServer.Do(new Send(new StartWorkerProcess(serviceEntry)));
                workUnitContext.LogLine("Starting service {0} with id {1}", serviceEntry.Id, serviceEntry.ServiceName);
            }

            workUnitContext.Stop();
        }
    }
}
