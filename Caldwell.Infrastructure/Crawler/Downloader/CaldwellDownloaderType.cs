namespace Caldwell.Infrastructure.Crawler.Downloader
{
    /// <summary>
    /// Type of the downloaders when crawler download source web
    /// </summary>
    public enum CaldwellDownloaderType
    {
        /// <summary>
        /// download to local file
        /// </summary>
        FromFile,
        /// <summary>
        /// without downloading to local
        /// </summary>
        FromMemory,
        /// <summary>
        /// read direct from web
        /// </summary>
        FromWeb
    }
}
