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
            
            Regex regexLink = new Regex(".*akilli-telefonlar/.+");
            // regexLink.Match()
            
            // Getlinks filtered by Regex
            //Request.Regex

            //var getall = GetLinks(Request.Url, true, true, 2);


            var document = await Downloader.Download(Request.Url);
            var catalog = await Processor.Process(document);
            await Pipeline.Run(catalog);
        }



        ////////////////////////////////////////////////////////////////////

        // Get Urls //
        // https://codereview.stackexchange.com/questions/139783/web-crawler-that-uses-task-parallel-library

        public IEnumerable<string> getLinks(string url, bool hostMatch = true, bool validatePages = true, int level = 0)
        {
            string formattedUrl = url;
            if (string.IsNullOrEmpty(formattedUrl)) return Enumerable.Empty<string>();
            //download root url's
            IEnumerable<string> rootUrls = getSinglePageLinks(formattedUrl, hostMatch, validatePages);
            //download url's for each level
            for (int i = 0; i < level; i++)
            {
                rootUrls = rootUrls.Union(getManyPageLinks(rootUrls, hostMatch, validatePages));
            }
            return rootUrls;
        }

        private IEnumerable<string> getSinglePageLinks(string formattedUrl, bool hostMatch = true, bool validatePages = true)
        {
            Console.WriteLine("getSinglePageLinks process : " + formattedUrl);
            HtmlDocument doc = new HtmlWeb().Load(formattedUrl);
            var linkedPages = doc.DocumentNode.Descendants("a")
                                                .Select(a => a.GetAttributeValue("href", null))
                                                .Where(u => !String.IsNullOrEmpty(u))
                                                .Distinct();

            //hostMatch and validatePages left out
            return linkedPages;

        }

        private IEnumerable<string> getManyPageLinks(IEnumerable<string> rootUrls, bool hostMatch, bool validatePages)
        {
            List<Task> tasks = new List<Task>();
            List<List<string>> allLinks = new List<List<string>>();

            foreach (string rootUrl in rootUrls)
            {
                Console.WriteLine("Task working for : " + rootUrl);
                string rootUrlCopy = rootUrl; //required
                var task = Task.Factory.StartNew(() =>
                {
                    IEnumerable<string> taskResult = getSinglePageLinks(rootUrlCopy, hostMatch, validatePages);
                    return taskResult;
                });

                tasks.Add(task);
                allLinks.Add(task.Result.ToList());
            }
            Console.WriteLine("Task finisged");

            Task.WaitAll(tasks.ToArray());
            return allLinks.SelectMany(x => x).Distinct();
        }


        // second solution
        private async Task<IEnumerable<string>> GetAllPagesLinks(IEnumerable<string> rootUrls, bool hostMatch, bool validatePages)
        {
            var result = await Task.WhenAll(rootUrls.Select(url => GetPageLinks(url, hostMatch, validatePages)));
            return result.SelectMany(x => x).Distinct();
        }

        private async Task<IEnumerable<string>> GetPageLinks(string formattedUrl, bool hostMatch = true, bool validatePages = true)
        {
            var htmlDocument = new HtmlDocument();

            try
            {


                var client = new HttpClient();
                {
                    // htmlDocument.Load(await client.GetStringAsync(formattedUrl));
                }

                return htmlDocument.DocumentNode
                                   .Descendants("a")
                                   .Select(a => a.GetAttributeValue("href", null))
                                   .Where(u => !string.IsNullOrEmpty(u))
                                   .Distinct();
            }
            catch
            {
                return Enumerable.Empty<string>();
            }
        }

        private async Task<IEnumerable<string>> GetLinks(string url, bool hostMatch = true, bool validatePages = true, int level = 0)
        {
            if (level < 0)
                throw new ArgumentOutOfRangeException(nameof(level));

            string formattedUrl = url;

            if (string.IsNullOrEmpty(formattedUrl))
                return Enumerable.Empty<string>();

            var rootUrls = await GetPageLinks(formattedUrl, hostMatch, validatePages);

            if (level == 0)
                return rootUrls;

            var links = await GetAllPagesLinks(rootUrls, hostMatch, validatePages);

            var tasks = await Task.WhenAll(links.Select(link => GetLinks(link, hostMatch, validatePages, --level)));

            return tasks.SelectMany(l => l);
        }

        // third solution
        public ISet<string> GetNewLinks(string content)
        {
            Regex regexLink = new Regex("(?<=<a\\s*?href=(?:'|\"))[^'\"]*?(?=(?:'|\"))");

            ISet<string> newLinks = new HashSet<string>();
            foreach (var match in regexLink.Matches(content))
            {
                if (!newLinks.Contains(match.ToString()))
                    newLinks.Add(match.ToString());
            }

            return newLinks;
        }

        ////////////////////////////////////////////////////////////////////

    }
}
