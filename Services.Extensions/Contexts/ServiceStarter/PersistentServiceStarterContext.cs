namespace Services.Extensions.Contexts.ServiceStarter
{
    using Chains.Persistence;

    internal sealed class PersistentServiceStarterContext : PersistentChain<PersistentServiceStarterContext, PersistentServiceStarterData>
    {
        public const string DefaultIdForStartingOptions = "data";

        // This must be the same for each admin, should not be scaled and can be used from any service.
        public const string DataFolderUnformatted =
            "..{0}..{0}..{0}..{0}repository{0}Services.Extensions{0}DataStore{0}Starting-Services";

        public PersistentServiceStarterContext(PersistentServiceStarterData data, IPersistentStore<PersistentServiceStarterData> persistentStore)
            : base(data, persistentStore)
        {
        }
    }
}
