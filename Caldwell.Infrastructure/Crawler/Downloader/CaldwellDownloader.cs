using HtmlAgilityPack;
using System;
using System.Net;

namespace Caldwell.Infrastructure.Crawler.Downloader
{
    public class CaldwellDownloader : ICaldwellDownloader
    {
        public CaldwellDownloaderType DownloderType { get; set; }
        public string DownloadPath { get; set; }
        private string _localFilePath;

        public CaldwellDownloader()
        {
            
        }

        public HtmlDocument Download(string crawlUrl)
        {
            // if exist dont download again
            PrepareFilePath(crawlUrl);

            var existing = GetExistingFile(_localFilePath);
            if (existing != null)
                return existing;

            return DownloadInternal(crawlUrl);
        }

        private HtmlDocument DownloadInternal(string crawlUrl)
        {
            switch (DownloderType)
            {
                case CaldwellDownloaderType.FromFile:
                    using (WebClient client = new WebClient()) // WebClient class inherits IDisposable
                    {
                        client.DownloadFile(crawlUrl, _localFilePath);
                    }
                    return GetExistingFile(_localFilePath);
                    
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
            var parts = fileName.Split('/');
            var htmlpage = string.Empty;
            if (parts.Length > 0)
                htmlpage = parts[parts.Length - 1];

            _localFilePath = $"{DownloadPath}{htmlpage}";
        }

        private HtmlDocument GetExistingFile(string fullPath)
        {
            try
            {
                var htmlDocument = new HtmlDocument();
                htmlDocument.Load(fullPath);
                return htmlDocument;
            }
            catch (Exception)
            {                
            }

            return null;
        }

        
    }
}
