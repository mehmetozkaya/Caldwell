using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Caldwell.Infrastructure.Crawler.Downloader
{
    public class CaldwellDownloader : ICaldwellDownloader
    {
        public void Download(string crawlUrl, CaldwellDownloaderType downloaderType, string fileName = null)
        {
            // if exist dont download again

            using (WebClient client = new WebClient()) // WebClient class inherits IDisposable
            {
                client.DownloadFile(crawlUrl, @"C:\mozk_delete\"+ fileName + ".html");

                // Or you can get the file content without saving it
                string htmlCode = client.DownloadString(crawlUrl);
            }
        }       
    }

    public enum CaldwellDownloaderType
    {
        FromFile,
        FromString,
        FromWeb
    }
}
