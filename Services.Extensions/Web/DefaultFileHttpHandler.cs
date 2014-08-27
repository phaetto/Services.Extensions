namespace Services.Extensions.Web
{
    using System;
    using System.IO;
    using System.Net;
    using Chains.Play.Web;
    using Chains.Play.Web.HttpListener;

    public sealed class DefaultFileHttpHandler : HttpRestHandler
    {
        private readonly string defaultFile;

        private readonly string directory;

        public DefaultFileHttpHandler(HttpServer httpServer, string defaultFile = "index.html", string path = "/", string directory = null)
            : base(httpServer, path, defaultFile)
        {
            this.defaultFile = defaultFile;
            this.directory = directory;
        }

        public override void Get(HttpListenerContext context)
        {
            var path = string.Format("{0}{1}{2}", directory, Path.DirectorySeparatorChar, defaultFile);

            new HttpResultContext().SendFile(path)
                .AccessControlAllowOriginAll()
                .ApplyOutputToHttpContext(context);
        }

        public override void Post(HttpListenerContext context)
        {
            throw new NotImplementedException();
        }

        public override void Put(HttpListenerContext context)
        {
            throw new NotImplementedException();
        }

        public override void Delete(HttpListenerContext context)
        {
            throw new NotImplementedException();
        }
    }
}
