namespace Services.Extensions.Ui.Modules.Administration
{
    using System;
    using System.IO;
    using Chains.Play.Web.HttpListener;
    using Services.Extensions.Ui.Admin;
    using Services.Extensions.Web.xTags;
    using Services.Management.Administration.Worker;
    using xLibrary;
    using xLibrary.Actions;

    internal sealed class AdministratorHttpRequestHandler : xTagHttpHtmlHandler
    {
        private readonly WorkUnitContext workUnitContext;

        public const string AdminHomepageUriPath = "/administration/";

        public static readonly string PathToContent = string.Format("{0}{1}Ui{1}Admin{1}Content{1}{2}", AppDomain.CurrentDomain.BaseDirectory, Path.DirectorySeparatorChar, "administration.xml");

        public AdministratorHttpRequestHandler(
            HttpServer httpServer,
            WorkUnitContext workUnitContext)
            : base(
                httpServer,
                AdminHomepageUriPath,
                PathToContent,
                "Administration.HomePage")
        {
            this.workUnitContext = workUnitContext;
        }

        protected override HttpResultContextWithxContext RenderHtmlTag(xContext xContext)
        {
            // TODO: use a configuration/persistence to store options
            var page = new Page
                       {
                           Header = new Header
                                    {
                                        HeadTitle =
                                            string.Format(
                                                "{0}:{1} - Microservices",
                                                workUnitContext.WorkerData.AdminHost,
                                                workUnitContext.WorkerData.AdminPort),
                                        Title = "Overview panel",
                                        Subtitle =
                                            string.Format(
                                                " @ {0}:{1}",
                                                workUnitContext.WorkerData.AdminHost,
                                                workUnitContext.WorkerData.AdminPort),
                                    },
                           Footer = new Footer
                                    {
                                        CopyrightName = "Modern software design // Alexander Mantzoukas // www.msd.am",
                                        ToYear = DateTime.UtcNow.Year,
                                        AboutText = "Spin and scale micro-services, brought to you by free software.",
                                    }
                       };

            return xContext.Do(new CreateTag(TagName)).Do(new Databind(page)).Do(new RenderHtml());
        }
    }
}
