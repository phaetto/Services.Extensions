namespace Services.Extensions.Ui.Modules
{
    using System;
    using System.IO;
    using System.Net;
    using Chains;
    using Chains.Play.Web.HttpListener;
    using Services.Management.Administration.Worker;

    public sealed class WorkUnitHttpModule : Chain<WorkUnitHttpModule>, IHttpRequestHandler
    {
        public const string ServiceHomepageUriPath = "/administration/services/{0}/";

        public const string ServiceContentPath = "/ui/content/";

        private readonly string PathToContent = String.Format("{0}{1}Ui{1}Content{1}", AppDomain.CurrentDomain.BaseDirectory, Path.DirectorySeparatorChar);

        private readonly string CurrentServiceUriPath;

        public WorkUnitHttpModule(HttpServer httpServer, WorkUnitContext workUnitContext)
        {
            CurrentServiceUriPath =
                Uri.EscapeUriString(string.Format(ServiceHomepageUriPath, workUnitContext.WorkerData.Id));

            httpServer.AddPath(CurrentServiceUriPath);
        }

        public bool ResolveRequest(HttpListenerContext context)
        {
            return false;
        }
    }
}
