using Caldwell.Infrastructure.Models;
using HtmlAgilityPack;
using HtmlAgilityPack.CssSelectors.NetCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Caldwell.Infrastructure.Crawler.Processor
{
    public class CaldwellProcessor : ICaldwellProcessor
    {
        public async Task<IEnumerable<Catalog>> Process(HtmlDocument document)
        {            
            var titleNode = document.DocumentNode.SelectSingleNode("//*[@id='ozet']/div[1]/div/h1/a");

            var realTitle = titleNode.InnerText;
            var title = titleNode.Attributes["title"].Value;

            var mainSpecsNode = document.DocumentNode.SelectSingleNode("//*[@id='oncelikli']");
            //*[@id="oncelikli"]/div[1]/div[1]/div[1]

            //# oncelikli > div:nth-child(1) > div:nth-child(1) > div.row.row2
            var mainSpecValues = mainSpecsNode.QuerySelectorAll("div.row.row2 a"); // go to div row row
            var node2 = mainSpecsNode.QuerySelector("div.row.row1");


            return new List<Catalog>();
        }



        // Get Urls //
        // https://codereview.stackexchange.com/questions/139783/web-crawler-that-uses-task-parallel-library

        public static IEnumerable<string> getLinks(string url, bool hostMatch = true, bool validatePages = true, int level = 0)
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

        private static IEnumerable<string> getSinglePageLinks(string formattedUrl, bool hostMatch = true, bool validatePages = true)
        {
            
            HtmlDocument doc = new HtmlWeb().Load(formattedUrl);
            var linkedPages = doc.DocumentNode.Descendants("a")
                                                .Select(a => a.GetAttributeValue("href", null))
                                                .Where(u => !String.IsNullOrEmpty(u))
                                                .Distinct();

            //hostMatch and validatePages left out
            return linkedPages;
           
        }

        private static IEnumerable<string> getManyPageLinks(IEnumerable<string> rootUrls, bool hostMatch, bool validatePages)
        {
            List<Task> tasks = new List<Task>();
            List<List<string>> allLinks = new List<List<string>>();

            foreach (string rootUrl in rootUrls)
            {
                string rootUrlCopy = rootUrl; //required
                var task = Task.Factory.StartNew(() =>
                {
                    IEnumerable<string> taskResult = getSinglePageLinks(rootUrlCopy, hostMatch, validatePages);
                    return taskResult;
                });

                tasks.Add(task);
                allLinks.Add(task.Result.ToList());
            }

            Task.WaitAll(tasks.ToArray());
            return allLinks.SelectMany(x => x).Distinct();
        }


        // second solution
        async static Task<IEnumerable<string>> GetAllPagesLinks(IEnumerable<string> rootUrls, bool hostMatch, bool validatePages)
        {
            var result = await Task.WhenAll(rootUrls.Select(url => GetPageLinks(url, hostMatch, validatePages)));
            return result.SelectMany(x => x).Distinct();
        }

        static async Task<IEnumerable<string>> GetPageLinks(string formattedUrl, bool hostMatch = true, bool validatePages = true)
        {
            var htmlDocument = new HtmlDocument();

            try
            {
                using (var client = new HttpClient())
                    htmlDocument.Load(await client.GetStringAsync(formattedUrl));

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

        async static Task<IEnumerable<string>> GetLinks(string url, bool hostMatch = true, bool validatePages = true, int level = 0)
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

    }
}
