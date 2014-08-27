namespace Services.Extensions.Contexts.ServiceAutoManager
{
    using Chains;

    internal sealed class UpdatePersistentAutoServiceData : IChainableAction<PersistentAutoServiceContext, PersistentAutoServiceContext>
    {
        private readonly string serviceId;

        public UpdatePersistentAutoServiceData(string serviceId)
        {
            this.serviceId = serviceId;
        }

        public PersistentAutoServiceContext Act(PersistentAutoServiceContext context)
        {
            context.Data.Id = serviceId;

            return context;
        }
    }
}
