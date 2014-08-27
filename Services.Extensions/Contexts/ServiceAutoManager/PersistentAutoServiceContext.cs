namespace Services.Extensions.Contexts.ServiceAutoManager
{
    using Chains.Persistence;

    internal sealed class PersistentAutoServiceContext : PersistentChain<PersistentAutoServiceContext, PersistentAutoServiceData>
    {
        // This must be the same for each admin, should not be scaled and can be used from any service.
        public const string DataFolderUnformatted =
            "..{0}..{0}..{0}..{0}repository{0}Services.Extensions{0}DataStore{0}Auto-Services";

        public PersistentAutoServiceContext(IPersistentStore<PersistentAutoServiceData> persistentStore)
            : base(persistentStore)
        {
        }

        public PersistentAutoServiceContext(PersistentAutoServiceData data, IPersistentStore<PersistentAutoServiceData> persistentStore)
            : base(data, persistentStore)
        {
        }
    }
}
