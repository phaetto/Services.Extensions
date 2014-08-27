namespace Services.Extensions.Contexts.ServiceStarter
{
    using System.Collections.Generic;
    using Chains;
    using Services.Management.Administration.Worker;

    internal sealed class UpdatePersistentStarterData : IChainableAction<PersistentServiceStarterContext, PersistentServiceStarterContext>
    {
        private readonly List<StartWorkerData> workerDataToPersist;

        public UpdatePersistentStarterData(List<StartWorkerData> workerDataToPersist)
        {
            this.workerDataToPersist = workerDataToPersist;
        }

        public PersistentServiceStarterContext Act(PersistentServiceStarterContext context)
        {
            context.Data.Id = PersistentServiceStarterContext.DefaultIdForStartingOptions;
            context.Data.ServicesToStart = workerDataToPersist;

            return context;
        }
    }
}
