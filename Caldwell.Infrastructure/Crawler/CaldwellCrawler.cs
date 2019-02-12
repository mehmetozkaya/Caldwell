using Caldwell.Core.Crawler;
using Caldwell.Infrastructure.Crawler.Downloader;
using Caldwell.Infrastructure.Crawler.Processor;
using Caldwell.Infrastructure.Crawler.Request;
using HtmlAgilityPack;
using HtmlAgilityPack.CssSelectors.NetCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

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

        public CaldwellCrawler()
        {
            //try
            //{
            //    HtmlWeb web = new HtmlWeb();
            //    _htmlDocument = web.Load(crawlUrl);
            //}
            //catch (Exception exception)
            //{
            //    throw new StackOverflowException($"Error when loading url.{exception.Message}");
            //}
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

            using (WebClient client = new WebClient()) // WebClient class inherits IDisposable
            {
                client.DownloadFile(_crawlUrl, @"C:\mozk_delete\localfile.html");

                // Or you can get the file content without saving it
                string htmlCode = client.DownloadString(_crawlUrl);
            }

            return this;
        }
        

        public void ReasonToSolve()
        {
            var titleNode = _htmlDocument.DocumentNode.SelectSingleNode("//*[@id='ozet']/div[1]/div/h1/a");

            var realTitle = titleNode.InnerText;
            var title = titleNode.Attributes["title"].Value;

            var mainSpecsNode = _htmlDocument.DocumentNode.SelectSingleNode("//*[@id='oncelikli']");
            //*[@id="oncelikli"]/div[1]/div[1]/div[1]
            
            //# oncelikli > div:nth-child(1) > div:nth-child(1) > div.row.row2
            var mainSpecValues = mainSpecsNode.QuerySelectorAll("div.row.row2 a"); // go to div row row
            //var node2 = mainSpecsNode.QuerySelector("div.row.row1");

        }

        public void CssReader()
        {
            // https://github.com/trenoncourt/HtmlAgilityPack.CssSelectors.NetCore

            var html = @"http://html-agility-pack.net/";
            // SELECTORS
            HtmlWeb web = new HtmlWeb();
            var htmlDoc = web.Load(html);

            IList<HtmlNode> nodes = htmlDoc.QuerySelectorAll("div .my-class[data-attr=123] > ul li");
            HtmlNode node = nodes[0].QuerySelector("p.with-this-class span[data-myattr]");

            // how to write css selector
            // https://www.w3schools.com/cssref/css_selectors.asp  -- https://www.w3schools.com/cssref/trysel.asp
            // https://www.w3schools.com/jsref/met_document_queryselector.asp
        }


        public void Crawle()
        {
            // https://html-agility-pack.net/documentation

            var html = @"http://html-agility-pack.net/";

            // SELECTORS
            HtmlWeb web = new HtmlWeb();
            var htmlDoc = web.Load(html);
            var node = htmlDoc.DocumentNode.SelectSingleNode("//head/title");

            Console.WriteLine("Node Name: " + node.Name + "\n" + node.OuterHtml);

            string name = htmlDoc.DocumentNode
                            .SelectNodes("//td/input")
                            .First()
                            .Attributes["value"].Value;

            // direct get first one
            string name2 = htmlDoc.DocumentNode
                            .SelectSingleNode("//td/input")
                            .Attributes["value"].Value;

            var htmlNodes = htmlDoc.DocumentNode.SelectNodes("//td/input");

            // MANUPLATORS

            var htmlNodes2 = htmlDoc.DocumentNode.SelectNodes("//body/h1");

            foreach (var node2 in htmlNodes)
            {
                Console.WriteLine(node2.InnerHtml);
                Console.WriteLine(node2.InnerText);
                Console.WriteLine(node2.OuterHtml);
            }

            HtmlNode parentNode = node.ParentNode;            
            Console.WriteLine(parentNode.Name);

            // TRAVERSING

            var htmlBody = htmlDoc.DocumentNode.SelectSingleNode("//body");
            HtmlNode firstChild = htmlBody.FirstChild;
            HtmlNode lastChild = htmlBody.LastChild;
            Console.WriteLine(firstChild.OuterHtml);

            HtmlNodeCollection childNodes = htmlBody.ChildNodes;
            foreach (var node3 in childNodes)
            {
                if (node3.NodeType == HtmlNodeType.Element)
                {
                    Console.WriteLine(node3.OuterHtml);
                }
            }

            //next sibling
            var node4 = htmlDoc.DocumentNode.SelectSingleNode("//body/h1");
            HtmlNode sibling = node4.NextSibling;

            while (sibling != null)
            {
                if (sibling.NodeType == HtmlNodeType.Element)
                    Console.WriteLine(sibling.OuterHtml);

                sibling = sibling.NextSibling;
            }

            // Ancestors -- atalar
            var node5 = htmlDoc.DocumentNode.SelectSingleNode("//b");

            foreach (var nNode in node5.Ancestors())
            {
                if (nNode.NodeType == HtmlNodeType.Element)
                {
                    Console.WriteLine(nNode.Name);
                }
            }

            // ancestor with matching name
            var node6 = htmlDoc.DocumentNode.SelectSingleNode("//b");
            foreach (var nNode in node6.Ancestors("body"))
            {
                if (nNode.NodeType == HtmlNodeType.Element)
                {
                    Console.WriteLine("Node name: " + nNode.Name);
                    Console.WriteLine(nNode.InnerText);
                }
            }

            // node6.Ancestors() -- ancs
            // node6.Ancestors("body") -- ancs filtered
            // node.AncestorsAndSelf() -- own self end ancs
            // node.AncestorsAndSelf("p") -- own self end ancs filtered

            // https://html-agility-pack.net/traversing
            //Ancestors()  Gets all the ancestors of the node.
            //Ancestors(String)   Gets ancestors with matching names.
            //AncestorsAndSelf()  Gets all anscestor nodes and the current node.
            //AncestorsAndSelf(String)    Gets all anscestor nodes and the current node with matching name.
            //DescendantNodes Gets all descendant nodes for this node and each of child nodes
            //DescendantNodesAndSelf  Returns a collection of all descendant nodes of this element, in document order
            //Descendants()   Gets all descendant nodes in enumerated list
            //Descendants(String) Get all descendant nodes with matching names
            //DescendantsAndSelf()    Returns a collection of all descendant nodes of this element, in document order
            //DescendantsAndSelf(String)  Gets all descendant nodes including this node
            //Element Gets first generation child node matching name
            //Elements    Gets matching first generation child nodes matching name
        }
    }
}
