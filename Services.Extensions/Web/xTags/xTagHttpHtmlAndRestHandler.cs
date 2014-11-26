﻿namespace Services.Extensions.Web.xTags
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

        private readonly string optionalRestTag;

        private readonly bool skipCheckForId;

        protected readonly string TagName;

        protected readonly string RequestPath;

        protected xTagHttpHtmlAndRestHandler(HttpServer httpServer, string path, string libraryDocument, string tagName, string optionalRestTag = null, bool skipCheckForId = false)
        {
            this.libraryDocument = libraryDocument;
            this.TagName = tagName;
            this.optionalRestTag = optionalRestTag;
            this.skipCheckForId = skipCheckForId;
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
                            xcontext.Do(new CreateTag(optionalRestTag ?? TagName))
                                .Do(
                                    new CheckIfRestRequest(
                                        onGet: Get,
                                        onPost: Post,
                                        onPut: Put,
                                        onDelete: Delete,
                                        skipCheckForId: skipCheckForId)),
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

        public abstract void Get(xTagContext xTagContext, bool isAjax);
        public abstract void Post(xTagContext xTagContextxtag, bool isAjax);
        public abstract void Put(xTagContext xTagContext, bool isAjax);
        public abstract void Delete(xTagContext xTagContext, bool isAjax);
    }
}
