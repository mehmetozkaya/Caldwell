using System;
using System.Collections.Generic;
using System.Text;

namespace Caldwell.Infrastructure.Crawler.Request
{
    public interface ICaldwellRequest
    {
        string Url { get; set; }
        long TimeOut { get; set; }
    }
}
