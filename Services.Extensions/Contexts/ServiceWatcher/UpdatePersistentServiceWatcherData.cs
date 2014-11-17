namespace Services.Extensions.Contexts.ServiceWatcher
{
    using Chains;

    internal sealed class UpdatePersistentServiceWatcherData : IChainableAction<PersistentServiceWatcherContext, PersistentServiceWatcherContext>
    {
        private readonly string serviceId;

        public UpdatePersistentServiceWatcherData(string serviceId)
        {
            this.serviceId = serviceId;
        }

        public PersistentServiceWatcherContext Act(PersistentServiceWatcherContext watcherContext)
        {
            watcherContext.Data.Id = serviceId;

            return watcherContext;
        }
    }
}
