using System;
using System.Collections.Generic;
using System.Text;

namespace Caldwell.Infrastructure.Crawler.Downloader
{
    public interface ICaldwellDownloader
    {
        void Download(string crawlUrl, CaldwellDownloaderType downloaderType, string fileName = null);
    }
}
