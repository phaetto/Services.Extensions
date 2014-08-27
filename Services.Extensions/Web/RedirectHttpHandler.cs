namespace Services.Extensions.Web
{
    using System.Net;
    using Chains;
    using Chains.Play.Web;
    using Chains.Play.Web.HttpListener;

    public sealed class RedirectHttpHandler : Chain<RedirectHttpHandler>, IHttpRequestHandler
    {
        private readonly string redirectAddressTo;

        public RedirectHttpHandler(HttpServer svfServer, string redirectAddressTo, string path = "/")
        {
            this.redirectAddressTo = redirectAddressTo;
            svfServer.Modules.Add(this);
            svfServer.AddPath(path);
        }

        public bool ResolveRequest(HttpListenerContext context)
        {
            new HttpResultContext(redirectAddressTo, false).ApplyOutputToHttpContext(context);

            return true;
        }
    }
}
