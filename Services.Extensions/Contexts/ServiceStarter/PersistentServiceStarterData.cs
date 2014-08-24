namespace Services.Extensions.Contexts.ServiceStarter
{
    using System.Collections.Generic;
    using Chains.Persistence;
    using Services.Management.Administration.Worker;

    internal sealed class PersistentServiceStarterData : SerializableSpecificationWithId
    {
        public List<StartWorkerData> ServicesToStart = new List<StartWorkerData>();

        public override int DataStructureVersionNumber
        {
            get
            {
                return 1;
            }
        }
    }
}
