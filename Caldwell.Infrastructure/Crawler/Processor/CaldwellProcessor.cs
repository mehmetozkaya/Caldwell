using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Caldwell.Infrastructure.Crawler.Processor
{
    public class CaldwellProcessor : ICaldwellProcessor
    {
        public void Process(HtmlDocument document)
        {
            Task.Run(() => Console.WriteLine());
            
            throw new NotImplementedException();
        }


        //public void ReasonToSolve()
        //{
        //    var titleNode = _htmlDocument.DocumentNode.SelectSingleNode("//*[@id='ozet']/div[1]/div/h1/a");

        //    var realTitle = titleNode.InnerText;
        //    var title = titleNode.Attributes["title"].Value;

        //    var mainSpecsNode = _htmlDocument.DocumentNode.SelectSingleNode("//*[@id='oncelikli']");
        //    //*[@id="oncelikli"]/div[1]/div[1]/div[1]

        //    //# oncelikli > div:nth-child(1) > div:nth-child(1) > div.row.row2
        //    var mainSpecValues = mainSpecsNode.QuerySelectorAll("div.row.row2 a"); // go to div row row
        //    //var node2 = mainSpecsNode.QuerySelector("div.row.row1");
        //}
    }
}
