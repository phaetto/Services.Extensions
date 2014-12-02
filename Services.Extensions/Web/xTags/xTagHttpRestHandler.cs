namespace Services.Extensions.Web.xTags
{
    using System;
    using System.Net;
    using Chains;
    using Chains.Play.Web;
    using Chains.Play.Web.HttpListener;
    using xLibrary;
    using xLibrary.Actions;

    public abstract class xTagHttpRestHandler : HttpHandlerBase
    {
        protected readonly string LibraryDocument;

        protected readonly string TagName;

        protected readonly bool SkipCheckForId;

        protected xTagHttpRestHandler(
            HttpServer httpServer,
            string path,
            string libraryDocument,
            string tagName,
            bool skipCheckForId = false,
            bool allowToSharePath = false)
            : base(httpServer, path, allowToSharePath: allowToSharePath)
        {
            this.LibraryDocument = libraryDocument;
            this.TagName = tagName;
            this.SkipCheckForId = skipCheckForId;
        }

        public override bool ResolveRequest(HttpListenerContext context)
        {
            try
            {
                if (CheckPathForMatch(context))
                {
                    var resultContext =
                        new xContext(new HttpContextInfo(context)).Do(new LoadLibrary(LibraryDocument))
                            .Do(new CreateTag(TagName))
                            .Do(new CheckIfRestRequest(onGet: Get, onPost: Post, onPut: Put, onDelete: Delete, skipCheckForId: SkipCheckForId));

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

        public abstract void Get(xTagContext xTagContext, bool isAjax);
        public abstract void Post(xTagContext xTagContextxtag, bool isAjax);
        public abstract void Put(xTagContext xTagContext, bool isAjax);
        public abstract void Delete(xTagContext xTagContext, bool isAjax);
    }
}
