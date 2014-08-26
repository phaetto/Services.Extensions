namespace Services.Extensions.Web.xTags
{
    using System;
    using System.Net;
    using Chains;
    using Chains.Play.Web;
    using Chains.Play.Web.HttpListener;
    using xLibrary;
    using xLibrary.Actions;

    public abstract class xTagHttpRestHandler : AbstractChain, IHttpRequestHandler
    {
        private readonly string libraryDocument;

        private readonly string tagName;

        protected readonly string RequestPath;

        protected xTagHttpRestHandler(HttpServer httpServer, string path, string libraryDocument, string tagName)
        {
            this.libraryDocument = libraryDocument;
            this.tagName = tagName;
            RequestPath = path.EndsWith("/") ? path.Substring(0, path.Length - 1) : path;
            httpServer.Modules.Add(this);
            httpServer.AddPath(RequestPath + "/");
        }

        public bool ResolveRequest(HttpListenerContext context)
        {
            try
            {
                if (CheckPathForMatch(context))
                {
                    var resultContext =
                        new xContext(new HttpContextInfo(context)).Do(new LoadLibrary(libraryDocument))
                            .Do(new CreateTag(tagName))
                            .Do(new CheckIfRestRequest(onGet: Get, onPost: Post, onPut: Put, onDelete: Delete));

                    if (resultContext != null)
                    {
                        resultContext.ApplyOutputToHttpContext();

                        return true;
                    }

                    return false;
                }
            }
            catch (NotSupportedException)
            {
            }

            return false;
        }

        private bool CheckPathForMatch(HttpListenerContext context)
        {
            return context.Request.Url.AbsolutePath.ToLowerInvariant() == RequestPath
                || context.Request.Url.AbsolutePath.ToLowerInvariant() == RequestPath + "/";
        }

        public abstract void Get(xTagContext xTagContext, bool isAjax);
        public abstract void Post(xTagContext xTagContext, bool isAjax);
        public abstract void Put(xTagContext xTagContext, bool isAjax);
        public abstract void Delete(xTagContext xTagContext, bool isAjax);
    }
}
