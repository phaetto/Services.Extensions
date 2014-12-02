namespace Services.Extensions.Web.xTags
{
    using System;
    using System.Net;
    using Chains;
    using Chains.Play.Web;
    using Chains.Play.Web.HttpListener;
    using xLibrary;
    using xLibrary.Actions;

    public abstract class xTagHttpHtmlHandler : HttpHandlerBase
    {
        protected readonly string LibraryDocument;

        protected readonly string TagName;

        protected xTagHttpHtmlHandler(
            HttpServer httpServer,
            string path,
            string libraryDocument,
            string tagName,
            bool allowToSharePath = false)
            : base(httpServer, path, allowToSharePath: allowToSharePath)
        {
            this.LibraryDocument = libraryDocument;
            this.TagName = tagName;
        }

        public override bool ResolveRequest(HttpListenerContext context)
        {
            try
            {
                if (CheckPathForMatch(context))
                {
                    RenderHtmlTag(new xContext(new HttpContextInfo(context)).Do(new LoadLibrary(LibraryDocument)))
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
    }
}
