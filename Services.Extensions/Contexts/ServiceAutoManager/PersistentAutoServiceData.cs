namespace Services.Extensions.Contexts.ServiceAutoManager
{
    using Chains.Persistence;

    internal sealed class PersistentAutoServiceData : SerializableSpecificationWithId
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
