namespace Services.Extensions.Contexts.ServiceStarter
{
    using System.Collections.Generic;
    using Chains;
    using Services.Management.Administration.Worker;

    public sealed class UpdateStartingServices : IChainableAction<ServiceStarterContext, ServiceStarterContext>
    {
        private readonly List<StartWorkerData> workerDataToPersist;

        public UpdateStartingServices(List<StartWorkerData> workerDataToPersist)
        {
            this.workerDataToPersist = workerDataToPersist;
        }

        public ServiceStarterContext Act(ServiceStarterContext context)
        {
            context.PersistentServiceStarterContext.Do(new UpdatePersistentStarterData(workerDataToPersist));

            return context;
        }
    }
}
