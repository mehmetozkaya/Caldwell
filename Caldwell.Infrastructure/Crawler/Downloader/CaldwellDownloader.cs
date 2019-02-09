using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace Caldwell.Infrastructure.Crawler.Downloader
{
    public class CaldwellDownloader : ICaldwellDownloader
    {
        private readonly CaldwellDownloaderType _downloaderType;

        public CaldwellDownloader(CaldwellDownloaderType downloaderType)
        {
            _downloaderType = downloaderType;
        }

        public HtmlDocument Download(string crawlUrl, string fileName = null)
        {
            // if exist dont download again
            if(!string.IsNullOrEmpty(fileName))
            {
                GetExistingFile(fileName);
            }

            switch (_downloaderType)
            {
                case CaldwellDownloaderType.FromFile:
                    DownloadToFile(crawlUrl, fileName);                    
                    break;
                case CaldwellDownloaderType.FromMemory:
                    DownloadToMemory(crawlUrl, fileName);
                    break;
                case CaldwellDownloaderType.FromWeb:
                    return DownloadToWeb(crawlUrl, fileName);
                    break;
                default:
                    break;
            }

         
        }

        private HtmlDocument DownloadToWeb(string crawlUrl, string fileName)
        {
            HtmlWeb web = new HtmlWeb();
            return web.Load(crawlUrl);
        }

        private void DownloadToMemory(string crawlUrl, string fileName)
        {
            using (WebClient client = new WebClient()) // WebClient class inherits IDisposable
            {
                // Or you can get the file content without saving it
                string htmlCode = client.DownloadString(crawlUrl);
            }
        }

        private void DownloadToFile(string crawlUrl, string fileName)
        {
            using (WebClient client = new WebClient()) // WebClient class inherits IDisposable
            {
                client.DownloadFile(crawlUrl, @"C:\mozk_delete\" + fileName + ".html");                
            }
        }

        private void GetExistingFile(string fileName)
        {            
            string htmlString = File.ReadAllText(fileName);
            HtmlDocument htmlDocument = 
        }
    }

    public enum CaldwellDownloaderType
    {
        FromFile, // download to local file
        FromMemory, // without downloading to local
        FromWeb // read direct from web
    }
}
