using Caldwell.Core.Crawler;
using Caldwell.Infrastructure.Crawler.Downloader;
using Caldwell.Infrastructure.Crawler.Pipeline;
using Caldwell.Infrastructure.Crawler.Processor;
using Caldwell.Infrastructure.Crawler.Request;
using Caldwell.Infrastructure.Crawler.Scheduler;
using HtmlAgilityPack.CssSelectors.NetCore;
using System.Net.Http;
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
            
            var linkReader = new CaldwellPageLinkReader(Request);
            var links = await linkReader.GetLinks(Request.Url, 0);

            foreach (var url in links)
            {
                var document = await Downloader.Download(url);
                var catalog = await Processor.Process(document);
                await Pipeline.Run(catalog);
            }

        }



            
    }
}
