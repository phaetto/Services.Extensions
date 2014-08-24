namespace Services.Extensions.Contexts.ServiceStarter
{
    using System;
    using Chains;
    using Chains.Persistence;

    public sealed class ServiceStarterContext : Chain<ServiceStarterContext>
    {
        internal const string DefaultIdForStartingOptions = "default";

        internal readonly PersistentServiceStarterContext PersistentServiceStarterContext;

        // This must be the same for each admin, should not be scaled and can be used from any service.
        private readonly string dataFolder = AppDomain.CurrentDomain.BaseDirectory + "Data";

        public ServiceStarterContext()
        {
            PersistentServiceStarterContext =
                new PersistentServiceStarterContext(
                    new PersistentServiceStarterData { Id = DefaultIdForStartingOptions }, 
                    new FilePersistentStoreWithMemorySnapshotCache<PersistentServiceStarterData>(dataFolder));
        }
    }
}
