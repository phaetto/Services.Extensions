namespace Services.Extensions.Web
{
    using System;
    using System.Net;
    using Chains;
    using Chains.Play.Web.HttpListener;

    public abstract class HttpRestHandler : AbstractChain, IHttpRequestHandler
    {
        protected readonly string Path;

        protected readonly string DefaultDocument;

        protected HttpRestHandler(string path, string defaultDocument = null)
        {
            DefaultDocument = defaultDocument;
            Path = path.EndsWith("/") ? path.Substring(0, path.Length - 1) : path;
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
            return context.Request.Url.AbsolutePath.ToLowerInvariant() == Path
                || context.Request.Url.AbsolutePath.ToLowerInvariant() == Path + "/"
                || (!string.IsNullOrWhiteSpace(DefaultDocument) && context.Request.Url.AbsolutePath.ToLowerInvariant() == Path + "/" + DefaultDocument);
        }

        public abstract void Get(HttpListenerContext context);
        public abstract void Post(HttpListenerContext context);
        public abstract void Put(HttpListenerContext context);
        public abstract void Delete(HttpListenerContext context);
    }
}
