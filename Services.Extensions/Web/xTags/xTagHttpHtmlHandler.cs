namespace Services.Extensions.Web.xTags
{
    using System;
    using System.Net;
    using Chains;
    using Chains.Play.Web;
    using Chains.Play.Web.HttpListener;
    using xLibrary;
    using xLibrary.Actions;

    public abstract class xTagHttpHtmlHandler : AbstractChain, IHttpRequestHandler
    {
        private readonly string libraryDocument;

        protected readonly string TagName;

        protected readonly string RequestPath;

        protected xTagHttpHtmlHandler(HttpServer httpServer, string path, string libraryDocument, string tagName)
        {
            this.libraryDocument = libraryDocument;
            this.TagName = tagName;
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
                    RenderHtmlTag(new xContext(new HttpContextInfo(context)).Do(new LoadLibrary(libraryDocument)))
                        .ApplyOutputToHttpContext();

                    return true;
                }
            }
            catch (NotSupportedException)
            {
            }

            return false;
        }

        protected virtual HttpResultContextWithxContext RenderHtmlTag(xContext xContext)
        {
            return xContext.Do(new CreateTag(TagName)).Do(new RenderHtml());
        }

        private bool CheckPathForMatch(HttpListenerContext context)
        {
            return context.Request.Url.AbsolutePath.ToLowerInvariant() == RequestPath
                || context.Request.Url.AbsolutePath.ToLowerInvariant() == RequestPath + "/";
        }
    }
}
