namespace Services.Extensions.Web.xTags
{
    using System;
    using System.Net;
    using Chains.Play.Web;
    using Chains.Play.Web.HttpListener;
    using xLibrary;
    using xLibrary.Actions;

    public abstract class xTagHttpHtmlAndRestHandler : xTagHttpRestHandler
    {
        private readonly string optionalRestTag;

        protected xTagHttpHtmlAndRestHandler(
            HttpServer httpServer,
            string path,
            string libraryDocument,
            string tagName,
            string optionalRestTag = null,
            bool skipCheckForId = false,
            bool allowToSharePath = false)
            : base(httpServer, path, libraryDocument, tagName, skipCheckForId, allowToSharePath)
        {
            this.optionalRestTag = optionalRestTag;
        }

        public override bool ResolveRequest(HttpListenerContext context)
        {
            try
            {
                if (CheckPathForMatch(context))
                {
                    var xContext = new xContext(new HttpContextInfo(context)).Do(new LoadLibrary(LibraryDocument));

                    xContext.DoFirstNotNull(
                        xcontext =>
                            xcontext.Do(new CreateTag(optionalRestTag ?? TagName))
                                .Do(
                                    new CheckIfRestRequest(
                                        onGet: Get,
                                        onPost: Post,
                                        onPut: Put,
                                        onDelete: Delete,
                                        skipCheckForId: SkipCheckForId)),
                        RenderHtmlTag).ApplyOutputToHttpContext();

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
