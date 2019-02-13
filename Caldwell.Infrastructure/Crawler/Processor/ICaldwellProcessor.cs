using Caldwell.Infrastructure.Models;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Caldwell.Infrastructure.Crawler.Processor
{
    public interface ICaldwellProcessor
    {
        Task<IEnumerable<Catalog>> Process(HtmlDocument document);
    }
}
