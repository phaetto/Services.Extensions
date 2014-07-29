namespace Services.Extensions.Ui.Data
{
    using Chains.Play;

    public sealed class GetAdminAndHostReturnData : SerializableSpecification
    {
        public string Host;

        public int Port;

        public override int DataStructureVersionNumber
        {
            get
            {
                return 1;
            }
        }
    }
}
