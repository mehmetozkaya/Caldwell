using Caldwell.Infrastructure.Models;
using HtmlAgilityPack;
using HtmlAgilityPack.CssSelectors.NetCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Caldwell.Infrastructure.Crawler.Processor
{
    public class CaldwellProcessor : ICaldwellProcessor
    {
        public Catalog Process(HtmlDocument document)
        {            
            var titleNode = document.DocumentNode.SelectSingleNode("//*[@id='ozet']/div[1]/div/h1/a");

            var realTitle = titleNode.InnerText;
            var title = titleNode.Attributes["title"].Value;

            var mainSpecsNode = document.DocumentNode.SelectSingleNode("//*[@id='oncelikli']");
            //*[@id="oncelikli"]/div[1]/div[1]/div[1]

            //# oncelikli > div:nth-child(1) > div:nth-child(1) > div.row.row2
            var mainSpecValues = mainSpecsNode.QuerySelectorAll("div.row.row2 a"); // go to div row row
            var node2 = mainSpecsNode.QuerySelector("div.row.row1");


            return new Catalog();

        }     
    }
}
