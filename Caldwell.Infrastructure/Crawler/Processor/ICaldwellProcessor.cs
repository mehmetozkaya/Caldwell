using Caldwell.Infrastructure.Models;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Text;

namespace Caldwell.Infrastructure.Crawler.Processor
{
    public interface ICaldwellProcessor
    {
        Catalog Process(HtmlDocument document);
    }
}
