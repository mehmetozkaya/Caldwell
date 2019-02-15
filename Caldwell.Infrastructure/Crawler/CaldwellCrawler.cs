using Caldwell.Core.Crawler;
using Caldwell.Infrastructure.Crawler.Downloader;
using Caldwell.Infrastructure.Crawler.Pipeline;
using Caldwell.Infrastructure.Crawler.Processor;
using Caldwell.Infrastructure.Crawler.Request;
using Caldwell.Infrastructure.Crawler.Scheduler;
using HtmlAgilityPack;
using HtmlAgilityPack.CssSelectors.NetCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Caldwell.Infrastructure.Crawler
{
    public class CaldwellCrawler : ICaldwellCrawler
    {
        // Crawler Steps -- visual -> https://github.com/dotnetcore/DotnetSpider
        // Add Request - Url
        // Add Downloader
        // Add Processor - BasePageProcessor - hmtl reading
        // Add EntityType<BaiduSearchEntry>();
        // Add Pipeline(new ConsoleEntityPipeline()); -- insert db

        public ICaldwellRequest Request { get; private set; }
        public ICaldwellDownloader Downloader { get; private set; }
        public ICaldwellProcessor Processor { get; private set; }
        public ICaldwellScheduler Scheduler { get; private set; }
        public ICaldwellPipeline Pipeline { get; private set; }

        public CaldwellCrawler()
        {
        }

        public CaldwellCrawler AddRequest(ICaldwellRequest request)
        {
            Request = request;
            return this;
        }

        public CaldwellCrawler AddDownloader(ICaldwellDownloader downloader)
        {
            Downloader = downloader;            
            return this;
        }

        public CaldwellCrawler AddProcessor(ICaldwellProcessor processor)
        {
            Processor = processor;
            return this;
        }

        public CaldwellCrawler AddScheduler(ICaldwellScheduler scheduler)
        {
            Scheduler = scheduler;
            return this;
        }

        public CaldwellCrawler AddPipeline(ICaldwellPipeline pipeline)
        {
            Pipeline = pipeline;
            return this;
        }

        public async Task Crawle()
        {
            // Getlinks filtered by Regex
            //Request.Regex
            
            var linkReader = new PageLinkReader(Request);
            var links = await linkReader.GetLinks(0);

            //var document = await Downloader.Download(Request.Url);
            //var catalog = await Processor.Process(document);
            //await Pipeline.Run(catalog);
        }



        ////////////////////////////////////////////////////////////////////

        // Get Urls //
        // https://codereview.stackexchange.com/questions/139783/web-crawler-that-uses-task-parallel-library        
    }


    public class PageLinkReader
    {
        private readonly ICaldwellRequest _request;
        public PageLinkReader(ICaldwellRequest request)
        {
            _request = request;
        }

        public async Task<IEnumerable<string>> GetLinks(int level = 0)
        {
            if (level < 0)
                throw new ArgumentOutOfRangeException(nameof(level));

            var rootUrls = await GetPageLinks();

            if (level == 0)
                return rootUrls;

            return null;
        }

        private async Task<IEnumerable<string>> GetPageLinks()
        {
            try
            {
                HtmlWeb web = new HtmlWeb();
                var htmlDocument =  await web.LoadFromWebAsync(_request.Url);

                return htmlDocument.DocumentNode
                                   .Descendants("a")
                                   .Select(a => a.GetAttributeValue("href", null))
                                   .Where(u => !string.IsNullOrEmpty(u))
                                   .Distinct();
            }
            catch (Exception exception)
            {
                return Enumerable.Empty<string>();
            }
        }
    }
}
