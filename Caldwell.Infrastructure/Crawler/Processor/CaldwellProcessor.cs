using Caldwell.Core.Repository;
using Caldwell.Infrastructure.Models;
using HtmlAgilityPack;
using HtmlAgilityPack.CssSelectors.NetCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Caldwell.Infrastructure.Crawler.Processor
{
    public class CaldwellProcessor<TEntity> : ICaldwellProcessor<TEntity> where TEntity : class, IEntity
    {
        public async Task<IEnumerable<TEntity>> Process(HtmlDocument document)
        {            
            var titleNode = document.DocumentNode.SelectSingleNode("//*[@id='ozet']/div[1]/div/h1/a");

            var realTitle = titleNode.InnerText;
            var title = titleNode.Attributes["title"].Value;

            var mainSpecsNode = document.DocumentNode.SelectSingleNode("//*[@id='oncelikli']");
            //*[@id="oncelikli"]/div[1]/div[1]/div[1]

            //# oncelikli > div:nth-child(1) > div:nth-child(1) > div.row.row2
            var mainSpecValues = mainSpecsNode.QuerySelectorAll("div.row.row2 a"); // go to div row row
            var node2 = mainSpecsNode.QuerySelector("div.row.row1");


            // reflection
            object instance = Activator.CreateInstance(typeof(TEntity));
            TrySetProperty(instance, "Name", "new swn");
            TrySetProperty(instance, "CatalogBrandId", 1);
            TrySetProperty(instance, "CatalogTypeId", 1);

            var list = new List<TEntity>();
            list.Add(instance as TEntity);

            return list;

            // future
            // you can create custom attributes on Entity class and properties which stores xpaths
            // as per these atributes create entities with value of crawler's data
        }

        private void TrySetProperty(object obj, string property, object value)
        {
            var prop = obj.GetType().GetProperty(property, BindingFlags.Public | BindingFlags.Instance);
            if (prop != null && prop.CanWrite)
                prop.SetValue(obj, value, null);
        }


        // future
        // you can create custom attributes on Entity class and properties which stores xpaths
        // as per these atributes create entities with value of crawler's data
        public static Dictionary<string, string> GetCustomAttributes()
        {
            Dictionary<string, string> _dict = new Dictionary<string, string>();

            PropertyInfo[] props = typeof(TEntity).GetProperties();
            foreach (PropertyInfo prop in props)
            {
                object[] attrs = prop.GetCustomAttributes(true);
                foreach (object attr in attrs)
                {
                    var authAttr = attr as CustomAttribute;
                    if (authAttr != null)
                    {
                        string propName = prop.Name;
                        string auth = authAttr.Name;

                        _dict.Add(propName, auth);
                    }
                }
            }

            return _dict;
        }




        /////////////////////   EXAMPLE OF /////////////////////
        ///
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


        public void Crawle_Example()
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

    public class CustomAttribute : Attribute
    {
        public string Name { get; set; }
    }
}
