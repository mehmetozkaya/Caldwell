using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Text;

namespace Caldwell.Infrastructure.Crawler.Downloader
{
    public interface ICaldwellDownloader
    {
        CaldwellDownloaderType DownloderType { get; set; }
        string DownloadPath { get; set; }

        HtmlDocument Download(string crawlUrl);
    }
}
