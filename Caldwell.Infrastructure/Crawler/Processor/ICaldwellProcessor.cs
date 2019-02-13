using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Text;

namespace Caldwell.Infrastructure.Crawler.Processor
{
    public interface ICaldwellProcessor
    {
        void Process(HtmlDocument document);
    }
}
