using HtmlAgilityPack;
using System;
using System.Net;
using System.Threading.Tasks;

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

        public async Task<HtmlDocument> Download(string crawlUrl)
        {
            // if exist dont download again
            PrepareFilePath(crawlUrl);

            var existing = await GetExistingFile(_localFilePath);
            if (existing != null)
                return existing;

            return await DownloadInternal(crawlUrl);
        }

        private async Task<HtmlDocument> DownloadInternal(string crawlUrl)
        {
            switch (DownloderType)
            {
                case CaldwellDownloaderType.FromFile:
                    using (WebClient client = new WebClient()) // WebClient class inherits IDisposable
                    {
                        client.DownloadFileAsync(new Uri(crawlUrl, true), _localFilePath);
                    }
                    return await GetExistingFile(_localFilePath);
                    
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

        private async Task<HtmlDocument> GetExistingFile(string fullPath)
        {
            try
            {
                var htmlDocument = new HtmlDocument();
                await Task.Run(() => htmlDocument.Load(fullPath));
                return htmlDocument;
            }
            catch (Exception)
            {                
            }

            return null;
        }

        
    }
}
