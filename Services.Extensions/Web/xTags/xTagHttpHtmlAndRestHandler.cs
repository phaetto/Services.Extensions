namespace Services.Extensions.Web.xTags
{
    using System;
    using System.Net;
    using Chains;
    using Chains.Play.Web;
    using Chains.Play.Web.HttpListener;
    using xLibrary;
    using xLibrary.Actions;

    public abstract class xTagHttpHtmlAndRestHandler : AbstractChain, IHttpRequestHandler
    {
        private readonly string libraryDocument;

        private readonly string tagName;

        private readonly string optionalRestTag;

        protected readonly string RequestPath;

        protected xTagHttpHtmlAndRestHandler(HttpServer httpServer, string path, string libraryDocument, string tagName, string optionalRestTag = null)
        {
            this.libraryDocument = libraryDocument;
            this.tagName = tagName;
            this.optionalRestTag = optionalRestTag;
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
                    var xContext = new xContext(new HttpContextInfo(context)).Do(new LoadLibrary(libraryDocument));

                    xContext.DoFirstNotNull(
                        xcontext =>
                            xcontext.Do(new CreateTag(optionalRestTag ?? tagName))
                                .Do(new CheckIfRestRequest(onGet: Get, onPost: Post, onPut: Put, onDelete: Delete)),
                        xcontext => xcontext.Do(new CreateTag(tagName)).Do(new RenderHtml()))
                        .ApplyOutputToHttpContext();

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
                || context.Request.Url.AbsolutePath.ToLowerInvariant() == RequestPath + "/";
        }

        public abstract void Get(xTagContext xTagContext, bool isAjax);
        public abstract void Post(xTagContext xTagContextxtag, bool isAjax);
        public abstract void Put(xTagContext xTagContext, bool isAjax);
        public abstract void Delete(xTagContext xTagContext, bool isAjax);
    }
}
