using System;
using System.Collections.Generic;
using System.Text;

namespace Caldwell.Infrastructure.Crawler.Request
{
    public interface ICaldwellRequest
    {
        long TimeOut { get; set; }
    }
}
