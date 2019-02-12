namespace Caldwell.Infrastructure.Crawler.Downloader
{
    public enum CaldwellDownloaderType
    {
        FromFile, // download to local file
        FromMemory, // without downloading to local
        FromWeb // read direct from web
    }
}
