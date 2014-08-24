namespace Services.Extensions.Contexts.ServiceStarter
{
    using Chains.Persistence;

    internal sealed class PersistentServiceStarterContext : PersistentChain<PersistentServiceStarterContext, PersistentServiceStarterData>
    {
        public PersistentServiceStarterContext(PersistentServiceStarterData data, IPersistentStore<PersistentServiceStarterData> persistentStore)
            : base(data, persistentStore)
        {
        }
    }
}
