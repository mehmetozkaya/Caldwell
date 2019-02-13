using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Caldwell.Infrastructure.Crawler.Downloader
{
    public interface ICaldwellDownloader
    {
        CaldwellDownloaderType DownloderType { get; set; }
        string DownloadPath { get; set; }

        Task<HtmlDocument> Download(string crawlUrl);
    }
}
