namespace Services.Extensions.Web
{
    using System;
    using System.Net;
    using Chains;
    using Chains.Play.Web.HttpListener;

    public abstract class HttpRestHandler : AbstractChain, IHttpRequestHandler
    {
        protected readonly string RequestPath;

        protected readonly string DefaultDocument;

        protected HttpRestHandler(HttpServer httpServer, string path, string defaultDocument = null)
        {
            DefaultDocument = defaultDocument;
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
                    switch (context.Request.HttpMethod.ToLower())
                    {
                        case "get":
                            Get(context);
                            break;
                        case "post":
                            Post(context);
                            break;
                        case "put":
                            Put(context);
                            break;
                        case "delete":
                            Delete(context);
                            break;
                    }

                    return true;
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
                || context.Request.Url.AbsolutePath.ToLowerInvariant() == RequestPath + "/"
                || (!string.IsNullOrWhiteSpace(DefaultDocument) && context.Request.Url.AbsolutePath.ToLowerInvariant() == RequestPath + "/" + DefaultDocument);
        }

        public abstract void Get(HttpListenerContext context);
        public abstract void Post(HttpListenerContext context);
        public abstract void Put(HttpListenerContext context);
        public abstract void Delete(HttpListenerContext context);
    }
}
