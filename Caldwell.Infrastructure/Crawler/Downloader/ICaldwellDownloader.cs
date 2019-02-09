using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Text;

namespace Caldwell.Infrastructure.Crawler.Downloader
{
    public interface ICaldwellDownloader
    {
        HtmlDocument Download(string crawlUrl, string fileName = null);
    }
}
