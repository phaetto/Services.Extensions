namespace Services.Extensions.Ui.Actions
{
    using Chains.Play;
    using Services.Extensions.Ui.Data;

    public sealed class GetAdminHostAndPortForUiServer : RemotableAction<GetAdminAndHostReturnData, AdministrationServer>
    {
        protected override GetAdminAndHostReturnData ActRemotely(AdministrationServer context)
        {
            return new GetAdminAndHostReturnData
                   {
                       Host = context.Hostname,
                       Port = context.Port,
                   };
        }
    }
}
