namespace Services.Extensions.Ui.Modules
{
    using System;
    using System.Net;
    using Chains.Play.Web.HttpListener;
    using Services.Extensions.Web;
    using Services.Management.Administration.Worker;

    internal sealed class WorkUnitHttpRequestHandler : HttpRestHandler
    {
        public const string ServiceContentPath = "/ui/service/content/";

        public WorkUnitHttpRequestHandler(HttpServer httpServer, WorkUnitContext workUnitContext, string path)
            : base(path)
        {
            httpServer.AddPath(path);

            httpServer.Modules.Add(this);
        }

        public override void Get(HttpListenerContext context)
        {
            throw new NotImplementedException();
        }

        public override void Post(HttpListenerContext context)
        {
            throw new NotImplementedException();
        }

        public override void Put(HttpListenerContext context)
        {
            throw new NotImplementedException();
        }

        public override void Delete(HttpListenerContext context)
        {
            throw new NotImplementedException();
        }
    }
}
