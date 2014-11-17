namespace Services.Extensions.Contexts.ServiceWatcher
{
    using Chains.Persistence;

    internal sealed class PersistentServiceWatcherData : SerializableSpecificationWithId
    {
        public bool RestartOnFail = true;

        public int MaximumRestartTimes = 5;

        public int LowerBoundInSecondsThatServiceShouldNotBeRestarted = 30;

        public string EmailListResponsibleForThisService = null;

        public override int DataStructureVersionNumber
        {
            get
            {
                return 1;
            }
        }
    }
}
