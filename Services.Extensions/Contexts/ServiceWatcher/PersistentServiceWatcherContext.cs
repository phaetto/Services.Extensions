namespace Services.Extensions.Contexts.ServiceWatcher
{
    using Chains.Persistence;

    internal sealed class PersistentServiceWatcherContext : PersistentChain<PersistentServiceWatcherContext, PersistentServiceWatcherData>
    {
        // This must be the same for each admin, should not be scaled and can be used from any service.
        public const string DataFolderUnformatted =
            "..{0}..{0}..{0}..{0}repository{0}Services.Extensions{0}DataStore{0}Auto-Services";

        public PersistentServiceWatcherContext(IPersistentStore<PersistentServiceWatcherData> persistentStore)
            : base(persistentStore)
        {
        }

        public PersistentServiceWatcherContext(PersistentServiceWatcherData watcherData, IPersistentStore<PersistentServiceWatcherData> persistentStore)
            : base(watcherData, persistentStore)
        {
        }
    }
}
