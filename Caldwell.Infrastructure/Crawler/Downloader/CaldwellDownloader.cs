using HtmlAgilityPack;
using System;
using System.Net;

namespace Caldwell.Infrastructure.Crawler.Downloader
{
    public class CaldwellDownloader : ICaldwellDownloader
    {
        public CaldwellDownloaderType DownloderType { get; set; }
        public string DownloadPath { get; set; }

        public CaldwellDownloader()
        {
        }

        public HtmlDocument Download(string crawlUrl, string fileName = null)
        {            
            // if exist dont download again
            if(!string.IsNullOrEmpty(fileName))
            {
                PrepareFilePath(fileName);

                var existing = GetExistingFile(LocalFilePath);
                if (existing != null)
                    return existing;
            }

            return DownloadInternal(crawlUrl);         
        }

        private HtmlDocument DownloadInternal(string crawlUrl)
        {
            switch (DownloderType)
            {
                case CaldwellDownloaderType.FromFile:
                    using (WebClient client = new WebClient()) // WebClient class inherits IDisposable
                    {
                        client.DownloadFile(crawlUrl, LocalFilePath);
                    }
                    return GetExistingFile(LocalFilePath);
                    
                case CaldwellDownloaderType.FromMemory:
                    var htmlDocument = new HtmlDocument();
                    using (WebClient client = new WebClient()) // WebClient class inherits IDisposable
                    {
                        // Or you can get the file content without saving it
                        string htmlCode = client.DownloadString(crawlUrl);
                        htmlDocument.LoadHtml(htmlCode);
                    }
                    return htmlDocument;
                    
                case CaldwellDownloaderType.FromWeb:
                    HtmlWeb web = new HtmlWeb();
                    return web.Load(crawlUrl);                                   
            }

            throw new InvalidOperationException("Can not load html file from given source.");
        }

        private void PrepareFilePath(string fileName)
        {
            LocalFilePath = $"{_mainPath}{fileName}.html";            
        }        

        private HtmlDocument GetExistingFile(string fullPath)
        {            
            //string htmlString = File.ReadAllText(fileName);
            var htmlDocument = new HtmlDocument();
            htmlDocument.Load(fullPath);
            return htmlDocument;
        }
    }
}
